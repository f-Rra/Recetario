using System.ComponentModel;

namespace Dominio
{
    public class IngredienteReceta
    {
        public int IdReceta { get; set; }

        public int IdIngrediente { get; set; }

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
