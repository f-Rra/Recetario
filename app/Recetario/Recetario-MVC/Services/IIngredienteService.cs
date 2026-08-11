using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IIngredienteService
{
    /// <summary>Todos los que coinciden: el stock y los combos los necesitan completos.</summary>
    Task<List<IngredienteListaItem>> ListarAsync(string? busqueda);

    /// <summary>De a una página, para el listado de ingredientes.</summary>
    Task<Paginado<IngredienteListaItem>> ListarPaginadoAsync(string? busqueda, int pagina);
    Task<IngredienteFormViewModel?> ObtenerAsync(int id);

    /// <summary>Ficha del ingrediente sin los precios: esos los trae su propio servicio.</summary>
    Task<IngredienteDetalleViewModel?> ObtenerDetalleAsync(int id);
    Task<List<Unidad>> ListarUnidadesAsync();
    Task<string> GenerarCodigoAsync();
    Task CrearAsync(IngredienteFormViewModel modelo, string usuarioId);

    /// <summary>Edita datos y stock mínimo. El stock actual solo se mueve por movimientos (guía 12).</summary>
    Task<bool> EditarAsync(IngredienteFormViewModel modelo);

    /// <returns>null si se eliminó; mensaje de error si está referenciado o no existe.</returns>
    Task<string?> EliminarAsync(int id);
}
