namespace RecetarioMVC.Helpers;

// El reloj de toda la aplicación. Un servidor en la nube corre en UTC y una PC en
// Argentina en UTC-3: si cada línea le pregunta la hora a la máquina con DateTime.Now, la
// misma comanda queda guardada con distinta fecha según dónde esté publicada la app. Con
// este reloj la respuesta es siempre la de Argentina.
//
// Hereda de TimeProvider, la clase de .NET para esto, y solo cambia la zona:
// GetUtcNow() sigue devolviendo la hora UTC de siempre y GetLocalNow() devuelve la de
// Argentina. En los tests se reemplaza por un reloj falso con hora fija.
public sealed class RelojArgentina : TimeProvider
{
    public static readonly TimeZoneInfo Zona = Buscar();

    public override TimeZoneInfo LocalTimeZone => Zona;

    // .NET acepta el nombre IANA en Windows y en Linux desde que usa ICU, pero no en todas
    // las instalaciones: si no lo encuentra, prueba el de Windows
    private static TimeZoneInfo Buscar()
    {
        foreach (var nombre in new[] { "America/Argentina/Buenos_Aires", "Argentina Standard Time" })
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(nombre); }
            catch (TimeZoneNotFoundException) { }
            catch (InvalidTimeZoneException) { }
        }

        // Argentina no tiene horario de verano desde 2009, así que un desfase fijo de tres
        // horas es correcto y no una aproximación
        return TimeZoneInfo.CreateCustomTimeZone("recetario-arg", TimeSpan.FromHours(-3), "Argentina", "Argentina");
    }
}

public static class RelojExtensiones
{
    // La fecha de hoy en Argentina. Reemplaza a DateOnly.FromDateTime(DateTime.Today).
    public static DateOnly Hoy(this TimeProvider reloj) => DateOnly.FromDateTime(reloj.GetLocalNow().DateTime);

    // La fecha y la hora de ahora en Argentina. Reemplaza a DateTime.Now.
    public static DateTime Ahora(this TimeProvider reloj) => reloj.GetLocalNow().DateTime;
}
