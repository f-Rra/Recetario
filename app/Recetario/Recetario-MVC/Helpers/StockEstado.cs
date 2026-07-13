namespace RecetarioMVC.Helpers;

/// <summary>
/// Umbrales de estado de stock compartidos por dashboard y listados.
/// Crítico: por debajo del mínimo (semántica de vw_StockCritico del sistema viejo).
/// Bajo: dentro del margen del 25% sobre el mínimo.
/// </summary>
public static class StockEstado
{
    public const decimal MargenBajo = 1.25m;

    public const string Critico = "Crítico";
    public const string Bajo = "Bajo";
    public const string Normal = "Normal";

    public static string De(decimal stockActual, decimal stockMinimo)
    {
        if (stockActual < stockMinimo) return Critico;
        if (stockActual < stockMinimo * MargenBajo) return Bajo;
        return Normal;
    }
}
