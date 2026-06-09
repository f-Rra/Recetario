using System.ComponentModel;

namespace Dominio
{
    // Tabla auxiliar: tipo de modificación sobre una comanda (sustitucion, adicion, eliminacion).
    public class TipoModificacion
    {
        public int IdTipoModificacion { get; set; }

        [DisplayName("Tipo")]
        public string Nombre { get; set; }
    }
}
