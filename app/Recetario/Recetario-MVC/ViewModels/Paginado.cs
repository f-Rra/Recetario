namespace RecetarioMVC.ViewModels;

/// <summary>
/// Una página de resultados junto con el total, para que la pantalla pueda
/// decir "26–50 de 128" y no solo "página 2".
/// </summary>
public class Paginado<T>
{
    public List<T> Items { get; set; } = new();
    public int Pagina { get; set; } = 1;
    public int PorPagina { get; set; }
    public int Total { get; set; }

    public int TotalPaginas => PorPagina <= 0 ? 1 : Math.Max(1, (int)Math.Ceiling(Total / (double)PorPagina));
    public bool HayAnterior => Pagina > 1;
    public bool HaySiguiente => Pagina < TotalPaginas;

    /// <summary>Número de la primera fila de esta página, contando desde 1.</summary>
    public int Desde => Total == 0 ? 0 : (Pagina - 1) * PorPagina + 1;

    public int Hasta => Math.Min(Pagina * PorPagina, Total);

    /// <summary>
    /// La misma página con los ítems transformados. Sirve cuando la consulta
    /// trae una forma y la pantalla necesita otra.
    /// </summary>
    public Paginado<TDestino> Mapear<TDestino>(Func<T, TDestino> proyeccion) => new()
    {
        Items = Items.Select(proyeccion).ToList(),
        Pagina = Pagina,
        PorPagina = PorPagina,
        Total = Total
    };
}

/// <summary>Lo que necesita el control de paginado para dibujarse.</summary>
public class PaginadorViewModel
{
    public int Pagina { get; set; }
    public int TotalPaginas { get; set; }
    public int Total { get; set; }
    public int Desde { get; set; }
    public int Hasta { get; set; }
    public string Singular { get; set; } = "resultado";
    public string Plural { get; set; } = "resultados";

    public bool HayAnterior() => Pagina > 1;
    public bool HaySiguiente() => Pagina < TotalPaginas;

    public static PaginadorViewModel De<T>(Paginado<T> pagina, string singular, string plural) => new()
    {
        Pagina = pagina.Pagina,
        TotalPaginas = pagina.TotalPaginas,
        Total = pagina.Total,
        Desde = pagina.Desde,
        Hasta = pagina.Hasta,
        Singular = singular,
        Plural = plural
    };
}
