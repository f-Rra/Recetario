using System.ComponentModel;

namespace Dominio
{
    public class Unidad
    {
        public int IdUnidad { get; set; }

        [DisplayName("Unidad")]
        public string Nombre { get; set; }

        [DisplayName("Abreviatura")]
        public string Abreviatura { get; set; }
    }
}
