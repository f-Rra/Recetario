using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class ProcedimientoConfiguracion : IEntityTypeConfiguration<Procedimiento>
{
    public void Configure(EntityTypeBuilder<Procedimiento> builder)
    {
        builder.ToTable("Procedimientos", t =>
        {
            t.HasCheckConstraint("CK_Procedimientos_NroPaso", "NroPaso > 0");
        });
        builder.HasKey(p => p.IdProcedimiento);
        builder.Property(p => p.Descripcion).IsRequired();

        // Un solo paso N por receta (el schema viejo permitía duplicados)
        builder.HasIndex(p => new { p.IdReceta, p.NroPaso }).IsUnique();

        builder.HasOne(p => p.Receta)
               .WithMany(r => r.Procedimientos)
               .HasForeignKey(p => p.IdReceta)
               .OnDelete(DeleteBehavior.Cascade);
    }
}
