using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class CostearPaginaViewModel
{
    public int IdReceta { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Receta { get; set; } = string.Empty;
    public int PorcionesBase { get; set; }

    [Required(ErrorMessage = "Ingresá las porciones.")]
    [Range(1, 99999, ErrorMessage = "Las porciones deben ser al menos {1}.")]
    [Display(Name = "Porciones a costear")]
    public int? Porciones { get; set; }

    /// <summary>Resultado del cálculo (null hasta que se aprieta Calcular).</summary>
    public CosteoResultado? Resultado { get; set; }

    public List<CosteoHistorialItem> Historial { get; set; } = new();
}

public class CosteoResultado
{
    public int IdReceta { get; set; }
    public int Porciones { get; set; }
    public decimal CostoTotal { get; set; }
    public decimal CostoUnitario { get; set; }
    public List<CosteoDetalleItem> Detalles { get; set; } = new();

    /// <summary>Ingredientes de la receta que no tienen ningún precio cargado.</summary>
    public List<string> IngredientesSinPrecio { get; set; } = new();

    public bool Ok => IngredientesSinPrecio.Count == 0 && Detalles.Count > 0;
}

public class CosteoDetalleItem
{
    public int IdIngrediente { get; set; }
    public string Ingrediente { get; set; } = string.Empty;
    public decimal CantBruta { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public decimal PrecioUnitario { get; set; }
    public string Proveedor { get; set; } = string.Empty;
    public decimal Subtotal { get; set; }
}

public class CosteoHistorialItem
{
    public int IdCosto { get; set; }
    public DateOnly Fecha { get; set; }
    public int Porciones { get; set; }
    public decimal CostoTotal { get; set; }
    public decimal CostoUnitario { get; set; }
    public string Usuario { get; set; } = string.Empty;
}
