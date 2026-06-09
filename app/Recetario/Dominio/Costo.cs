using System;
using System.ComponentModel;

namespace Dominio
{
    // Cálculo de costo de una receta en un momento dado.
    // Lo genera sp_CalcularCostoReceta con los precios vigentes.
    public class Costo
    {
        public int IdCosto { get; set; }

        public int IdReceta { get; set; }

        [DisplayName("Receta")]
        public string NombreReceta { get; set; }

        [DisplayName("Fecha")]
        public DateTime Fecha { get; set; }

        [DisplayName("Porciones")]
        public int Porciones { get; set; }

        [DisplayName("Costo Total")]
        public decimal CostoTotal { get; set; }

        [DisplayName("Costo Unitario")]
        public decimal CostoUnitario { get; set; }

        public int IdUsuario { get; set; }
    }
}
