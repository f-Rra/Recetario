using System.ComponentModel;

namespace Dominio
{
    public class Receta
    {
        public int IdReceta { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }

        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        public int IdClasificacion { get; set; }

        // Dato denormalizado que trae el Mapper para mostrar la clasificación en grillas.
        [DisplayName("Clasificación")]
        public string NombreClasificacion { get; set; }

        [DisplayName("Porciones Base")]
        public int PorcionesBase { get; set; }

        [DisplayName("Activo")]
        public bool Activo { get; set; }

        // Ruta del archivo de imagen de la receta (puede ser nula).
        public string Imagen { get; set; }
    }
}
