using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data;

public static class DbSeeder
{
    public const string RolAdmin = "Admin";
    public const string RolCocina = "Cocina";

    public static async Task SeedAsync(IServiceProvider services)
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // Las migraciones solo existen en una base relacional (SQL Server); en los tests, que usan una
        // base en memoria, no hay nada que migrar
        if (context.Database.IsRelational())
            await context.Database.MigrateAsync();

        await SeedRolesAsync(services.GetRequiredService<RoleManager<IdentityRole>>());
        await SeedAdminAsync(
            services.GetRequiredService<UserManager<ApplicationUser>>(),
            services.GetRequiredService<IConfiguration>()["DefaultAdmin:Password"]);
        await SeedCatalogosAsync(context);
    }

    private static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
    {
        foreach (var rol in new[] { RolAdmin, RolCocina })
        {
            if (!await roleManager.RoleExistsAsync(rol))
                await roleManager.CreateAsync(new IdentityRole(rol));
        }
    }

    private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager, string? clave)
    {
        const string email = "admin@recetario.local";
        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        // La clave no puede estar escrita acá: el repositorio es público y cualquiera podría entrar
        // al sitio publicado como administrador. Solo hace falta la primera vez; si el admin ya
        // existe (un servidor que ya arrancó, o tu base de desarrollo) no se pide nada.
        if (string.IsNullOrWhiteSpace(clave))
            throw new InvalidOperationException(
                "Falta la clave del administrador. En tu máquina, corré esto en la carpeta Recetario-MVC: " +
                "dotnet user-secrets set \"DefaultAdmin:Password\" \"una-clave-de-8-o-mas-caracteres\". " +
                "En un servidor, definí la variable de entorno DefaultAdmin__Password.");

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            Nombre = "Administrador",
            Apellido = "Sistema"
        };

        // Si Identity la rechaza (por ejemplo, por corta) se corta el arranque en lugar de seguir
        // sin administrador y sin avisar
        var resultado = await userManager.CreateAsync(admin, clave);
        if (!resultado.Succeeded)
            throw new InvalidOperationException("No se pudo crear el administrador: " +
                string.Join(" ", resultado.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(admin, RolAdmin);
    }

    private static async Task SeedCatalogosAsync(ApplicationDbContext context)
    {
        if (!await context.Clasificaciones.AnyAsync())
        {
            context.Clasificaciones.AddRange(
                new Clasificacion { Nombre = "Entrada" },
                new Clasificacion { Nombre = "Plato Principal" },
                new Clasificacion { Nombre = "Postre" },
                new Clasificacion { Nombre = "Decoración" },
                new Clasificacion { Nombre = "Ensalada" },
                new Clasificacion { Nombre = "Salsa" });
        }

        if (!await context.Unidades.AnyAsync())
        {
            context.Unidades.AddRange(
                new Unidad { Nombre = "Kilogramo", Abreviatura = "kg" },
                new Unidad { Nombre = "Gramo", Abreviatura = "g" },
                new Unidad { Nombre = "Litro", Abreviatura = "L" },
                new Unidad { Nombre = "Mililitro", Abreviatura = "ml" },
                new Unidad { Nombre = "Unidad", Abreviatura = "u" });
        }

        await context.SaveChangesAsync();
    }
}
