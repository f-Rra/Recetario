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
        await context.Database.MigrateAsync();

        await SeedRolesAsync(services.GetRequiredService<RoleManager<IdentityRole>>());
        await SeedAdminAsync(services.GetRequiredService<UserManager<ApplicationUser>>());
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

    private static async Task SeedAdminAsync(UserManager<ApplicationUser> userManager)
    {
        const string email = "admin@recetario.local";
        if (await userManager.FindByEmailAsync(email) is not null)
            return;

        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true,
            Nombre = "Administrador",
            Apellido = "Sistema"
        };

        // Contraseña inicial de desarrollo; cambiarla en el primer login real
        var resultado = await userManager.CreateAsync(admin, "Admin123!");
        if (resultado.Succeeded)
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
