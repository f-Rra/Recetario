using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class IngredienteConfiguracion : IEntityTypeConfiguration<Ingrediente>
{
    public void Configure(EntityTypeBuilder<Ingrediente> builder)
    {
        builder.ToTable("Ingredientes", t =>
        {
            t.HasCheckConstraint("CK_Ingredientes_StockMinimo", "StockMinimo >= 0");
        });
        builder.HasKey(i => i.IdIngrediente);
        builder.Property(i => i.Codigo).HasMaxLength(20).IsRequired();
        builder.Property(i => i.Descripcion).HasMaxLength(100).IsRequired();
        builder.Property(i => i.StockActual).HasPrecision(10, 4);
        builder.Property(i => i.StockMinimo).HasPrecision(10, 4);
        builder.HasIndex(i => i.Codigo).IsUnique();

        builder.HasOne(i => i.Unidad)
               .WithMany()
               .HasForeignKey(i => i.IdUnidad)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
