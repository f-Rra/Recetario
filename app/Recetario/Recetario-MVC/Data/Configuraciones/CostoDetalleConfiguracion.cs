using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class CostoDetalleConfiguracion : IEntityTypeConfiguration<CostoDetalle>
{
    public void Configure(EntityTypeBuilder<CostoDetalle> builder)
    {
        builder.ToTable("CostosDetalle");
        builder.HasKey(d => d.IdCostoDetalle);
        builder.Property(d => d.CantBruta).HasPrecision(10, 4);
        builder.Property(d => d.CostoUnitario).HasPrecision(12, 4);
        builder.Property(d => d.Subtotal).HasPrecision(12, 4);

        builder.HasOne(d => d.Costo)
               .WithMany(c => c.Detalles)
               .HasForeignKey(d => d.IdCosto)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.Ingrediente)
               .WithMany()
               .HasForeignKey(d => d.IdIngrediente)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
