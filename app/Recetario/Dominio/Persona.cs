using System.ComponentModel;

namespace Dominio
{
    // Persona real del sistema: integrantes de cocina y usuarios con acceso.
    // Reemplaza a la antigua entidad Equipo (feedback Abel Faure).
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

        // Nullable: una persona puede no tener clasificación (ej. el admin).
        public int? IdClasificacion { get; set; }

        // Dato denormalizado que trae el Mapper para mostrar en grillas sin otro JOIN en la app.
        [DisplayName("Clasificación")]
        public string NombreClasificacion { get; set; }

        [DisplayName("Nombre Completo")]
        public string NombreCompleto
        {
            get { return $"{Nombre} {Apellido}"; }
        }
    }
}
