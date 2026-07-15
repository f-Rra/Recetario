using System.ComponentModel.DataAnnotations;
using RecetarioMVC.Models;

namespace RecetarioMVC.ViewModels;

public class MovimientoFormViewModel
{
    [Required(ErrorMessage = "Elegí un ingrediente.")]
    public int? IdIngrediente { get; set; }

    [Required(ErrorMessage = "Elegí el tipo de movimiento.")]
    [Display(Name = "Tipo")]
    public TipoMovimiento? Tipo { get; set; }

    /// <summary>Para entrada/salida es la cantidad; para ajuste, el stock contado.</summary>
    [Required(ErrorMessage = "Ingresá la cantidad.")]
    [Range(0, 999999, ErrorMessage = "La cantidad no puede ser negativa.")]
    [Display(Name = "Cantidad")]
    public decimal? Cantidad { get; set; }

    [StringLength(255, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Observaciones")]
    public string? Observaciones { get; set; }
}

public class MovimientoHistorialItem
{
    public DateTime Fecha { get; set; }
    public string Ingrediente { get; set; } = string.Empty;
    public TipoMovimiento Tipo { get; set; }
    public decimal Cantidad { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
}

public class HistorialFiltrosViewModel
{
    public int? IdIngrediente { get; set; }
    public TipoMovimiento? Tipo { get; set; }
    public DateOnly? Desde { get; set; }
    public DateOnly? Hasta { get; set; }
    public List<MovimientoHistorialItem> Movimientos { get; set; } = new();
}
