using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Clasificacion> Clasificaciones => Set<Clasificacion>();
    public DbSet<Unidad> Unidades => Set<Unidad>();
    public DbSet<Persona> Personas => Set<Persona>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Ingrediente> Ingredientes => Set<Ingrediente>();
    public DbSet<PrecioIngrediente> PreciosIngrediente => Set<PrecioIngrediente>();
    public DbSet<Receta> Recetas => Set<Receta>();
    public DbSet<IngredienteReceta> IngredientesReceta => Set<IngredienteReceta>();
    public DbSet<Procedimiento> Procedimientos => Set<Procedimiento>();
    public DbSet<Comanda> Comandas => Set<Comanda>();
    public DbSet<Modificacion> Modificaciones => Set<Modificacion>();
    public DbSet<Costo> Costos => Set<Costo>();
    public DbSet<CostoDetalle> CostosDetalle => Set<CostoDetalle>();
    public DbSet<MovimientoStock> MovimientosStock => Set<MovimientoStock>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
