using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class PasoComandaConfiguracion : IEntityTypeConfiguration<PasoComanda>
{
    public void Configure(EntityTypeBuilder<PasoComanda> builder)
    {
        builder.ToTable("PasosComanda");
        builder.HasKey(p => p.IdPasoComanda);
        builder.Property(p => p.Descripcion).IsRequired();

        // Un solo paso N por comanda, igual que en el procedimiento de la receta
        builder.HasIndex(p => new { p.IdComanda, p.NroPaso }).IsUnique();

        builder.HasOne(p => p.Comanda)
               .WithMany(c => c.Pasos)
               .HasForeignKey(p => p.IdComanda)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
