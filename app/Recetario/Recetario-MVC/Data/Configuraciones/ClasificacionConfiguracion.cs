using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class ClasificacionConfiguracion : IEntityTypeConfiguration<Clasificacion>
{
    public void Configure(EntityTypeBuilder<Clasificacion> builder)
    {
        builder.ToTable("Clasificaciones");
        builder.HasKey(c => c.IdClasificacion);
        builder.Property(c => c.Nombre).HasMaxLength(100).IsRequired();
        builder.HasIndex(c => c.Nombre).IsUnique();
    }
}
