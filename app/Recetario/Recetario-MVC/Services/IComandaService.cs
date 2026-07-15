using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IComandaService
{
    Task<List<ComandaListaItem>> ListarPorFechaAsync(DateOnly fecha);
    Task<ComandaDetalleViewModel?> ObtenerDetalleAsync(int idComanda);
    Task<PanelCocinaViewModel> ObtenerPanelDelDiaAsync();
    Task<List<Receta>> ListarRecetasActivasAsync();
    Task<List<Persona>> ListarResponsablesAsync();
    Task<List<Ingrediente>> ListarIngredientesAsync();

    /// <summary>
    /// Registra la comanda y consume stock en una transacción.
    /// Reemplaza a sp_RegistrarComanda + trg_ConsumirStockComanda.
    /// </summary>
    /// <returns>(idComanda, null) si se registró; (null, error) si no.</returns>
    Task<(int? IdComanda, string? Error)> RegistrarAsync(int idReceta, int porciones, string usuarioId, int? idPersona);

    /// <summary>
    /// Agrega una modificación ajustando stock con movimientos auditados.
    /// Reemplaza a trg_ActualizarStockModificacion (que no auditaba).
    /// </summary>
    /// <returns>null si se agregó; mensaje de error si no.</returns>
    Task<string?> AgregarModificacionAsync(ModificacionFormViewModel modelo, string usuarioId);
}
