using System.ComponentModel;

namespace Dominio
{
    public class Proveedor
    {
        public int IdProveedor { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [DisplayName("Contacto")]
        public string Contacto { get; set; }

        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Dirección")]
        public string Direccion { get; set; }
    }
}
