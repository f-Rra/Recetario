using System.ComponentModel;

namespace Dominio
{
    // Categoría de cocina compartida por recetas y personas (sector/especialidad).
    public class Clasificacion
    {
        public int IdClasificacion { get; set; }

        [DisplayName("Clasificación")]
        public string Nombre { get; set; }
    }
}
