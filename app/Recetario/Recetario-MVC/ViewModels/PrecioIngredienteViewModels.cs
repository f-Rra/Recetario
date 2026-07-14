using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class IngredientePreciosViewModel
{
    public int IdIngrediente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public List<PrecioListaItem> Historial { get; set; } = new();
    public PrecioFormViewModel NuevoPrecio { get; set; } = new();
}

public class PrecioListaItem
{
    public int IdPrecio { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public decimal Precio { get; set; }
    public DateOnly FechaVigencia { get; set; }
    public bool EsVigente { get; set; }
}

public class PrecioFormViewModel
{
    public int IdIngrediente { get; set; }

    [Required(ErrorMessage = "Elegí un proveedor.")]
    [Display(Name = "Proveedor")]
    public int? IdProveedor { get; set; }

    [Required(ErrorMessage = "Ingresá el precio.")]
    [Range(0.0001, 99999999, ErrorMessage = "El precio debe ser mayor a cero.")]
    [Display(Name = "Precio")]
    public decimal? Precio { get; set; }

    [Required(ErrorMessage = "Ingresá la fecha de vigencia.")]
    [Display(Name = "Fecha de vigencia")]
    public DateOnly FechaVigencia { get; set; } = DateOnly.FromDateTime(DateTime.Today);
}
