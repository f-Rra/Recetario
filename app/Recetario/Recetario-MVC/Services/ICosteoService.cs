using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface ICosteoService
{
    /// <summary>Arma la página de costeo (datos de la receta + historial). Null si la receta no existe.</summary>
    Task<CostearPaginaViewModel?> ObtenerPaginaAsync(int idReceta);

    /// <summary>
    /// Calcula el costo sin persistir nada. Reemplaza a sp_CalcularCostoReceta:
    /// precio vigente por ingrediente y cantidades escaladas por porciones.
    /// Null si la receta no existe o las porciones son inválidas.
    /// </summary>
    Task<CosteoResultado?> CalcularAsync(int idReceta, int porciones);

    /// <summary>Recalcula y persiste cabecera + desglose. Null si el cálculo no es registrable.</summary>
    Task<int?> RegistrarAsync(int idReceta, int porciones, string usuarioId);
}
