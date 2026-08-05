namespace RecetarioMVC.Helpers;

/// <summary>
/// Lleva una cantidad escalada a una medida que se pueda sacar del depósito:
/// nadie toma 1,51 cabezas de ajo ni pesa 6,8 g de sal. A diferencia de
/// <see cref="FormatoCantidad"/>, esto no es presentación: lo redondeado es lo
/// que se descuenta del stock y lo que sale impreso.
/// </summary>
public static class RedondeoCocina
{
    /// <summary>
    /// Redondea al paso de la unidad en la que la cantidad se lee, y devuelve
    /// el resultado en la unidad original del ingrediente.
    /// </summary>
    public static decimal Redondear(decimal cantidad, string unidad)
    {
        if (cantidad <= 0)
            return cantidad;

        // La regla depende de cómo se lee: 3200 g son 3,2 kg y redondean a 3 kg
        var (valor, lectura) = FormatoCantidad.UnidadDeLectura(cantidad, unidad);
        var paso = Paso(lectura);

        var redondeado = Math.Round(valor / paso, MidpointRounding.AwayFromZero) * paso;

        // Una comanda chica no puede hacer desaparecer un ingrediente
        if (redondeado <= 0)
            redondeado = paso;

        return Unidades.Convertir(redondeado, lectura, unidad) ?? redondeado;
    }

    /// <summary>Medida más chica que tiene sentido pedir en cada unidad.</summary>
    private static decimal Paso(string unidad) => unidad.ToLowerInvariant() switch
    {
        // Los kilos y litros viven casi siempre entre 1 y 5 —abajo de 1 ya se
        // leen en gramos—, así que el entero se comería hasta un 30%
        "kg" or "l" => 0.5m,
        // Gramos, mililitros y unidades: la fracción no se puede ni pesar ni contar
        _ => 1m
    };
}
