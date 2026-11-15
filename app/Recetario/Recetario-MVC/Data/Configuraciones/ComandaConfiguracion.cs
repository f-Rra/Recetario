using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RecetarioMVC.Models;

namespace RecetarioMVC.Data.Configuraciones;

public class ComandaConfiguracion : IEntityTypeConfiguration<Comanda>
{
    public void Configure(EntityTypeBuilder<Comanda> builder)
    {
        builder.ToTable("Comandas", t =>
        {
            t.HasCheckConstraint("CK_Comandas_Porciones", "Porciones > 0");
        });
        builder.HasKey(c => c.IdComanda);

        // El listado de comandas y el tablero filtran por rango de fechas y ordenan de la
        // más nueva a la más vieja: sin este índice, cada consulta recorre la tabla entera
        builder.HasIndex(c => c.Fecha);

        builder.HasOne(c => c.Receta)
               .WithMany()
               .HasForeignKey(c => c.IdReceta)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Usuario)
               .WithMany()
               .HasForeignKey(c => c.UsuarioId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(c => c.Persona)
               .WithMany()
               .HasForeignKey(c => c.IdPersona)
               .OnDelete(DeleteBehavior.Restrict);
    }
}
