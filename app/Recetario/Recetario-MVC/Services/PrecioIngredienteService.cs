using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class PrecioIngredienteService : IPrecioIngredienteService
{
    private readonly ApplicationDbContext _context;

    public PrecioIngredienteService(ApplicationDbContext context)
    {
        _context = context;
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

        // Vigente = fecha más reciente (desempate por id más alto),
        // misma semántica que el ROW_NUMBER de sp_CalcularCostoReceta.
        modelo.Historial = await _context.PreciosIngrediente
            .Where(p => p.IdIngrediente == idIngrediente)
            .OrderByDescending(p => p.FechaVigencia)
            .ThenByDescending(p => p.IdPrecio)
            .Select(p => new PrecioListaItem
            {
                IdPrecio = p.IdPrecio,
                Proveedor = p.Proveedor.Nombre,
                Precio = p.Precio,
                FechaVigencia = p.FechaVigencia
            })
            .ToListAsync();

        if (modelo.Historial.Count > 0)
            modelo.Historial[0].EsVigente = true;

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
