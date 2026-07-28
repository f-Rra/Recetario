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

    /// <summary>Ingredientes de la receta escalados a las porciones de la comanda.</summary>
    public List<IngredienteEscaladoItem> Ingredientes { get; set; } = new();

    public List<PasoItem> Pasos { get; set; } = new();
    public List<ModificacionItem> Modificaciones { get; set; } = new();
}

public class IngredienteEscaladoItem
{
    public int IdIngrediente { get; set; }
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

    public string Descripcion => Tipo switch
    {
        TipoModificacion.Sustitucion =>
            $"Sustituir {IngredienteOriginal} por {IngredienteReemplazo} ({Cantidad:N2} {Unidad})",
        TipoModificacion.Adicion =>
            $"Agregar {IngredienteReemplazo} ({Cantidad:N2} {Unidad})",
        _ =>
            $"Quitar {IngredienteOriginal} ({Cantidad:N2} {Unidad})"
    };
}
