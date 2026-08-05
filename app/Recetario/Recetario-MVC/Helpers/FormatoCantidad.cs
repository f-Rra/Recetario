namespace RecetarioMVC.Helpers;

/// <summary>
/// Muestra las cantidades como las diría alguien en la cocina: sin ceros de
/// relleno y en la unidad que pide menos números (1800 g → 1,8 kg,
/// 0,8 kg → 800 g). Solo cambia la presentación: lo guardado no se toca.
/// </summary>
public static class FormatoCantidad
{
    public static string Formatear(decimal cantidad, string unidad)
    {
        var (valor, abreviatura) = Convertir(cantidad, unidad);
        return $"{Numero(valor)} {abreviatura}";
    }

    /// <summary>Solo el número, sin la unidad.</summary>
    public static string Numero(decimal cantidad)
    {
        // "#,0.##" recorta los decimales que no aportan: 15,00 → 15 · 1,80 → 1,8
        return Math.Round(cantidad, 2).ToString("#,0.##");
    }

    /// <summary>
    /// En qué unidad conviene leer una cantidad. Es pública porque el
    /// redondeo (<see cref="RedondeoCocina"/>) aplica su regla sobre la unidad
    /// que se muestra, no sobre la que está guardada.
    /// </summary>
    public static (decimal Valor, string Unidad) UnidadDeLectura(decimal cantidad, string unidad) =>
        Convertir(cantidad, unidad);

    private static (decimal Valor, string Unidad) Convertir(decimal cantidad, string unidad)
    {
        var esCero = cantidad == 0;

        return unidad.ToLowerInvariant() switch
        {
            "g" when Math.Abs(cantidad) >= 1000 => (cantidad / 1000, "kg"),
            "kg" when !esCero && Math.Abs(cantidad) < 1 => (cantidad * 1000, "g"),
            "ml" when Math.Abs(cantidad) >= 1000 => (cantidad / 1000, "L"),
            "l" when !esCero && Math.Abs(cantidad) < 1 => (cantidad * 1000, "ml"),
            _ => (cantidad, unidad)
        };
    }
}
