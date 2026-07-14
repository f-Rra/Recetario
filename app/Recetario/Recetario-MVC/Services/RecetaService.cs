using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class RecetaService : IRecetaService
{
    private const string PrefijoCodigo = "REC";

    private readonly ApplicationDbContext _context;

    public RecetaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<RecetaListaItem>> ListarAsync(string? busqueda, int? idClasificacion)
    {
        var query = _context.Recetas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(r => r.Codigo.Contains(busqueda) || r.Nombre.Contains(busqueda));

        if (idClasificacion.HasValue)
            query = query.Where(r => r.IdClasificacion == idClasificacion.Value);

        return await query
            .OrderBy(r => r.Nombre)
            .Select(r => new RecetaListaItem
            {
                IdReceta = r.IdReceta,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                Clasificacion = r.Clasificacion.Nombre,
                PorcionesBase = r.PorcionesBase,
                CantidadIngredientes = r.Ingredientes.Count,
                Activo = r.Activo
            })
            .ToListAsync();
    }

    public async Task<RecetaDetalleViewModel?> ObtenerDetalleAsync(int id)
    {
        var receta = await _context.Recetas
            .Where(r => r.IdReceta == id)
            .Select(r => new RecetaDetalleViewModel
            {
                IdReceta = r.IdReceta,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                Clasificacion = r.Clasificacion.Nombre,
                PorcionesBase = r.PorcionesBase,
                Activo = r.Activo
            })
            .FirstOrDefaultAsync();

        if (receta is null)
            return null;

        receta.Ingredientes = await _context.IngredientesReceta
            .Where(ir => ir.IdReceta == id)
            .OrderBy(ir => ir.Ingrediente.Descripcion)
            .Select(ir => new IngredienteRecetaItem
            {
                IdIngrediente = ir.IdIngrediente,
                Ingrediente = ir.Ingrediente.Descripcion,
                CantNeta = ir.CantNeta,
                Rendimiento = ir.Rendimiento,
                CantBruta = ir.CantBruta,
                Unidad = ir.Unidad.Abreviatura
            })
            .ToListAsync();

        receta.Pasos = await _context.Procedimientos
            .Where(p => p.IdReceta == id)
            .OrderBy(p => p.NroPaso)
            .Select(p => new PasoItem
            {
                IdProcedimiento = p.IdProcedimiento,
                NroPaso = p.NroPaso,
                Descripcion = p.Descripcion
            })
            .ToListAsync();

        receta.Datos = (await ObtenerFormAsync(id))!;
        receta.NuevoIngrediente.IdReceta = id;
        receta.NuevoPaso.IdReceta = id;
        return receta;
    }

    public async Task<RecetaFormViewModel?> ObtenerFormAsync(int id)
    {
        return await _context.Recetas
            .Where(r => r.IdReceta == id)
            .Select(r => new RecetaFormViewModel
            {
                IdReceta = r.IdReceta,
                Codigo = r.Codigo,
                Nombre = r.Nombre,
                IdClasificacion = r.IdClasificacion,
                PorcionesBase = r.PorcionesBase,
                Activo = r.Activo
            })
            .FirstOrDefaultAsync();
    }

    public Task<List<Clasificacion>> ListarClasificacionesAsync() =>
        _context.Clasificaciones.OrderBy(c => c.Nombre).ToListAsync();

    public Task<List<Ingrediente>> ListarIngredientesAsync() =>
        _context.Ingredientes.Include(i => i.Unidad).OrderBy(i => i.Descripcion).ToListAsync();

    public async Task<string> GenerarCodigoAsync()
    {
        var codigos = await _context.Recetas
            .Where(r => r.Codigo.StartsWith(PrefijoCodigo))
            .Select(r => r.Codigo)
            .ToListAsync();

        var maximo = codigos
            .Select(c => int.TryParse(c.AsSpan(PrefijoCodigo.Length), out var n) ? n : 0)
            .DefaultIfEmpty(0)
            .Max();

        return $"{PrefijoCodigo}{maximo + 1:000}";
    }

    public async Task<int> CrearAsync(RecetaFormViewModel modelo)
    {
        var receta = new Receta
        {
            Codigo = await GenerarCodigoAsync(),
            Nombre = modelo.Nombre.Trim(),
            IdClasificacion = modelo.IdClasificacion!.Value,
            PorcionesBase = modelo.PorcionesBase!.Value,
            Activo = modelo.Activo
        };

        _context.Recetas.Add(receta);
        await _context.SaveChangesAsync();
        return receta.IdReceta;
    }

    public async Task<bool> EditarAsync(RecetaFormViewModel modelo)
    {
        var receta = await _context.Recetas.FindAsync(modelo.IdReceta);
        if (receta is null)
            return false;

        receta.Nombre = modelo.Nombre.Trim();
        receta.IdClasificacion = modelo.IdClasificacion!.Value;
        receta.PorcionesBase = modelo.PorcionesBase!.Value;
        receta.Activo = modelo.Activo;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> EliminarAsync(int id)
    {
        var receta = await _context.Recetas.FindAsync(id);
        if (receta is null)
            return "La receta no existe.";

        var comandas = await _context.Comandas.CountAsync(c => c.IdReceta == id);
        var costos = await _context.Costos.CountAsync(c => c.IdReceta == id);
        if (comandas > 0 || costos > 0)
            return "No se puede eliminar: tiene comandas o costeos históricos. Podés desactivarla para sacarla de circulación.";

        // Ingredientes y procedimientos se borran en cascada
        _context.Recetas.Remove(receta);
        await _context.SaveChangesAsync();
        return null;
    }

    public async Task<string?> AgregarIngredienteAsync(IngredienteRecetaFormViewModel modelo)
    {
        var receta = await _context.Recetas.FindAsync(modelo.IdReceta);
        var ingrediente = await _context.Ingredientes.FindAsync(modelo.IdIngrediente);
        if (receta is null || ingrediente is null)
            return "La receta o el ingrediente no existen.";

        var duplicado = await _context.IngredientesReceta
            .AnyAsync(ir => ir.IdReceta == modelo.IdReceta && ir.IdIngrediente == modelo.IdIngrediente);
        if (duplicado)
            return $"{ingrediente.Descripcion} ya está en la receta. Quitalo primero si querés cambiar la cantidad.";

        var cantNeta = modelo.CantNeta!.Value;
        var rendimiento = modelo.Rendimiento!.Value;

        _context.IngredientesReceta.Add(new IngredienteReceta
        {
            IdReceta = modelo.IdReceta,
            IdIngrediente = modelo.IdIngrediente!.Value,
            CantNeta = cantNeta,
            Rendimiento = rendimiento,
            // Lo que hay que comprar para obtener la cantidad neta con ese desperdicio
            CantBruta = Math.Round(cantNeta / (rendimiento / 100m), 4),
            IdUnidad = ingrediente.IdUnidad
        });
        await _context.SaveChangesAsync();
        return null;
    }

    public async Task<bool> QuitarIngredienteAsync(int idReceta, int idIngrediente)
    {
        var item = await _context.IngredientesReceta.FindAsync(idReceta, idIngrediente);
        if (item is null)
            return false;

        _context.IngredientesReceta.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> AgregarPasoAsync(PasoFormViewModel modelo)
    {
        if (!await _context.Recetas.AnyAsync(r => r.IdReceta == modelo.IdReceta))
            return false;

        var ultimoPaso = await _context.Procedimientos
            .Where(p => p.IdReceta == modelo.IdReceta)
            .MaxAsync(p => (int?)p.NroPaso) ?? 0;

        _context.Procedimientos.Add(new Procedimiento
        {
            IdReceta = modelo.IdReceta,
            NroPaso = ultimoPaso + 1,
            Descripcion = modelo.Descripcion.Trim()
        });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> QuitarPasoAsync(int idProcedimiento, int idReceta)
    {
        var paso = await _context.Procedimientos.FindAsync(idProcedimiento);
        if (paso is null || paso.IdReceta != idReceta)
            return false;

        _context.Procedimientos.Remove(paso);
        await _context.SaveChangesAsync();

        // Renumerar para no dejar huecos. En dos fases (offset temporal) para no
        // chocar con el índice único receta+paso durante los UPDATEs.
        var restantes = await _context.Procedimientos
            .Where(p => p.IdReceta == idReceta)
            .OrderBy(p => p.NroPaso)
            .ToListAsync();

        foreach (var p in restantes)
            p.NroPaso += 100000;
        await _context.SaveChangesAsync();

        for (var i = 0; i < restantes.Count; i++)
            restantes[i].NroPaso = i + 1;
        await _context.SaveChangesAsync();

        return true;
    }
}
