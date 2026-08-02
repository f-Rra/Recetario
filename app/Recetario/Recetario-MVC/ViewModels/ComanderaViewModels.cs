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
        TipoModificacion.Adicion =>
            $"Agregar {NombreReemplazo} ({Helpers.FormatoCantidad.Formatear(Cantidad ?? 0, Unidad ?? "")})",
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

    /// <summary>Lo que anda mal en el pedido tal como está, antes de generarlo.</summary>
    public RevisionComanda Revision { get; set; } = new();
}

public class RecetaCatalogoItem
{
    public int IdReceta { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Clasificacion { get; set; } = string.Empty;
    public int PorcionesBase { get; set; }
    public bool EnCarrito { get; set; }

    public int CantidadIngredientes { get; set; }
    public int CantidadPasos { get; set; }

    /// <summary>
    /// Ingrediente por el que la receta apareció en la búsqueda, cuando no
    /// coincidió por nombre ni por código. Null si coincidió por lo obvio.
    /// </summary>
    public string? IngredienteCoincidente { get; set; }

    public bool Completa => CantidadIngredientes > 0 && CantidadPasos > 0;

    /// <summary>Qué le falta a la receta, para el título del aviso.</summary>
    public string Advertencia => (CantidadIngredientes, CantidadPasos) switch
    {
        (0, 0) => "No tiene ingredientes ni procedimiento cargados",
        (0, _) => "No tiene ingredientes cargados",
        (_, 0) => "No tiene procedimiento cargado",
        _ => string.Empty
    };
}

/// <summary>Ingrediente de la receta seleccionada, escalado a los comensales.</summary>
public class IngredientePreviewItem
{
    public int IdIngrediente { get; set; }
    public string Ingrediente { get; set; } = string.Empty;
    public decimal Cantidad { get; set; }
    public string Unidad { get; set; } = string.Empty;

    /// <summary>Cantidad ya formateada para mostrar (el JS solo la imprime).</summary>
    public string CantidadTexto { get; set; } = string.Empty;

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

// ---------- Revisión previa ----------

/// <summary>Ingrediente que no alcanza para el pedido tal como está armado.</summary>
public class FaltanteStock
{
    public int IdIngrediente { get; set; }
    public string Ingrediente { get; set; } = string.Empty;
    public decimal Necesario { get; set; }
    public decimal Disponible { get; set; }
    public string Unidad { get; set; } = string.Empty;

    public string NecesarioTexto => Helpers.FormatoCantidad.Formatear(Necesario, Unidad);
    public string DisponibleTexto => Helpers.FormatoCantidad.Formatear(Disponible, Unidad);

    public string Descripcion => $"{Ingrediente}: se necesitan {NecesarioTexto} y hay {DisponibleTexto}.";
}

/// <summary>
/// Lo que le pasa al pedido antes de generarlo. Se calcula al pintar la
/// comandera para avisar mientras se arma, y es lo mismo que revisa
/// <c>GenerarAsync</c> al confirmar.
/// </summary>
public class RevisionComanda
{
    /// <summary>Ingredientes cuyo stock no alcanza: mientras haya alguno no se puede generar.</summary>
    public List<FaltanteStock> Faltantes { get; set; } = new();

    /// <summary>Recetas del carrito sin ingredientes o sin procedimiento: avisan, no bloquean.</summary>
    public List<string> Incompletas { get; set; } = new();

    public bool SePuedeGenerar => Faltantes.Count == 0;
}

/// <summary>Resultado de generar el pedido.</summary>
public class ResultadoGeneracion
{
    public List<int> IdsComandas { get; set; } = new();

    /// <summary>Contenido listo para armar el PDF.</summary>
    public List<SeccionComandaPdf> Secciones { get; set; } = new();

    public string? Error { get; set; }

    /// <summary>Ingredientes cuyo stock no alcanza; si hay alguno, no se generó nada.</summary>
    public List<FaltanteStock> Faltantes { get; set; } = new();

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
