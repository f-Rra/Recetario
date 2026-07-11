using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class UnidadConfiguracion : IEntityTypeConfiguration<Unidad>
{
    public void Configure(EntityTypeBuilder<Unidad> builder)
    {
        builder.ToTable("Unidades");
        builder.HasKey(u => u.IdUnidad);
        builder.Property(u => u.Nombre).HasMaxLength(50).IsRequired();
        builder.Property(u => u.Abreviatura).HasMaxLength(10).IsRequired();
        builder.HasIndex(u => u.Nombre).IsUnique();
        builder.HasIndex(u => u.Abreviatura).IsUnique();
    }
}
