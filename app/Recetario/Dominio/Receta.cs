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

        [DisplayName("Clasificación")]
        public string NombreClasificacion { get; set; }

        [DisplayName("Porciones Base")]
        public int PorcionesBase { get; set; }

        [DisplayName("Activo")]
        public bool Activo { get; set; }

        public string Imagen { get; set; }
    }
}
