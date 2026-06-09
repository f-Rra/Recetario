using System.ComponentModel;

namespace Dominio
{
    public class Persona
    {
        public int IdPersona { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Apellido")]
        public string Apellido { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        public int? IdClasificacion { get; set; }

        [DisplayName("Clasificación")]
        public string NombreClasificacion { get; set; }

        [DisplayName("Nombre Completo")]
        public string NombreCompleto
        {
            get { return $"{Nombre} {Apellido}"; }
        }
    }
}
