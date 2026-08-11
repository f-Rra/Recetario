using Microsoft.EntityFrameworkCore;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Services;

/// <summary>
/// Corta una consulta en páginas. El recorte va en la base (`Skip`/`Take`), no
/// en memoria: si no, traer 5.000 filas para mostrar 25 dejaría el problema
/// intacto.
/// </summary>
public static class Paginacion
{
    public static async Task<Paginado<T>> AplicarAsync<T>(
        IQueryable<T> query, int pagina, int porPagina)
    {
        var total = await query.CountAsync();
        var totalPaginas = Math.Max(1, (int)Math.Ceiling(total / (double)porPagina));

        // Una página fuera de rango —por un enlace viejo o por filtrar— cae
        // en la última que existe, en vez de mostrar el vacío
        pagina = Math.Clamp(pagina, 1, totalPaginas);

        var items = await query
            .Skip((pagina - 1) * porPagina)
            .Take(porPagina)
            .ToListAsync();

        return new Paginado<T>
        {
            Items = items,
            Pagina = pagina,
            PorPagina = porPagina,
            Total = total
        };
    }
}
