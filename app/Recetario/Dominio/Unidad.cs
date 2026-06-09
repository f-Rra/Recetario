using System.ComponentModel;

namespace Dominio
{
    // Unidad de medida de ingredientes (kg, g, L, ml, u).
    public class Unidad
    {
        public int IdUnidad { get; set; }

        [DisplayName("Unidad")]
        public string Nombre { get; set; }

        [DisplayName("Abreviatura")]
        public string Abreviatura { get; set; }
    }
}
