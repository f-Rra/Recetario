using System;
using System.ComponentModel;

namespace Dominio
{
    public class PrecioIngrediente
    {
        public int IdIngrediente { get; set; }

        public int IdProveedor { get; set; }

        [DisplayName("Ingrediente")]
        public string NombreIngrediente { get; set; }

        [DisplayName("Proveedor")]
        public string NombreProveedor { get; set; }

        [DisplayName("Precio")]
        public decimal Precio { get; set; }

        [DisplayName("Vigencia")]
        public DateTime FechaVigencia { get; set; }
    }
}
