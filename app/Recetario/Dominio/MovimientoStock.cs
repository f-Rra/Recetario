using System;
using System.ComponentModel;

namespace Dominio
{
    // Entrada, salida o ajuste de stock de un ingrediente.
    // El trigger trg_ActualizarStockMovimiento actualiza el StockActual al insertarse.
    public class MovimientoStock
    {
        public int IdMovimiento { get; set; }

        public int IdIngrediente { get; set; }

        [DisplayName("Ingrediente")]
        public string NombreIngrediente { get; set; }

        public int IdTipoMovimiento { get; set; }

        // Dato denormalizado que trae el Mapper (nombre del tipo de movimiento).
        [DisplayName("Tipo")]
        public string TipoMovimiento { get; set; }

        [DisplayName("Cantidad")]
        public decimal Cantidad { get; set; }

        public int IdUnidad { get; set; }

        [DisplayName("Unidad")]
        public string Abreviatura { get; set; }

        [DisplayName("Fecha")]
        public DateTime Fecha { get; set; }

        public int IdUsuario { get; set; }

        [DisplayName("Observaciones")]
        public string Observaciones { get; set; }
    }
}
