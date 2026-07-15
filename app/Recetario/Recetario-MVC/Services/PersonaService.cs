using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

public class PersonaService : IPersonaService
{
    private readonly ApplicationDbContext _context;

    public PersonaService(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<PersonaListaItem>> ListarAsync()
    {
        return _context.Personas
            .OrderBy(p => p.Apellido).ThenBy(p => p.Nombre)
            .Select(p => new PersonaListaItem
            {
                IdPersona = p.IdPersona,
                NombreCompleto = p.Nombre + " " + p.Apellido,
                Email = p.Email,
                Telefono = p.Telefono,
                Sector = p.Clasificacion != null ? p.Clasificacion.Nombre : "—",
                CantidadComandas = _context.Comandas.Count(c => c.IdPersona == p.IdPersona)
            })
            .ToListAsync();
    }

    public Task<PersonaFormViewModel?> ObtenerAsync(int id)
    {
        return _context.Personas
            .Where(p => p.IdPersona == id)
            .Select(p => new PersonaFormViewModel
            {
                IdPersona = p.IdPersona,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Email = p.Email,
                Telefono = p.Telefono,
                IdClasificacion = p.IdClasificacion
            })
            .FirstOrDefaultAsync();
    }

    public Task<List<Clasificacion>> ListarSectoresAsync() =>
        _context.Clasificaciones.OrderBy(c => c.Nombre).ToListAsync();

    public async Task CrearAsync(PersonaFormViewModel modelo)
    {
        _context.Personas.Add(new Persona
        {
            Nombre = modelo.Nombre.Trim(),
            Apellido = modelo.Apellido.Trim(),
            Email = Limpiar(modelo.Email),
            Telefono = Limpiar(modelo.Telefono),
            IdClasificacion = modelo.IdClasificacion
        });
        await _context.SaveChangesAsync();
    }

    public async Task<bool> EditarAsync(PersonaFormViewModel modelo)
    {
        var persona = await _context.Personas.FindAsync(modelo.IdPersona);
        if (persona is null)
            return false;

        persona.Nombre = modelo.Nombre.Trim();
        persona.Apellido = modelo.Apellido.Trim();
        persona.Email = Limpiar(modelo.Email);
        persona.Telefono = Limpiar(modelo.Telefono);
        persona.IdClasificacion = modelo.IdClasificacion;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<string?> EliminarAsync(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona is null)
            return "El responsable no existe.";

        var comandas = await _context.Comandas.CountAsync(c => c.IdPersona == id);
        if (comandas > 0)
            return $"No se puede eliminar: figura como responsable en {comandas} comanda(s).";

        _context.Personas.Remove(persona);
        await _context.SaveChangesAsync();
        return null;
    }

    private static string? Limpiar(string? valor) =>
        string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
}
