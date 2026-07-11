using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class RecetaConfiguracion : IEntityTypeConfiguration<Receta>
{
    public void Configure(EntityTypeBuilder<Receta> builder)
    {
        builder.ToTable("Recetas", t =>
        {
            t.HasCheckConstraint("CK_Recetas_PorcionesBase", "PorcionesBase > 0");
        });
        builder.HasKey(r => r.IdReceta);
        builder.Property(r => r.Codigo).HasMaxLength(20).IsRequired();
        builder.Property(r => r.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(r => r.Imagen).HasMaxLength(255);
        builder.HasIndex(r => r.Codigo).IsUnique();

        builder.HasOne(r => r.Clasificacion)
               .WithMany(c => c.Recetas)
               .HasForeignKey(r => r.IdClasificacion)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
