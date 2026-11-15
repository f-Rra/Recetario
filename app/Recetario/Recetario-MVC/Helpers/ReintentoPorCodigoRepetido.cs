using Microsoft.EntityFrameworkCore;

namespace RecetarioMVC.Helpers;

// El código de una receta o un ingrediente (REC007, ING042) se arma mirando el más alto
// que hay y sumándole uno. Si dos personas dan de alta a la vez, las dos calculan el mismo
// y la base rechaza a la segunda por el índice único del código. Acá esa segunda alta
// se vuelve a intentar con el código siguiente, en lugar de terminar en un error.
public static class ReintentoPorCodigoRepetido
{
    private const int MaxIntentos = 5;

    /// <param name="armar">Arma la entidad nueva, calculando el código en ese momento.</param>
    /// <param name="codigoYaExiste">Dice si el código de esa entidad ya lo tiene otra en la base.</param>
    public static async Task<T> CrearAsync<T>(
        DbContext context, Func<Task<T>> armar, Func<T, Task<bool>> codigoYaExiste) where T : class
    {
        for (var intento = 1; ; intento++)
        {
            var entidad = await armar();
            context.Add(entidad);

            try
            {
                await context.SaveChangesAsync();
                return entidad;
            }
            catch (DbUpdateException) when (intento < MaxIntentos)
            {
                // La entidad no se guardó: se saca del contexto para que no se intente de nuevo
                context.Entry(entidad).State = EntityState.Detached;

                // Solo se reintenta si lo que pasó fue que alguien usó el código en el medio.
                // Cualquier otro error de guardado sigue de largo
                if (!await codigoYaExiste(entidad))
                    throw;
            }
        }
    }
}
