using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class IngredienteRecetaConfiguracion : IEntityTypeConfiguration<IngredienteReceta>
{
    public void Configure(EntityTypeBuilder<IngredienteReceta> builder)
    {
        builder.ToTable("IngredientesReceta", t =>
        {
            t.HasCheckConstraint("CK_IngredientesReceta_Rendimiento", "Rendimiento > 0 AND Rendimiento <= 100");
            t.HasCheckConstraint("CK_IngredientesReceta_CantNeta", "CantNeta > 0");
        });
        builder.HasKey(ir => new { ir.IdReceta, ir.IdIngrediente });
        builder.Property(ir => ir.CantNeta).HasPrecision(10, 4);
        builder.Property(ir => ir.Rendimiento).HasPrecision(5, 2);
        builder.Property(ir => ir.CantBruta).HasPrecision(10, 4);

        builder.HasOne(ir => ir.Receta)
               .WithMany(r => r.Ingredientes)
               .HasForeignKey(ir => ir.IdReceta)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ir => ir.Ingrediente)
               .WithMany()
               .HasForeignKey(ir => ir.IdIngrediente)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ir => ir.Unidad)
               .WithMany()
               .HasForeignKey(ir => ir.IdUnidad)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
