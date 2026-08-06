using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IPersonaService
{
    Task<List<PersonaListaItem>> ListarAsync();

    /// <summary>Los sectores con quienes los cubren, incluidos los que están vacíos.</summary>
    Task<ResponsablesPorSector> ListarPorSectorAsync();
    Task<PersonaFormViewModel?> ObtenerAsync(int id);
    Task<List<Clasificacion>> ListarSectoresAsync();
    Task CrearAsync(PersonaFormViewModel modelo);
    Task<bool> EditarAsync(PersonaFormViewModel modelo);

    /// <returns>null si se eliminó; mensaje de error si está referenciada o no existe.</returns>
    Task<string?> EliminarAsync(int id);
}
