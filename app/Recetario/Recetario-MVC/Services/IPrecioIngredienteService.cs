using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IPrecioIngredienteService
{
    Task<IngredientePreciosViewModel?> ObtenerHistorialAsync(int idIngrediente);
    Task<List<Proveedor>> ListarProveedoresAsync();
    Task<bool> AgregarAsync(PrecioFormViewModel modelo);
    Task<bool> EliminarAsync(int idPrecio);
}
