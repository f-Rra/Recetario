using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IIngredienteService
{
    Task<List<IngredienteListaItem>> ListarAsync(string? busqueda);
    Task<IngredienteFormViewModel?> ObtenerAsync(int id);
    Task<List<Unidad>> ListarUnidadesAsync();
    Task<string> GenerarCodigoAsync();
    Task CrearAsync(IngredienteFormViewModel modelo, string usuarioId);

    /// <summary>Edita datos y stock mínimo. El stock actual solo se mueve por movimientos (guía 12).</summary>
    Task<bool> EditarAsync(IngredienteFormViewModel modelo);

    /// <returns>null si se eliminó; mensaje de error si está referenciado o no existe.</returns>
    Task<string?> EliminarAsync(int id);
}
