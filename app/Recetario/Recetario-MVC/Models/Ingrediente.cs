namespace RecetarioMVC.Models;

public class Ingrediente
{
    public int IdIngrediente { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int IdUnidad { get; set; }
    public decimal StockActual { get; set; }
    public decimal StockMinimo { get; set; }

    public Unidad Unidad { get; set; } = null!;
    public ICollection<PrecioIngrediente> Precios { get; set; } = new List<PrecioIngrediente>();
}
