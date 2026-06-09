using System.ComponentModel;

namespace Dominio
{
    public class Modificacion
    {
        public int IdModificacion { get; set; }

        public int IdComanda { get; set; }

        public int IdTipoModificacion { get; set; }

        [DisplayName("Tipo")]
        public string Tipo { get; set; }

        public int? IdIngredienteOriginal { get; set; }

        [DisplayName("Ingrediente Original")]
        public string IngredienteOriginal { get; set; }

        public int? IdIngredienteReemplazo { get; set; }

        [DisplayName("Ingrediente Reemplazo")]
        public string IngredienteReemplazo { get; set; }

        [DisplayName("Cantidad")]
        public decimal Cantidad { get; set; }

        public int IdUnidad { get; set; }

        [DisplayName("Unidad")]
        public string Abreviatura { get; set; }
    }
}
