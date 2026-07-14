using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IProveedorService
{
    Task<List<ProveedorListaItem>> ListarAsync(string? busqueda);
    Task<ProveedorFormViewModel?> ObtenerAsync(int id);
    Task CrearAsync(ProveedorFormViewModel modelo);
    Task<bool> EditarAsync(ProveedorFormViewModel modelo);

    /// <returns>null si se eliminó; mensaje de error si está referenciado o no existe.</returns>
    Task<string?> EliminarAsync(int id);
}
