using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Helpers;

/// <summary>
/// Reglas para que las modificaciones de una misma receta no se contradigan.
/// Sin esto se podía quitar un ingrediente y además sustituirlo: la comanda
/// aplicaba solo la primera, pero guardaba y mostraba las dos.
/// </summary>
public static class ReglasModificacion
{
    /// <summary>
    /// Revisa una modificación nueva contra las que la receta ya tiene en el carrito.
    /// Devuelve el motivo del rechazo, o null si se puede agregar.
    /// </summary>
    public static string? Validar(IReadOnlyCollection<ModificacionCarrito> existentes, ModificacionCarrito nueva)
    {
        if (nueva.Tipo == TipoModificacion.Adicion)
        {
            // "Sin harina" y "agregar harina" a la vez no dicen cuánta harina lleva el plato
            var yaSale = existentes.Any(m =>
                m.Tipo != TipoModificacion.Adicion &&
                m.IdIngredienteOriginal == nueva.IdIngredienteReemplazo);

            return yaSale
                ? $"{nueva.NombreReemplazo} ya está quitado o sustituido en esta receta. " +
                  "Quitá esa modificación antes de agregarlo."
                : null;
        }

        if (nueva.Tipo == TipoModificacion.Sustitucion &&
            nueva.IdIngredienteOriginal == nueva.IdIngredienteReemplazo)
            return $"No se puede sustituir {nueva.NombreOriginal} por sí mismo.";

        // Cada ingrediente de la receta se quita o se sustituye una sola vez
        var yaModificado = existentes.Any(m =>
            m.Tipo != TipoModificacion.Adicion &&
            m.IdIngredienteOriginal == nueva.IdIngredienteOriginal);

        if (yaModificado)
            return $"{nueva.NombreOriginal} ya tiene una modificación en esta receta. " +
                   "Quitala antes de cargar otra.";

        var yaAgregado = existentes.Any(m =>
            m.Tipo == TipoModificacion.Adicion &&
            m.IdIngredienteReemplazo == nueva.IdIngredienteOriginal);

        return yaAgregado
            ? $"{nueva.NombreOriginal} ya está agregado en esta receta. " +
              "Quitá esa modificación antes de quitarlo o sustituirlo."
            : null;
    }
}
