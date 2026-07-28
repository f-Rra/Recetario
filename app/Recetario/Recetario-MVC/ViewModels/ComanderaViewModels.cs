using System.ComponentModel.DataAnnotations;
using RecetarioMVC.Models;

namespace RecetarioMVC.ViewModels;

/// <summary>
/// Pedido que el cocinero arma en pantalla. Vive en la sesión hasta que se
/// genera (equivale al BindingList del ucDashboardCocina).
/// </summary>
public class CarritoComanda
{
    public int Comensales { get; set; }
    public List<CarritoItem> Items { get; set; } = new();

    public bool EstaVacio => Items.Count == 0;
}

public class CarritoItem
{
    public int IdReceta { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Clasificacion { get; set; } = string.Empty;
    public int IdClasificacion { get; set; }
    public int PorcionesBase { get; set; }
    public List<ModificacionCarrito> Modificaciones { get; set; } = new();
}

/// <summary>
/// Modificación pendiente sobre una receta del carrito.
/// <see cref="Cantidad"/> en null significa "la que tenga el ingrediente
/// original en la receta escalada": así el cocinero no ingresa números al
/// sustituir o quitar, y el valor sigue siendo correcto aunque cambie
/// la cantidad de comensales después.
/// </summary>
public class ModificacionCarrito
{
    public TipoModificacion Tipo { get; set; }
    public int? IdIngredienteOriginal { get; set; }
    public string? NombreOriginal { get; set; }
    public int? IdIngredienteReemplazo { get; set; }
    public string? NombreReemplazo { get; set; }
    public decimal? Cantidad { get; set; }
    public string? Unidad { get; set; }

    public string Descripcion => Tipo switch
    {
        TipoModificacion.Sustitucion => $"Sustituir {NombreOriginal} por {NombreReemplazo}",
        TipoModificacion.Adicion => $"Agregar {NombreReemplazo} ({Cantidad:N2} {Unidad})",
        _ => $"Quitar {NombreOriginal}"
    };
}

// ---------- Pantalla ----------

public class ComanderaViewModel
{
    public string? Busqueda { get; set; }
    public int? IdClasificacion { get; set; }
    public List<RecetaCatalogoItem> Catalogo { get; set; } = new();
    public CarritoComanda Carrito { get; set; } = new();

    /// <summary>Responsable resuelto por sector para cada ítem del carrito.</summary>
    public Dictionary<int, string> Responsables { get; set; } = new();

    /// <summary>Ingredientes de cada receta del carrito, para los selects del modal.</summary>
    public Dictionary<int, List<IngredienteEscaladoItem>> IngredientesPorItem { get; set; } = new();
}

public class RecetaCatalogoItem
{
    public int IdReceta { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Clasificacion { get; set; } = string.Empty;
    public int PorcionesBase { get; set; }
    public bool EnCarrito { get; set; }
}

/// <summary>Ingrediente de la receta seleccionada, escalado a los comensales.</summary>
public class IngredientePreviewItem
{
    public int IdIngrediente { get; set; }
    public string Ingrediente { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string Unidad { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public bool Alcanza { get; set; }
}

public class IngredientesPreviewViewModel
{
    public string Receta { get; set; } = string.Empty;
    public int Comensales { get; set; }
    public List<IngredientePreviewItem> Ingredientes { get; set; } = new();
}

// ---------- Modificación ----------

public class ModificacionComanderaForm
{
    public int IdReceta { get; set; }

    [Required(ErrorMessage = "Elegí el tipo de modificación.")]
    public TipoModificacion? Tipo { get; set; }

    /// <summary>Ingrediente de la receta a sustituir o quitar.</summary>
    public int? IdIngredienteOriginal { get; set; }

    /// <summary>Ingrediente que reemplaza o se agrega.</summary>
    public int? IdIngredienteReemplazo { get; set; }

    /// <summary>Solo para adición (o sustitución entre unidades distintas).</summary>
    public decimal? Cantidad { get; set; }
}

/// <summary>Resultado de generar el pedido.</summary>
public class ResultadoGeneracion
{
    public List<int> IdsComandas { get; set; } = new();

    /// <summary>Contenido listo para armar el PDF.</summary>
    public List<SeccionComandaPdf> Secciones { get; set; } = new();

    public string? Error { get; set; }

    /// <summary>Ingredientes cuyo stock no alcanza; si hay alguno, no se generó nada.</summary>
    public List<string> Faltantes { get; set; } = new();

    public bool Ok => Error is null && Faltantes.Count == 0;
}

// ---------- PDF ----------

public class ComandaPdfViewModel
{
    public DateOnly Fecha { get; set; }
    public int Comensales { get; set; }
    public string Usuario { get; set; } = string.Empty;
    public List<SeccionComandaPdf> Secciones { get; set; } = new();
}

public class SeccionComandaPdf
{
    public string Receta { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Responsable { get; set; } = string.Empty;

    /// <summary>Ingredientes con las modificaciones ya aplicadas.</summary>
    public List<IngredienteEscaladoItem> Ingredientes { get; set; } = new();

    /// <summary>Detalle de qué se cambió respecto de la receta original.</summary>
    public List<string> Modificaciones { get; set; } = new();

    public List<PasoItem> Pasos { get; set; } = new();
}
