using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class DashboardService : IDashboardService
{
    /// <summary>Margen sobre el stock mínimo a partir del cual un ingrediente se considera "bajo".</summary>
    private const decimal MargenStockBajo = 1.25m;

    private const int MaxFilasStockBajo = 8;

    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardViewModel> ObtenerResumenAsync()
    {
        var recetasActivas = await _context.Recetas.CountAsync(r => r.Activo);
        var totalIngredientes = await _context.Ingredientes.CountAsync();
        var ingredientesCriticos = await _context.Ingredientes
            .CountAsync(i => i.StockActual < i.StockMinimo);

        // Último costeo de cada receta; el promedio se calcula en memoria
        // porque Average sobre secuencia vacía lanza excepción.
        var ultimosCostosUnitarios = await _context.Costos
            .GroupBy(c => c.IdReceta)
            .Select(g => g
                .OrderByDescending(c => c.Fecha)
                .ThenByDescending(c => c.IdCosto)
                .First().CostoUnitario)
            .ToListAsync();

        var stockBajo = await _context.Ingredientes
            .Where(i => i.StockActual < i.StockMinimo * MargenStockBajo)
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

        return new DashboardViewModel
        {
            RecetasActivas = recetasActivas,
            TotalIngredientes = totalIngredientes,
            IngredientesCriticos = ingredientesCriticos,
            CostoPromedioPorcion = ultimosCostosUnitarios.Count > 0
                ? Math.Round(ultimosCostosUnitarios.Average(), 2)
                : 0,
            StockBajo = stockBajo
        };
    }
}
