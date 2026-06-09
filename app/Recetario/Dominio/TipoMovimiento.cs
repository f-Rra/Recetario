using System.ComponentModel;

namespace Dominio
{
    // Tabla auxiliar: tipo de movimiento de stock (entrada, salida, ajuste).
    public class TipoMovimiento
    {
        public int IdTipoMovimiento { get; set; }

        [DisplayName("Tipo")]
        public string Nombre { get; set; }
    }
}
