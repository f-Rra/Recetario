using System.ComponentModel;

namespace Dominio
{
    // Ingrediente que compone una receta, con su cantidad y rendimiento.
    // CantBruta = CantNeta / (Rendimiento / 100): contempla la merma.
    public class IngredienteReceta
    {
        public int IdReceta { get; set; }

        public int IdIngrediente { get; set; }

        // Dato denormalizado que trae el Mapper para mostrar el ingrediente en grillas.
        [DisplayName("Ingrediente")]
        public string NombreIngrediente { get; set; }

        [DisplayName("Cant. Neta")]
        public decimal CantNeta { get; set; }

        [DisplayName("Rendimiento (%)")]
        public decimal Rendimiento { get; set; }

        [DisplayName("Cant. Bruta")]
        public decimal CantBruta { get; set; }

        public int IdUnidad { get; set; }

        [DisplayName("Unidad")]
        public string Abreviatura { get; set; }
    }
}
