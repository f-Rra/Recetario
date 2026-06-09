using System;
using System.ComponentModel;

namespace Dominio
{
    // Precio de un ingrediente para un proveedor, con fecha de vigencia.
    // sp_CalcularCostoReceta usa el de mayor FechaVigencia (precio vigente).
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
