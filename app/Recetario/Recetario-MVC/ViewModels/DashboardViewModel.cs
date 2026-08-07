namespace RecetarioMVC.ViewModels;

public class DashboardViewModel
{
    // ---------- Las cuatro tarjetas ----------

    public int RecetasActivas { get; set; }

    /// <summary>Sin ingredientes o sin procedimiento: saldrían a medias.</summary>
    public int RecetasIncompletas { get; set; }

    public int TotalIngredientes { get; set; }
    public int IngredientesBodega { get; set; }
    public int IngredientesCamara { get; set; }

    /// <summary>
    /// Porciones producidas en el mes. No son comensales: un servicio de tres
    /// recetas para 200 personas son 600 porciones.
    /// </summary>
    public int PorcionesDelMes { get; set; }

    public int PorcionesMesAnterior { get; set; }

    /// <summary>Stock actual valorizado al precio vigente de cada ingrediente.</summary>
    public decimal ValorInventario { get; set; }

    /// <summary>Cuántos entraron en ese valor: sin precio no se puede valorizar.</summary>
    public int IngredientesValorizados { get; set; }

    /// <summary>Cuánto cambió la producción contra el mes pasado. Null si no hay con qué comparar.</summary>
    public decimal? VariacionPorciones => PorcionesMesAnterior == 0
        ? null
        : Math.Round((PorcionesDelMes - PorcionesMesAnterior) / (decimal)PorcionesMesAnterior * 100, 1);

    // ---------- Los paneles ----------

    public List<IngredienteStockItem> StockBajo { get; set; } = new();
    public List<ComandaRecienteItem> UltimasComandas { get; set; } = new();
    public List<ActividadItem> Actividad { get; set; } = new();
}

public class IngredienteStockItem
{
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }
    public string Unidad { get; set; } = string.Empty;

    public bool EsCritico => StockActual < StockMinimo;
}

public class ComandaRecienteItem
{
    public int IdComanda { get; set; }
    public DateOnly Fecha { get; set; }
    public string Receta { get; set; } = string.Empty;
    public int Porciones { get; set; }
}

/// <summary>
/// Una línea del registro de actividad. Se arma con lo que el sistema audita
/// de verdad: comandas generadas y movimientos de stock. Precios y ediciones
/// de recetas no guardan cuándo se hicieron, así que no aparecen.
/// </summary>
public class ActividadItem
{
    public DateOnly Fecha { get; set; }

    /// <summary>Hora del movimiento; las comandas solo guardan el día.</summary>
    public TimeOnly? Hora { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;

    /// <summary>comanda · entrada · salida · ajuste, para el color del punto.</summary>
    public string Tipo { get; set; } = string.Empty;
}
