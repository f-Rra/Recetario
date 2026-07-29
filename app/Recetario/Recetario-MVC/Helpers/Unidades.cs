namespace RecetarioMVC.Helpers;

/// <summary>
/// Equivalencias entre unidades de la misma familia: kilos con gramos y
/// litros con mililitros. Permite sustituir un ingrediente por otro medido
/// en otra unidad sin pedirle al cocinero que haga la cuenta.
/// </summary>
public static class Unidades
{
    private static readonly Dictionary<string, (string Familia, decimal EnUnidadChica)> Tabla =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["kg"] = ("masa", 1000m),
            ["g"] = ("masa", 1m),
            ["L"] = ("volumen", 1000m),
            ["ml"] = ("volumen", 1m)
        };

    /// <summary>Si una cantidad puede pasarse de una unidad a la otra.</summary>
    public static bool SonEquivalentes(string unidadA, string unidadB)
    {
        if (string.Equals(unidadA, unidadB, StringComparison.OrdinalIgnoreCase))
            return true;

        return Tabla.TryGetValue(unidadA, out var a)
               && Tabla.TryGetValue(unidadB, out var b)
               && a.Familia == b.Familia;
    }

    /// <returns>La cantidad en la unidad destino, o null si no son equivalentes.</returns>
    public static decimal? Convertir(decimal cantidad, string desde, string hacia)
    {
        if (string.Equals(desde, hacia, StringComparison.OrdinalIgnoreCase))
            return cantidad;

        if (!Tabla.TryGetValue(desde, out var origen) ||
            !Tabla.TryGetValue(hacia, out var destino) ||
            origen.Familia != destino.Familia)
        {
            return null;
        }

        return Math.Round(cantidad * origen.EnUnidadChica / destino.EnUnidadChica, 4);
    }
}
