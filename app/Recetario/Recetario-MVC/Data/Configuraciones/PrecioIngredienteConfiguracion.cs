using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class PrecioIngredienteConfiguracion : IEntityTypeConfiguration<PrecioIngrediente>
{
    public void Configure(EntityTypeBuilder<PrecioIngrediente> builder)
    {
        builder.ToTable("PreciosIngrediente", t =>
        {
            t.HasCheckConstraint("CK_PreciosIngrediente_Precio", "Precio > 0");
        });
        builder.HasKey(p => p.IdPrecio);
        builder.Property(p => p.Precio).HasPrecision(12, 4);

        // El costeo busca el último precio vigente por ingrediente
        builder.HasIndex(p => new { p.IdIngrediente, p.FechaVigencia });

        builder.HasOne(p => p.Ingrediente)
               .WithMany(i => i.Precios)
               .HasForeignKey(p => p.IdIngrediente)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Proveedor)
               .WithMany(pr => pr.Precios)
               .HasForeignKey(p => p.IdProveedor)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
