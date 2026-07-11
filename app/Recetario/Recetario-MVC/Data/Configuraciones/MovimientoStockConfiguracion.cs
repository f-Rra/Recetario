using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class MovimientoStockConfiguracion : IEntityTypeConfiguration<MovimientoStock>
{
    public void Configure(EntityTypeBuilder<MovimientoStock> builder)
    {
        builder.ToTable("MovimientosStock", t =>
        {
            t.HasCheckConstraint("CK_MovimientosStock_Cantidad", "Cantidad > 0");
        });
        builder.HasKey(m => m.IdMovimiento);
        builder.Property(m => m.Cantidad).HasPrecision(10, 4);
        builder.Property(m => m.Observaciones).HasMaxLength(255);
        builder.Property(m => m.Fecha).HasDefaultValueSql("GETDATE()");

        builder.HasOne(m => m.Ingrediente)
               .WithMany()
               .HasForeignKey(m => m.IdIngrediente)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Unidad)
               .WithMany()
               .HasForeignKey(m => m.IdUnidad)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Usuario)
               .WithMany()
               .HasForeignKey(m => m.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
