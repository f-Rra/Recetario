using System.ComponentModel;

namespace Dominio
{
    public class Ingrediente
    {
        public int IdIngrediente { get; set; }

        [DisplayName("Código")]
        public string Codigo { get; set; }

        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        public int IdUnidad { get; set; }

        [DisplayName("Unidad")]
        public string Abreviatura { get; set; }

        [DisplayName("Stock Actual")]
        public decimal StockActual { get; set; }

        [DisplayName("Stock Mínimo")]
        public decimal StockMinimo { get; set; }

        [DisplayName("Bajo Mínimo")]
        public bool BajoMinimo
        {
            get { return StockActual < StockMinimo; }
        }
    }
}
