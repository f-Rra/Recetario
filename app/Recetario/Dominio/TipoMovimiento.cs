using System.ComponentModel;

namespace Dominio
{
    public class TipoMovimiento
    {
        public int IdTipoMovimiento { get; set; }

        [DisplayName("Tipo")]
        public string Nombre { get; set; }
    }
}
