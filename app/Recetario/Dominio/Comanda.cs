using System;
using System.ComponentModel;

namespace Dominio
{
    // Orden de cocina: una receta a preparar en una fecha, con el integrante responsable.
    // El responsable (IdPersona) lo asigna sp_RegistrarComanda según la clasificación de la receta.
    public class Comanda
    {
        public int IdComanda { get; set; }

        public int IdReceta { get; set; }

        [DisplayName("Receta")]
        public string NombreReceta { get; set; }

        [DisplayName("Fecha")]
        public DateTime Fecha { get; set; }

        [DisplayName("Porciones")]
        public int Porciones { get; set; }

        // Usuario que registró la comanda (líder de cocina).
        public int IdUsuario { get; set; }

        // Integrante de cocina responsable, asignado automáticamente.
        public int IdPersona { get; set; }

        [DisplayName("Responsable")]
        public string NombrePersona { get; set; }
    }
}
