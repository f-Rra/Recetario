using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class IngredienteService : IIngredienteService
{
    private const string PrefijoCodigo = "ING";

    private readonly ApplicationDbContext _context;

    public IngredienteService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<IngredienteListaItem>> ListarAsync(string? busqueda)
    {
        var query = _context.Ingredientes.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(i => i.Codigo.Contains(busqueda) || i.Descripcion.Contains(busqueda));

        var ingredientes = await query
            .OrderBy(i => i.Descripcion)
            .Select(i => new
            {
                i.IdIngrediente,
                i.Codigo,
                i.Descripcion,
                Unidad = i.Unidad.Abreviatura,
                i.StockActual,
                i.StockMinimo
            })
            .ToListAsync();

        return ingredientes.Select(i => new IngredienteListaItem
        {
            IdIngrediente = i.IdIngrediente,
            Codigo = i.Codigo,
            Descripcion = i.Descripcion,
            Unidad = i.Unidad,
            StockActual = i.StockActual,
            StockMinimo = i.StockMinimo,
            Estado = StockEstado.De(i.StockActual, i.StockMinimo)
        }).ToList();
    }

    public async Task<IngredienteFormViewModel?> ObtenerAsync(int id)
    {
        return await _context.Ingredientes
            .Where(i => i.IdIngrediente == id)
            .Select(i => new IngredienteFormViewModel
            {
                IdIngrediente = i.IdIngrediente,
                Codigo = i.Codigo,
                Descripcion = i.Descripcion,
                IdUnidad = i.IdUnidad,
                StockActual = i.StockActual,
                StockMinimo = i.StockMinimo
            })
            .FirstOrDefaultAsync();
    }

    public Task<List<Unidad>> ListarUnidadesAsync() =>
        _context.Unidades.OrderBy(u => u.Nombre).ToListAsync();

    public async Task<string> GenerarCodigoAsync()
    {
        var codigos = await _context.Ingredientes
            .Where(i => i.Codigo.StartsWith(PrefijoCodigo))
            .Select(i => i.Codigo)
            .ToListAsync();

        var maximo = codigos
            .Select(c => int.TryParse(c.AsSpan(PrefijoCodigo.Length), out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();

        return $"{PrefijoCodigo}{maximo + 1:000}";
    }

    public async Task CrearAsync(IngredienteFormViewModel modelo, string usuarioId)
    {
        // El código se regenera en el POST: el del form pudo quedar viejo
        // si otro admin creó un ingrediente en el medio.
        var ingrediente = new Ingrediente
        {
            Codigo = await GenerarCodigoAsync(),
            Descripcion = modelo.Descripcion.Trim(),
            IdUnidad = modelo.IdUnidad!.Value,
            StockActual = modelo.StockActual,
            StockMinimo = modelo.StockMinimo
        };

        _context.Ingredientes.Add(ingrediente);
        await _context.SaveChangesAsync();

        // El stock inicial también queda auditado (guía 12)
        if (modelo.StockActual > 0)
        {
            _context.MovimientosStock.Add(new MovimientoStock
            {
                IdIngrediente = ingrediente.IdIngrediente,
                Tipo = TipoMovimiento.Entrada,
                Cantidad = modelo.StockActual,
                IdUnidad = ingrediente.IdUnidad,
                Fecha = DateTime.Now,
                UsuarioId = usuarioId,
                Observaciones = "Stock inicial"
            });
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> EditarAsync(IngredienteFormViewModel modelo)
    {
        var ingrediente = await _context.Ingredientes.FindAsync(modelo.IdIngrediente);
        if (ingrediente is null)
            return false;

        ingrediente.Descripcion = modelo.Descripcion.Trim();
        ingrediente.IdUnidad = modelo.IdUnidad!.Value;
        // StockActual no se toca: solo se mueve por movimientos auditados (guía 12)
        ingrediente.StockMinimo = modelo.StockMinimo;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> EliminarAsync(int id)
    {
        var ingrediente = await _context.Ingredientes.FindAsync(id);
        if (ingrediente is null)
            return "El ingrediente no existe.";

        var recetas = await _context.IngredientesReceta.CountAsync(x => x.IdIngrediente == id);
        if (recetas > 0)
            return $"No se puede eliminar: está usado en {recetas} receta(s).";

        var tieneHistorial =
            await _context.MovimientosStock.AnyAsync(m => m.IdIngrediente == id) ||
            await _context.CostosDetalle.AnyAsync(d => d.IdIngrediente == id) ||
            await _context.Modificaciones.AnyAsync(m =>
                m.IdIngredienteOriginal == id || m.IdIngredienteReemplazo == id);

        if (tieneHistorial)
            return "No se puede eliminar: tiene movimientos o historial asociado.";

        // Sus precios (PreciosIngrediente) se borran en cascada
        _context.Ingredientes.Remove(ingrediente);
        await _context.SaveChangesAsync();
        return null;
    }
}
