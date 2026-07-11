using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class PersonaConfiguracion : IEntityTypeConfiguration<Persona>
{
    public void Configure(EntityTypeBuilder<Persona> builder)
    {
        builder.ToTable("Personas");
        builder.HasKey(p => p.IdPersona);
        builder.Property(p => p.Nombre).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Apellido).HasMaxLength(100).IsRequired();
        builder.Property(p => p.Email).HasMaxLength(150);
        builder.Property(p => p.Telefono).HasMaxLength(20);

        builder.HasOne(p => p.Clasificacion)
               .WithMany(c => c.Personas)
               .HasForeignKey(p => p.IdClasificacion)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
