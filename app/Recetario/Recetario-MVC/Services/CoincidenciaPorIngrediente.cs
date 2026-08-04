using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

/// <summary>
/// Por qué una receta apareció en una búsqueda cuando no coincide ni por
/// nombre ni por código: cuál de sus ingredientes matcheó. Lo comparten el
/// catálogo de la comandera y el listado de recetas.
/// </summary>
public static class CoincidenciaPorIngrediente
{
    public static async Task ResolverAsync(
        ApplicationDbContext context, IReadOnlyList<IRecetaBuscada> recetas, string texto)
    {
        var sinMotivo = recetas
            .Where(r => !r.Nombre.Contains(texto, StringComparison.OrdinalIgnoreCase) &&
                        !r.Codigo.Contains(texto, StringComparison.OrdinalIgnoreCase))
            .ToList();

        if (sinMotivo.Count == 0)
            return;

        var ids = sinMotivo.Select(r => r.IdReceta).ToList();
        var coincidencias = await context.IngredientesReceta
            .Where(ir => ids.Contains(ir.IdReceta) && ir.Ingrediente.Descripcion.Contains(texto))
            .Select(ir => new { ir.IdReceta, ir.Ingrediente.Descripcion })
            .ToListAsync();

        foreach (var receta in sinMotivo)
            receta.IngredienteCoincidente = coincidencias
                .FirstOrDefault(c => c.IdReceta == receta.IdReceta)?.Descripcion;
    }
}
