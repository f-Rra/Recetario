using System;
using System.ComponentModel;

namespace Dominio
{
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

        public int IdUsuario { get; set; }

        public int IdPersona { get; set; }

        [DisplayName("Responsable")]
        public string NombrePersona { get; set; }
    }
}
