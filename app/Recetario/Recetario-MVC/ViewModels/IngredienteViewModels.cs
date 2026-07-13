using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class IngredienteFormViewModel
{
    public int IdIngrediente { get; set; }

    [Display(Name = "Código")]
    public string Codigo { get; set; } = string.Empty; // autogenerado, solo lectura

    [Required(ErrorMessage = "Ingresá la descripción.")]
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Descripción")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "Elegí una unidad.")]
    [Display(Name = "Unidad")]
    public int? IdUnidad { get; set; }

    [Range(0, 999999, ErrorMessage = "El stock no puede ser negativo.")]
    [Display(Name = "Stock actual")]
    public decimal StockActual { get; set; }

    [Range(0, 999999, ErrorMessage = "El stock mínimo no puede ser negativo.")]
    [Display(Name = "Stock mínimo")]
    public decimal StockMinimo { get; set; }
}

public class IngredienteListaItem
{
    public int IdIngrediente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public string Estado { get; set; } = string.Empty;
}
