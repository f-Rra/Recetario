using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class CosteoService : ICosteoService
{
    private readonly ApplicationDbContext _context;

    public CosteoService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CostearPaginaViewModel?> ObtenerPaginaAsync(int idReceta)
    {
        var pagina = await _context.Recetas
            .Where(r => r.IdReceta == idReceta)
            .Select(r => new CostearPaginaViewModel
            {
                IdReceta = r.IdReceta,
                Codigo = r.Codigo,
                Receta = r.Nombre,
                PorcionesBase = r.PorcionesBase,
                Porciones = r.PorcionesBase
            })
            .FirstOrDefaultAsync();

        if (pagina is null)
            return null;

        pagina.Historial = await _context.Costos
            .Where(c => c.IdReceta == idReceta)
            .OrderByDescending(c => c.Fecha)
            .ThenByDescending(c => c.IdCosto)
            .Select(c => new CosteoHistorialItem
            {
                IdCosto = c.IdCosto,
                Fecha = c.Fecha,
                Porciones = c.Porciones,
                CostoTotal = c.CostoTotal,
                CostoUnitario = c.CostoUnitario,
                Usuario = c.Usuario.Nombre + " " + c.Usuario.Apellido
            })
            .ToListAsync();

        return pagina;
    }

    public async Task<CosteoResultado?> CalcularAsync(int idReceta, int porciones)
    {
        if (porciones <= 0)
            return null;

        var receta = await _context.Recetas.FindAsync(idReceta);
        if (receta is null)
            return null;

        var ingredientes = await _context.IngredientesReceta
            .Where(ir => ir.IdReceta == idReceta)
            .Select(ir => new
            {
                ir.IdIngrediente,
                Descripcion = ir.Ingrediente.Descripcion,
                ir.CantBruta,
                Unidad = ir.Unidad.Abreviatura
            })
            .ToListAsync();

        var resultado = new CosteoResultado
        {
            IdReceta = idReceta,
            Porciones = porciones
        };

        if (ingredientes.Count == 0)
            return resultado; // sin ingredientes: Ok = false, la vista lo explica

        // Precio vigente por ingrediente: fecha más reciente y, ante empate,
        // el último cargado (IdPrecio más alto). Reemplaza al ROW_NUMBER del SP.
        var idsIngredientes = ingredientes.Select(i => i.IdIngrediente).ToList();
        var preciosVigentes = await _context.PreciosIngrediente
            .Where(p => idsIngredientes.Contains(p.IdIngrediente))
            .GroupBy(p => p.IdIngrediente)
            .Select(g => g
                .OrderByDescending(p => p.FechaVigencia)
                .ThenByDescending(p => p.IdPrecio)
                .Select(p => new { p.IdIngrediente, p.Precio, Proveedor = p.Proveedor.Nombre })
                .First())
            .ToListAsync();

        var precioPorIngrediente = preciosVigentes.ToDictionary(p => p.IdIngrediente);

        // Mejora sobre el SP: las cantidades se escalan a las porciones pedidas
        var factor = porciones / (decimal)receta.PorcionesBase;

        foreach (var ing in ingredientes)
        {
            if (!precioPorIngrediente.TryGetValue(ing.IdIngrediente, out var precio))
            {
                resultado.IngredientesSinPrecio.Add(ing.Descripcion);
                continue;
            }

            var cantBrutaEscalada = Math.Round(ing.CantBruta * factor, 4);
            resultado.Detalles.Add(new CosteoDetalleItem
            {
                IdIngrediente = ing.IdIngrediente,
                Ingrediente = ing.Descripcion,
                CantBruta = cantBrutaEscalada,
                Unidad = ing.Unidad,
                PrecioUnitario = precio.Precio,
                Proveedor = precio.Proveedor,
                Subtotal = Math.Round(cantBrutaEscalada * precio.Precio, 4)
            });
        }

        // Nunca se costea incompleto: con faltantes no se calculan totales
        if (resultado.IngredientesSinPrecio.Count > 0)
            return resultado;

        resultado.CostoTotal = Math.Round(resultado.Detalles.Sum(d => d.Subtotal), 4);
        resultado.CostoUnitario = Math.Round(resultado.CostoTotal / porciones, 4);
        return resultado;
    }

    public async Task<int?> RegistrarAsync(int idReceta, int porciones, string usuarioId)
    {
        var resultado = await CalcularAsync(idReceta, porciones);
        if (resultado is null || !resultado.Ok)
            return null;

        var costo = new Costo
        {
            IdReceta = idReceta,
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            Porciones = porciones,
            CostoTotal = resultado.CostoTotal,
            CostoUnitario = resultado.CostoUnitario,
            UsuarioId = usuarioId,
            Detalles = resultado.Detalles.Select(d => new CostoDetalle
            {
                IdIngrediente = d.IdIngrediente,
                CantBruta = d.CantBruta,
                CostoUnitario = d.PrecioUnitario,
                Subtotal = d.Subtotal
            }).ToList()
        };

        _context.Costos.Add(costo);
        await _context.SaveChangesAsync();
        return costo.IdCosto;
    }
}
