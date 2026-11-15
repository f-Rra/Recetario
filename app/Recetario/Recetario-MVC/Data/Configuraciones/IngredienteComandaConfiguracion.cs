using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class IngredienteComandaConfiguracion : IEntityTypeConfiguration<IngredienteComanda>
{
    public void Configure(EntityTypeBuilder<IngredienteComanda> builder)
    {
        builder.ToTable("IngredientesComanda");
        builder.HasKey(i => i.IdIngredienteComanda);
        builder.Property(i => i.Cantidad).HasPrecision(10, 4);

        // Sin clave única por (comanda, ingrediente): el mismo ingrediente puede figurar
        // dos veces, por ejemplo si la receta ya lo lleva y además se agrega más
        builder.HasOne(i => i.Comanda)
               .WithMany(c => c.Ingredientes)
               .HasForeignKey(i => i.IdComanda)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Ingrediente)
               .WithMany()
               .HasForeignKey(i => i.IdIngrediente)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(i => i.Unidad)
               .WithMany()
               .HasForeignKey(i => i.IdUnidad)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
