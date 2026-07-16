using System.ComponentModel.DataAnnotations;
using RecetarioMVC.Models;

namespace RecetarioMVC.ViewModels;

public class ComandaListaItem
{
    public int IdComanda { get; set; }
    public DateOnly Fecha { get; set; }
    public string Receta { get; set; } = string.Empty;
    public string Clasificacion { get; set; } = string.Empty;
    public int Porciones { get; set; }
    public string Responsable { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public int CantidadModificaciones { get; set; }
}

public class RegistrarComandaViewModel
{
    [Required(ErrorMessage = "Elegí una receta.")]
    [Display(Name = "Receta")]
    public int? IdReceta { get; set; }

    [Required(ErrorMessage = "Ingresá las porciones.")]
    [Range(1, 99999, ErrorMessage = "Las porciones deben ser al menos {1}.")]
    [Display(Name = "Porciones")]
    public int? Porciones { get; set; }

    [Display(Name = "Responsable")]
    public int? IdPersona { get; set; } // opcional: se sugiere por sector
}

public class ComandaDetalleViewModel
{
    public int IdComanda { get; set; }
    public DateOnly Fecha { get; set; }
    public int IdReceta { get; set; }
    public string Receta { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Clasificacion { get; set; } = string.Empty;
    public int Porciones { get; set; }
    public int PorcionesBase { get; set; }
    public string Responsable { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;

    /// <summary>Ingredientes de la receta escalados a las porciones de la comanda (ex vw_Comanda).</summary>
    public List<IngredienteEscaladoItem> Ingredientes { get; set; } = new();

    public List<PasoItem> Pasos { get; set; } = new();
    public List<ModificacionItem> Modificaciones { get; set; } = new();
    public ModificacionFormViewModel NuevaModificacion { get; set; } = new();
}

public class IngredienteEscaladoItem
{
    public string Ingrediente { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string Unidad { get; set; } = string.Empty;
}

public class ModificacionItem
{
    public TipoModificacion Tipo { get; set; }
    public string? IngredienteOriginal { get; set; }
    public string? IngredienteReemplazo { get; set; }
    public decimal Cantidad { get; set; }
    public string Unidad { get; set; } = string.Empty;
}

public class ModificacionFormViewModel
{
    public int IdComanda { get; set; }

    [Required(ErrorMessage = "Elegí el tipo de modificación.")]
    [Display(Name = "Tipo")]
    public TipoModificacion? Tipo { get; set; }

    [Display(Name = "Ingrediente original")]
    public int? IdIngredienteOriginal { get; set; }

    [Display(Name = "Ingrediente reemplazo/agregado")]
    public int? IdIngredienteReemplazo { get; set; }

    [Required(ErrorMessage = "Ingresá la cantidad.")]
    [Range(0.0001, 999999, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    [Display(Name = "Cantidad")]
    public decimal? Cantidad { get; set; }
}

public class PanelCocinaViewModel
{
    public int ComandasHoy { get; set; }
    public int PorcionesHoy { get; set; }
    public List<ComandaListaItem> Comandas { get; set; } = new();
}
