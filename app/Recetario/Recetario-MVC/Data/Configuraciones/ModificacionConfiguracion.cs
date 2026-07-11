using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class ModificacionConfiguracion : IEntityTypeConfiguration<Modificacion>
{
    public void Configure(EntityTypeBuilder<Modificacion> builder)
    {
        builder.ToTable("Modificaciones", t =>
        {
            t.HasCheckConstraint("CK_Modificaciones_Cantidad", "Cantidad > 0");
        });
        builder.HasKey(m => m.IdModificacion);
        builder.Property(m => m.Cantidad).HasPrecision(10, 4);

        builder.HasOne(m => m.Comanda)
               .WithMany(c => c.Modificaciones)
               .HasForeignKey(m => m.IdComanda)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(m => m.IngredienteOriginal)
               .WithMany()
               .HasForeignKey(m => m.IdIngredienteOriginal)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.IngredienteReemplazo)
               .WithMany()
               .HasForeignKey(m => m.IdIngredienteReemplazo)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(m => m.Unidad)
               .WithMany()
               .HasForeignKey(m => m.IdUnidad)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
