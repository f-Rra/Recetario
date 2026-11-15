using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using RecetarioMVC.Controllers;
using RecetarioMVC.Data;
using RecetarioMVC.Models;

namespace RecetarioMVC.Tests;

// Desactivar a un usuario tiene que dejarlo afuera aunque ya haya iniciado sesión.
// Identity invalida las sesiones abiertas cuando cambia el sello de seguridad del usuario,
// así que lo que se comprueba acá es que desactivar cambie ese sello.
public class UsuariosControllerTests
{
    private sealed class TempDataEnMemoria : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>();
        public void SaveTempData(HttpContext context, IDictionary<string, object> values) { }
    }

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);
        context.Users.AddRange(
            new ApplicationUser { Id = "admin", UserName = "admin@test", Nombre = "Ana", Apellido = "Admin", SecurityStamp = "sello-admin" },
            new ApplicationUser { Id = "cocina", UserName = "cocina@test", Nombre = "Coco", Apellido = "Cocina", SecurityStamp = "sello-cocina" });
        context.SaveChanges();
        return context;
    }

    // El controlador con el Admin como usuario que hace el pedido
    private static UsuariosController CrearControlador(ApplicationDbContext context)
    {
        var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(context),
            Options.Create(new IdentityOptions()),
            new PasswordHasher<ApplicationUser>(),
            [], [],
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            null!,
            NullLogger<UserManager<ApplicationUser>>.Instance);

        var httpContext = new DefaultHttpContext
        {
            User = new ClaimsPrincipal(new ClaimsIdentity(
                [new Claim(ClaimTypes.NameIdentifier, "admin")], "Test"))
        };

        return new UsuariosController(userManager)
        {
            ControllerContext = new ControllerContext { HttpContext = httpContext },
            TempData = new TempDataDictionary(httpContext, new TempDataEnMemoria())
        };
    }

    [Fact]
    public async Task Desactivar_CambiaElSelloParaCerrarLasSesionesAbiertas()
    {
        using var context = CrearContexto();
        var controlador = CrearControlador(context);

        await controlador.CambiarEstado("cocina");

        var usuario = await context.Users.AsNoTracking().SingleAsync(u => u.Id == "cocina");
        Assert.False(usuario.Activo);
        Assert.NotEqual("sello-cocina", usuario.SecurityStamp);
    }

    [Fact]
    public async Task Reactivar_VuelveAActivarSinTocarElSello()
    {
        using var context = CrearContexto();
        var controlador = CrearControlador(context);
        await controlador.CambiarEstado("cocina");
        var selloAlDesactivar = (await context.Users.AsNoTracking().SingleAsync(u => u.Id == "cocina")).SecurityStamp;

        await controlador.CambiarEstado("cocina");

        var usuario = await context.Users.AsNoTracking().SingleAsync(u => u.Id == "cocina");
        Assert.True(usuario.Activo);
        Assert.Equal(selloAlDesactivar, usuario.SecurityStamp);
    }

    [Fact]
    public async Task DesactivarseAUnoMismo_NoCambiaNada()
    {
        using var context = CrearContexto();
        var controlador = CrearControlador(context);

        await controlador.CambiarEstado("admin");

        var admin = await context.Users.AsNoTracking().SingleAsync(u => u.Id == "admin");
        Assert.True(admin.Activo);
        Assert.Equal("sello-admin", admin.SecurityStamp);
    }
}
