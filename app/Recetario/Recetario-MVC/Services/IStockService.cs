using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public interface IStockService
{
    /// <summary>
    /// Registra un movimiento manual y actualiza el stock en una transacción.
    /// Entrada suma, salida resta (puede quedar negativo), ajuste setea el valor.
    /// Reemplaza a trg_ActualizarStockMovimiento.
    /// </summary>
    /// <returns>null si se registró; mensaje de error si no.</returns>
    Task<string?> RegistrarMovimientoAsync(MovimientoFormViewModel modelo, string usuarioId);

    /// <summary>Historial con filtros, más reciente primero (últimos 100).</summary>
    Task<List<MovimientoHistorialItem>> HistorialAsync(
        int? idIngrediente, Models.TipoMovimiento? tipo, DateOnly? desde, DateOnly? hasta);

    /// <summary>Ingredientes de un depósito para hacer el conteo físico.</summary>
    Task<InventarioViewModel> ObtenerInventarioAsync(Models.Deposito deposito);

    /// <summary>
    /// Aplica el conteo: cada cantidad distinta a la del sistema genera un ajuste.
    /// Las que vienen en blanco no se tocan.
    /// </summary>
    /// <returns>Cuántos ingredientes se ajustaron.</returns>
    Task<int> GuardarInventarioAsync(
        Models.Deposito deposito, List<InventarioItem> conteo, string usuarioId);
}
