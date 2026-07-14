using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class ProveedorService : IProveedorService
{
    private readonly ApplicationDbContext _context;

    public ProveedorService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ProveedorListaItem>> ListarAsync(string? busqueda)
    {
        var query = _context.Proveedores.AsQueryable();

        if (!string.IsNullOrWhiteSpace(busqueda))
            query = query.Where(p => p.Nombre.Contains(busqueda) ||
                                     (p.Contacto != null && p.Contacto.Contains(busqueda)));

        return await query
            .OrderBy(p => p.Nombre)
            .Select(p => new ProveedorListaItem
            {
                IdProveedor = p.IdProveedor,
                Nombre = p.Nombre,
                Contacto = p.Contacto,
                Telefono = p.Telefono,
                Email = p.Email,
                CantidadPrecios = p.Precios.Count
            })
            .ToListAsync();
    }

    public async Task<ProveedorFormViewModel?> ObtenerAsync(int id)
    {
        return await _context.Proveedores
            .Where(p => p.IdProveedor == id)
            .Select(p => new ProveedorFormViewModel
            {
                IdProveedor = p.IdProveedor,
                Nombre = p.Nombre,
                Contacto = p.Contacto,
                Telefono = p.Telefono,
                Email = p.Email,
                Direccion = p.Direccion
            })
            .FirstOrDefaultAsync();
    }

    public async Task CrearAsync(ProveedorFormViewModel modelo)
    {
        _context.Proveedores.Add(new Proveedor
        {
            Nombre = modelo.Nombre.Trim(),
            Contacto = Limpiar(modelo.Contacto),
            Telefono = Limpiar(modelo.Telefono),
            Email = Limpiar(modelo.Email),
            Direccion = Limpiar(modelo.Direccion)
        });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EditarAsync(ProveedorFormViewModel modelo)
    {
        var proveedor = await _context.Proveedores.FindAsync(modelo.IdProveedor);
        if (proveedor is null)
            return false;

        proveedor.Nombre = modelo.Nombre.Trim();
        proveedor.Contacto = Limpiar(modelo.Contacto);
        proveedor.Telefono = Limpiar(modelo.Telefono);
        proveedor.Email = Limpiar(modelo.Email);
        proveedor.Direccion = Limpiar(modelo.Direccion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> EliminarAsync(int id)
    {
        var proveedor = await _context.Proveedores.FindAsync(id);
        if (proveedor is null)
            return "El proveedor no existe.";

        var precios = await _context.PreciosIngrediente.CountAsync(p => p.IdProveedor == id);
        if (precios > 0)
            return $"No se puede eliminar: tiene {precios} precio(s) registrado(s), que son la base del costeo.";

        _context.Proveedores.Remove(proveedor);
        await _context.SaveChangesAsync();
        return null;
    }

    private static string? Limpiar(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
