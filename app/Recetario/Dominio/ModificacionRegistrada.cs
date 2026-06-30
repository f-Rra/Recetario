using System;

namespace Dominio
{
    public class ModificacionRegistrada
    {
        public DateTime Fecha { get; set; }
        public string NombreReceta { get; set; }
        public string Tipo { get; set; }
        public string IngredienteOriginal { get; set; }
        public string IngredienteReemplazo { get; set; }
        public decimal Cantidad { get; set; }
        public string Unidad { get; set; }
    }
}
