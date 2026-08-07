using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class DashboardService : IDashboardService
{
    private const int MaxFilasStockBajo = 6;
    private const int MaxComandasRecientes = 5;
    private const int MaxActividad = 12;

    /// <summary>Los movimientos de una comanda se resumen en una sola línea.</summary>
    private const string PrefijoConsumo = "Consumo comanda";

    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardViewModel> ObtenerResumenAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var inicioMes = new DateOnly(hoy.Year, hoy.Month, 1);
        var inicioMesAnterior = inicioMes.AddMonths(-1);

        var (valor, valorizados) = await ValorizarInventarioAsync();

        return new DashboardViewModel
        {
            RecetasActivas = await _context.Recetas.CountAsync(r => r.Activo),
            RecetasIncompletas = await _context.Recetas
                .CountAsync(r => r.Activo && (!r.Ingredientes.Any() || !r.Procedimientos.Any())),

            TotalIngredientes = await _context.Ingredientes.CountAsync(),
            IngredientesBodega = await _context.Ingredientes.CountAsync(i => i.Deposito == Deposito.Bodega),
            IngredientesCamara = await _context.Ingredientes.CountAsync(i => i.Deposito == Deposito.Camara),

            PorcionesDelMes = await SumarPorcionesAsync(inicioMes, hoy),
            PorcionesMesAnterior = await SumarPorcionesAsync(inicioMesAnterior, inicioMes.AddDays(-1)),

            ValorInventario = valor,
            IngredientesValorizados = valorizados,

            StockBajo = await ObtenerStockBajoAsync(),
            UltimasComandas = await ObtenerUltimasComandasAsync(),
            Actividad = await ObtenerActividadAsync()
        };
    }

    /// <summary>El cast a int? evita que Sum reviente cuando no hay comandas.</summary>
    private async Task<int> SumarPorcionesAsync(DateOnly desde, DateOnly hasta) =>
        await _context.Comandas
            .Where(c => c.Fecha >= desde && c.Fecha <= hasta)
            .SumAsync(c => (int?)c.Porciones) ?? 0;

    /// <summary>
    /// Stock actual × precio vigente. Los ingredientes sin ningún precio no se
    /// pueden valorizar, así que se cuentan aparte para no mentir el total.
    /// </summary>
    private async Task<(decimal Valor, int Valorizados)> ValorizarInventarioAsync()
    {
        var stocks = await _context.Ingredientes
            .Select(i => new { i.IdIngrediente, i.StockActual })
            .ToListAsync();

        // Vigente = fecha más reciente, desempate por id: la misma semántica
        // que usa el costeo (guía 10)
        var precios = await _context.PreciosIngrediente
            .GroupBy(p => p.IdIngrediente)
            .Select(g => new
            {
                IdIngrediente = g.Key,
                Precio = g.OrderByDescending(p => p.FechaVigencia)
                          .ThenByDescending(p => p.IdPrecio)
                          .First().Precio
            })
            .ToDictionaryAsync(x => x.IdIngrediente, x => x.Precio);

        var valor = 0m;
        var valorizados = 0;

        foreach (var stock in stocks)
        {
            if (!precios.TryGetValue(stock.IdIngrediente, out var precio))
                continue;

            valor += stock.StockActual * precio;
            valorizados++;
        }

        return (Math.Round(valor, 2), valorizados);
    }

    private Task<List<IngredienteStockItem>> ObtenerStockBajoAsync() =>
        _context.Ingredientes
            .Where(i => i.StockActual < i.StockMinimo * StockEstado.MargenBajo)
            .OrderBy(i => i.StockActual - i.StockMinimo)
            .Take(MaxFilasStockBajo)
            .Select(i => new IngredienteStockItem
            {
                Codigo = i.Codigo,
                Descripcion = i.Descripcion,
                StockActual = i.StockActual,
                StockMinimo = i.StockMinimo,
                Unidad = i.Unidad.Abreviatura
            })
            .ToListAsync();

    private Task<List<ComandaRecienteItem>> ObtenerUltimasComandasAsync() =>
        _context.Comandas
            .OrderByDescending(c => c.Fecha).ThenByDescending(c => c.IdComanda)
            .Take(MaxComandasRecientes)
            .Select(c => new ComandaRecienteItem
            {
                IdComanda = c.IdComanda,
                Fecha = c.Fecha,
                Receta = c.Receta.Nombre,
                Porciones = c.Porciones
            })
            .ToListAsync();

    /// <summary>
    /// Comandas generadas y movimientos de stock, mezclados por fecha. Los
    /// movimientos de consumo se omiten: ya están representados por su comanda,
    /// y una sola de nueve ingredientes taparía todo lo demás.
    /// </summary>
    private async Task<List<ActividadItem>> ObtenerActividadAsync()
    {
        var comandas = await _context.Comandas
            .OrderByDescending(c => c.Fecha).ThenByDescending(c => c.IdComanda)
            .Take(MaxActividad)
            .Select(c => new ActividadItem
            {
                Fecha = c.Fecha,
                Titulo = $"Comanda generada · {c.Receta.Nombre}",
                Detalle = c.Porciones + (c.Porciones == 1 ? " porción · " : " porciones · ") +
                          c.Usuario.Nombre + " " + c.Usuario.Apellido,
                Tipo = "comanda"
            })
            .ToListAsync();

        var movimientos = await _context.MovimientosStock
            .Where(m => m.Observaciones == null || !m.Observaciones.StartsWith(PrefijoConsumo))
            .OrderByDescending(m => m.Fecha)
            .Take(MaxActividad)
            .Select(m => new
            {
                m.Fecha,
                m.Tipo,
                m.Cantidad,
                m.Observaciones,
                Ingrediente = m.Ingrediente.Descripcion,
                Unidad = m.Unidad.Abreviatura,
                Usuario = m.Usuario.Nombre + " " + m.Usuario.Apellido
            })
            .ToListAsync();

        var actividad = movimientos.Select(m => new ActividadItem
        {
            Fecha = DateOnly.FromDateTime(m.Fecha),
            Hora = TimeOnly.FromDateTime(m.Fecha),
            Titulo = m.Tipo switch
            {
                TipoMovimiento.Entrada => $"Entrada de stock · {m.Ingrediente}",
                TipoMovimiento.Salida => $"Salida de stock · {m.Ingrediente}",
                _ => $"Ajuste de inventario · {m.Ingrediente}"
            },
            Detalle = FormatoCantidad.Formatear(m.Cantidad, m.Unidad) +
                      (string.IsNullOrWhiteSpace(m.Observaciones) ? "" : " · " + m.Observaciones) +
                      " · " + m.Usuario,
            Tipo = m.Tipo.ToString().ToLowerInvariant()
        }).ToList();

        actividad.AddRange(comandas);

        // Las comandas solo guardan el día: dentro de una misma fecha van
        // primero, y después los movimientos por hora
        return actividad
            .OrderByDescending(a => a.Fecha)
            .ThenBy(a => a.Hora is null ? 0 : 1)
            .ThenByDescending(a => a.Hora)
            .Take(MaxActividad)
            .ToList();
    }
}
