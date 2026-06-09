using System.ComponentModel;

namespace Dominio
{
    public class Usuario
    {
        public int IdUsuario { get; set; }

        public int IdPersona { get; set; }

        public string Password { get; set; }

        [DisplayName("Rol")]
        public string Rol { get; set; }

        public Persona Persona { get; set; }
    }
}
