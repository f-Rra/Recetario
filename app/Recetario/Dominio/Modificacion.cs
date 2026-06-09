using System.ComponentModel;

namespace Dominio
{
    // Cambio de ingredientes sobre una comanda (sustitución, adición o eliminación).
    // El trigger trg_ActualizarStockModificacion ajusta el stock al insertarse.
    public class Modificacion
    {
        public int IdModificacion { get; set; }

        public int IdComanda { get; set; }

        public int IdTipoModificacion { get; set; }

        // Dato denormalizado que trae el Mapper (nombre del tipo de modificación).
        [DisplayName("Tipo")]
        public string Tipo { get; set; }

        // Ingrediente que se quita (null si es una adición).
        public int? IdIngredienteOriginal { get; set; }

        [DisplayName("Ingrediente Original")]
        public string IngredienteOriginal { get; set; }

        // Ingrediente que se agrega (null si es una eliminación).
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
