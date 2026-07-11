using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class CostoConfiguracion : IEntityTypeConfiguration<Costo>
{
    public void Configure(EntityTypeBuilder<Costo> builder)
    {
        builder.ToTable("Costos", t =>
        {
            t.HasCheckConstraint("CK_Costos_Porciones", "Porciones > 0");
        });
        builder.HasKey(c => c.IdCosto);
        builder.Property(c => c.CostoTotal).HasPrecision(12, 4);
        builder.Property(c => c.CostoUnitario).HasPrecision(12, 4);

        builder.HasOne(c => c.Receta)
               .WithMany()
               .HasForeignKey(c => c.IdReceta)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Usuario)
               .WithMany()
               .HasForeignKey(c => c.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
