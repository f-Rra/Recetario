using Microsoft.EntityFrameworkCore;

namespace RecetarioMVC.Helpers;

// Cuando dos personas cambian el stock de un ingrediente a la vez, EF se da cuenta (el
// stock tiene un token de concurrencia, ver IngredienteConfiguracion) y lanza
// DbUpdateConcurrencyException en lugar de pisar el dato de la otra. Acá se vuelve a
// correr la operación entera: tiene que leer adentro lo que necesita, así el segundo
// intento trabaja con el stock actualizado.
public static class ReintentoPorConcurrencia
{
    private const int MaxIntentos = 3;

    // Si los tres intentos chocan, la excepción sale para que la maneje quien llamó
    public static async Task EjecutarAsync(DbContext context, Func<Task> operacion)
    {
        for (var intento = 1; ; intento++)
        {
            try
            {
                await operacion();
                return;
            }
            catch (DbUpdateConcurrencyException) when (intento < MaxIntentos)
            {
                // Se descarta todo lo leído y lo a medio guardar: el próximo intento lo pide de nuevo
                context.ChangeTracker.Clear();
            }
        }
    }
}
