using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecetarioMVC.Data;
using RecetarioMVC.Models;

namespace RecetarioMVC.Tests;

// El sembrador corre al arrancar la aplicación: crea los roles, los catálogos y el administrador
// inicial. La clave de ese administrador no puede estar escrita en el código, porque el repo es
// público y cualquiera podría entrar al sitio publicado: tiene que venir de la configuración.
public class DbSeederTests
{
    private const string Email = "admin@recetario.local";

    // Una aplicación mínima: la base en memoria, Identity y la configuración que se le pase.
    // El proveedor de servicios es el mismo tipo de objeto que recibe SeedAsync al arrancar.
    private static ServiceProvider CrearServicios(Dictionary<string, string?> configuracion, string? baseDeDatos = null)
    {
        var servicios = new ServiceCollection();
        servicios.AddLogging();
        servicios.AddSingleton<IConfiguration>(new ConfigurationBuilder().AddInMemoryCollection(configuracion).Build());

        var nombre = baseDeDatos ?? Guid.NewGuid().ToString();
        servicios.AddDbContext<ApplicationDbContext>(o => o.UseInMemoryDatabase(nombre));
        servicios.AddIdentity<ApplicationUser, IdentityRole>(o =>
            {
                o.Password.RequiredLength = 8;
                o.Password.RequireNonAlphanumeric = false;
                o.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

        return servicios.BuildServiceProvider();
    }

    private static Dictionary<string, string?> ConClave(string clave) => new() { ["DefaultAdmin:Password"] = clave };

    [Fact]
    public async Task SinClaveConfigurada_YSinAdmin_CortaElArranqueConUnMensajeClaro()
    {
        using var servicios = CrearServicios(new());

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => DbSeeder.SeedAsync(servicios));

        Assert.Contains("DefaultAdmin", error.Message);
        Assert.Contains("user-secrets", error.Message);
        Assert.Empty(servicios.GetRequiredService<UserManager<ApplicationUser>>().Users);
    }

    [Fact]
    public async Task ConClaveConfigurada_CreaElAdminConEsaClave_YNoConLaVieja()
    {
        using var servicios = CrearServicios(ConClave("ClaveDePrueba1"));

        await DbSeeder.SeedAsync(servicios);

        var usuarios = servicios.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await usuarios.FindByEmailAsync(Email);
        Assert.NotNull(admin);
        Assert.True(await usuarios.CheckPasswordAsync(admin, "ClaveDePrueba1"));
        Assert.False(await usuarios.CheckPasswordAsync(admin, "Admin123!"));
        Assert.True(await usuarios.IsInRoleAsync(admin, DbSeeder.RolAdmin));
    }

    [Fact]
    public async Task ConClaveConfigurada_TambienCargaLosCatalogosBase()
    {
        using var servicios = CrearServicios(ConClave("ClaveDePrueba1"));

        await DbSeeder.SeedAsync(servicios);

        var contexto = servicios.GetRequiredService<ApplicationDbContext>();
        Assert.NotEmpty(await contexto.Clasificaciones.ToListAsync());
        Assert.NotEmpty(await contexto.Unidades.ToListAsync());
    }

    [Fact]
    public async Task ConElAdminYaCreado_NoPideLaClaveDeNuevo()
    {
        // Es lo que pasa en un servidor que ya arrancó una vez, o en tu base de desarrollo
        var baseDeDatos = Guid.NewGuid().ToString();
        using (var primera = CrearServicios(ConClave("ClaveDePrueba1"), baseDeDatos))
            await DbSeeder.SeedAsync(primera);

        using var segunda = CrearServicios(new(), baseDeDatos);
        await DbSeeder.SeedAsync(segunda);

        var usuarios = segunda.GetRequiredService<UserManager<ApplicationUser>>();
        var admin = await usuarios.FindByEmailAsync(Email);
        Assert.True(await usuarios.CheckPasswordAsync(admin!, "ClaveDePrueba1"));
    }

    [Fact]
    public async Task ConUnaClaveQueNoCumpleLaPolitica_CortaElArranqueConElMotivo()
    {
        using var servicios = CrearServicios(ConClave("corta"));

        var error = await Assert.ThrowsAsync<InvalidOperationException>(() => DbSeeder.SeedAsync(servicios));

        Assert.StartsWith("No se pudo crear el administrador", error.Message);
        Assert.Empty(servicios.GetRequiredService<UserManager<ApplicationUser>>().Users);
    }
}
