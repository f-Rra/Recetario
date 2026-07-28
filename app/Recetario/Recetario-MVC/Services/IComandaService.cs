using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IComandaService
{
    // ---------- Comandera ----------

    /// <summary>Catálogo de recetas activas, filtrable por texto y clasificación.</summary>
    Task<List<RecetaCatalogoItem>> ListarCatalogoAsync(string? busqueda, int? idClasificacion);

    /// <summary>Ingredientes de una receta escalados a los comensales, con estado de stock.</summary>
    Task<IngredientesPreviewViewModel?> ObtenerIngredientesPreviewAsync(int idReceta, int comensales);

    /// <summary>Datos mínimos de la receta para sumarla al carrito. Null si no existe o está inactiva.</summary>
    Task<CarritoItem?> ObtenerParaCarritoAsync(int idReceta);

    /// <summary>Ingredientes que la receta tiene hoy (para elegir cuál sustituir o quitar).</summary>
    Task<List<IngredienteEscaladoItem>> ListarIngredientesDeRecetaAsync(int idReceta);

    /// <summary>Responsable resuelto por sector para cada receta del carrito.</summary>
    Task<Dictionary<int, string>> ResolverResponsablesAsync(CarritoComanda carrito);

    /// <summary>
    /// Registra el pedido completo: una comanda por receta, sus modificaciones y
    /// el descuento de stock, y devuelve el PDF. Bloquea si falta stock.
    /// </summary>
    Task<ResultadoGeneracion> GenerarAsync(CarritoComanda carrito, string usuarioId);

    Task<List<Clasificacion>> ListarClasificacionesAsync();
    Task<List<Ingrediente>> ListarIngredientesAsync();

    // ---------- Consulta ----------

    Task<List<ComandaListaItem>> ListarPorFechaAsync(DateOnly fecha);
    Task<ComandaDetalleViewModel?> ObtenerDetalleAsync(int idComanda);
}
