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

    [Required(ErrorMessage = "Elegí dónde se guarda.")]
    [Display(Name = "Depósito")]
    public Models.Deposito? Deposito { get; set; }

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
    public Models.Deposito Deposito { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public string Estado { get; set; } = string.Empty;
}

/// <summary>
/// La pantalla entera: la lista de la izquierda y la ficha del elegido.
/// La selección viaja en la URL, así que compartir el enlace abre lo mismo.
/// </summary>
public class IngredientesPaginaViewModel
{
    public string? Busqueda { get; set; }
    public List<IngredienteListaItem> Lista { get; set; } = new();
    public IngredienteDetalleViewModel? Seleccionado { get; set; }

    /// <summary>Para el modal de alta; trae el código que va a tocar.</summary>
    public IngredienteFormViewModel Nuevo { get; set; } = new();

    /// <summary>Se reabre el modal cuando el alta o la edición vuelven con errores.</summary>
    public string? ModalAbierto { get; set; }
}

/// <summary>Todo lo del ingrediente elegido, junto: datos, precios y dónde se usa.</summary>
public class IngredienteDetalleViewModel
{
    public int IdIngrediente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Unidad { get; set; } = string.Empty;
    public Models.Deposito Deposito { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public string Estado { get; set; } = string.Empty;

    /// <summary>Recetas que llevan este ingrediente.</summary>
    public List<string> Recetas { get; set; } = new();

    public List<PrecioListaItem> Precios { get; set; } = new();
    public PrecioFormViewModel NuevoPrecio { get; set; } = new();

    /// <summary>Los mismos datos en formato de formulario, para el modal de edición.</summary>
    public IngredienteFormViewModel Datos { get; set; } = new();
}
