using System.ComponentModel;

namespace Dominio
{
    public class Clasificacion
    {
        public int IdClasificacion { get; set; }

        [DisplayName("Clasificación")]
        public string Nombre { get; set; }
    }
}
