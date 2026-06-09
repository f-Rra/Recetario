using System.ComponentModel;

namespace Dominio
{
    // Datos de acceso al sistema. Vinculado a la persona real por FK (IdPersona).
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public int IdPersona { get; set; }

        public string Password { get; set; }

        [DisplayName("Rol")]
        public string Rol { get; set; }

        // La persona asociada: la trae el Mapper para mostrar nombre/apellido tras el login.
        public Persona Persona { get; set; }
    }
}
