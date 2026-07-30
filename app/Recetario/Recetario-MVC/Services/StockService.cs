using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class StockService : IStockService
{
    private const int MaxFilasHistorial = 100;

    private readonly ApplicationDbContext _context;

    public StockService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string?> RegistrarMovimientoAsync(MovimientoFormViewModel modelo, string usuarioId)
    {
        var cantidad = Math.Round(modelo.Cantidad!.Value, 4);
        var tipo = modelo.Tipo!.Value;

        // Para entrada/salida la cantidad debe ser positiva; el ajuste admite 0
        // ("conté y no queda nada")
        if (cantidad < 0 || (cantidad == 0 && tipo != TipoMovimiento.Ajuste))
            return "La cantidad debe ser mayor a cero.";

        var ingrediente = await _context.Ingredientes.FindAsync(modelo.IdIngrediente);
        if (ingrediente is null)
            return "El ingrediente no existe.";

        var estrategia = _context.Database.CreateExecutionStrategy();
        await estrategia.ExecuteAsync(async () =>
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();

            _context.MovimientosStock.Add(new MovimientoStock
            {
                IdIngrediente = ingrediente.IdIngrediente,
                Tipo = tipo,
                Cantidad = cantidad,
                IdUnidad = ingrediente.IdUnidad,
                Fecha = DateTime.Now,
                UsuarioId = usuarioId,
                Observaciones = string.IsNullOrWhiteSpace(modelo.Observaciones)
                    ? null
                    : modelo.Observaciones.Trim()
            });

            // Misma semántica que el trigger viejo
            ingrediente.StockActual = tipo switch
            {
                TipoMovimiento.Entrada => ingrediente.StockActual + cantidad,
                TipoMovimiento.Salida => ingrediente.StockActual - cantidad,
                TipoMovimiento.Ajuste => cantidad,
                _ => ingrediente.StockActual
            };

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();
        });

        return null;
    }

    public async Task<InventarioViewModel> ObtenerInventarioAsync(Deposito deposito)
    {
        return new InventarioViewModel
        {
            Deposito = deposito,
            Ingredientes = await _context.Ingredientes
                .Where(i => i.Deposito == deposito)
                .OrderBy(i => i.Descripcion)
                .Select(i => new InventarioItem
                {
                    IdIngrediente = i.IdIngrediente,
                    Codigo = i.Codigo,
                    Ingrediente = i.Descripcion,
                    Unidad = i.Unidad.Abreviatura,
                    StockActual = i.StockActual
                })
                .ToListAsync()
        };
    }

    public async Task<int> GuardarInventarioAsync(
        Deposito deposito, List<InventarioItem> conteo, string usuarioId)
    {
        var contados = conteo
            .Where(c => c.Contado.HasValue && c.Contado.Value >= 0)
            .ToDictionary(c => c.IdIngrediente, c => Math.Round(c.Contado!.Value, 4));

        if (contados.Count == 0)
            return 0;

        var ids = contados.Keys.ToList();
        var ingredientes = await _context.Ingredientes
            .Where(i => i.Deposito == deposito && ids.Contains(i.IdIngrediente))
            .ToListAsync();

        var ajustados = 0;
        var estrategia = _context.Database.CreateExecutionStrategy();

        await estrategia.ExecuteAsync(async () =>
        {
            await using var transaccion = await _context.Database.BeginTransactionAsync();
            ajustados = 0;

            foreach (var ingrediente in ingredientes)
            {
                var contado = contados[ingrediente.IdIngrediente];

                // Lo que ya coincide con el sistema no genera movimiento
                if (contado == ingrediente.StockActual)
                    continue;

                _context.MovimientosStock.Add(new MovimientoStock
                {
                    IdIngrediente = ingrediente.IdIngrediente,
                    Tipo = TipoMovimiento.Ajuste,
                    Cantidad = contado,
                    IdUnidad = ingrediente.IdUnidad,
                    Fecha = DateTime.Now,
                    UsuarioId = usuarioId,
                    Observaciones = $"Inventario de {NombreDeposito.De(deposito).ToLowerInvariant()}"
                });

                ingrediente.StockActual = contado;
                ajustados++;
            }

            await _context.SaveChangesAsync();
            await transaccion.CommitAsync();
        });

        return ajustados;
    }

    public async Task<List<MovimientoHistorialItem>> HistorialAsync(
        int? idIngrediente, TipoMovimiento? tipo, DateOnly? desde, DateOnly? hasta)
    {
        var query = _context.MovimientosStock.AsQueryable();

        if (idIngrediente.HasValue)
            query = query.Where(m => m.IdIngrediente == idIngrediente.Value);
        if (tipo.HasValue)
            query = query.Where(m => m.Tipo == tipo.Value);
        if (desde.HasValue)
            query = query.Where(m => m.Fecha >= desde.Value.ToDateTime(TimeOnly.MinValue));
        if (hasta.HasValue)
            query = query.Where(m => m.Fecha < hasta.Value.AddDays(1).ToDateTime(TimeOnly.MinValue));

        return await query
            .OrderByDescending(m => m.Fecha)
            .ThenByDescending(m => m.IdMovimiento)
            .Take(MaxFilasHistorial)
            .Select(m => new MovimientoHistorialItem
            {
                Fecha = m.Fecha,
                Ingrediente = m.Ingrediente.Descripcion,
                Tipo = m.Tipo,
                Cantidad = m.Cantidad,
                Unidad = m.Unidad.Abreviatura,
                Usuario = m.Usuario.Nombre + " " + m.Usuario.Apellido,
                Observaciones = m.Observaciones
            })
            .ToListAsync();
    }
}
