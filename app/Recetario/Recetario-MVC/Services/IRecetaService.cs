using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IRecetaService
{
    Task<List<RecetaListaItem>> ListarAsync(string? busqueda, int? idClasificacion);
    Task<RecetaDetalleViewModel?> ObtenerDetalleAsync(int id);
    Task<RecetaFormViewModel?> ObtenerFormAsync(int id);
    Task<List<Clasificacion>> ListarClasificacionesAsync();
    Task<List<Ingrediente>> ListarIngredientesAsync();
    Task<string> GenerarCodigoAsync();
    Task<int> CrearAsync(RecetaFormViewModel modelo);
    Task<bool> EditarAsync(RecetaFormViewModel modelo);

    /// <returns>null si se eliminó; mensaje de error si está referenciada o no existe.</returns>
    Task<string?> EliminarAsync(int id);

    /// <returns>null si se agregó; mensaje de error si es duplicado o no existe.</returns>
    Task<string?> AgregarIngredienteAsync(IngredienteRecetaFormViewModel modelo);
    Task<bool> QuitarIngredienteAsync(int idReceta, int idIngrediente);
    Task<bool> AgregarPasoAsync(PasoFormViewModel modelo);
    Task<bool> QuitarPasoAsync(int idProcedimiento, int idReceta);
}
