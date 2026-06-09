using System.ComponentModel;

namespace Dominio
{
    public class TipoModificacion
    {
        public int IdTipoModificacion { get; set; }

        [DisplayName("Tipo")]
        public string Nombre { get; set; }
    }
}
