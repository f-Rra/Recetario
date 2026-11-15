using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class PrecioIngredienteService : IPrecioIngredienteService
{
    private readonly ApplicationDbContext _context;
    private readonly TimeProvider _reloj;

    public PrecioIngredienteService(ApplicationDbContext context, TimeProvider reloj)
    {
        _context = context;
        _reloj = reloj;
    }

    public async Task<IngredientePreciosViewModel?> ObtenerHistorialAsync(int idIngrediente)
    {
        var modelo = await _context.Ingredientes
            .Where(i => i.IdIngrediente == idIngrediente)
            .Select(i => new IngredientePreciosViewModel
            {
                IdIngrediente = i.IdIngrediente,
                Codigo = i.Codigo,
                Descripcion = i.Descripcion,
                Unidad = i.Unidad.Abreviatura
            })
            .FirstOrDefaultAsync();

        if (modelo is null)
            return null;

        // Vigente = fecha más reciente que ya haya llegado (desempate por id más alto),
        // misma semántica que el costeo. Los de fecha futura se listan, pero todavía no rigen.
        var hoy = _reloj.Hoy();
        modelo.Historial = await _context.PreciosIngrediente
            .Where(p => p.IdIngrediente == idIngrediente)
            .OrderByDescending(p => p.FechaVigencia)
            .ThenByDescending(p => p.IdPrecio)
            .Select(p => new PrecioListaItem
            {
                IdPrecio = p.IdPrecio,
                Proveedor = p.Proveedor.Nombre,
                Precio = p.Precio,
                FechaVigencia = p.FechaVigencia,
                EsFuturo = p.FechaVigencia > hoy
            })
            .ToListAsync();

        var vigente = modelo.Historial.FirstOrDefault(p => !p.EsFuturo);
        if (vigente is not null)
            vigente.EsVigente = true;

        modelo.NuevoPrecio.IdIngrediente = idIngrediente;
        return modelo;
    }

    public Task<List<Proveedor>> ListarProveedoresAsync() =>
        _context.Proveedores.OrderBy(p => p.Nombre).ToListAsync();

    public async Task<bool> AgregarAsync(PrecioFormViewModel modelo)
    {
        var existeIngrediente = await _context.Ingredientes.AnyAsync(i => i.IdIngrediente == modelo.IdIngrediente);
        var existeProveedor = await _context.Proveedores.AnyAsync(p => p.IdProveedor == modelo.IdProveedor);
        if (!existeIngrediente || !existeProveedor)
            return false;

        _context.PreciosIngrediente.Add(new PrecioIngrediente
        {
            IdIngrediente = modelo.IdIngrediente,
            IdProveedor = modelo.IdProveedor!.Value,
            Precio = modelo.Precio!.Value,
            FechaVigencia = modelo.FechaVigencia
        });
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int idPrecio)
    {
        var precio = await _context.PreciosIngrediente.FindAsync(idPrecio);
        if (precio is null)
            return false;

        _context.PreciosIngrediente.Remove(precio);
        await _context.SaveChangesAsync();
        return true;
    }
}
