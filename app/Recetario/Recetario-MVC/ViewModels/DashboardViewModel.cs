namespace RecetarioMVC.ViewModels;

public class DashboardViewModel
{
    public int RecetasActivas { get; set; }
    public int TotalIngredientes { get; set; }
    public int IngredientesCriticos { get; set; }
    public decimal CostoPromedioPorcion { get; set; }
    public List<IngredienteStockItem> StockBajo { get; set; } = new();
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
