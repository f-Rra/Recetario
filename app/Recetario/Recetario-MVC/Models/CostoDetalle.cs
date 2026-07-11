namespace RecetarioMVC.Models;

public class CostoDetalle
{
    public int IdCostoDetalle { get; set; }
    public int IdCosto { get; set; }
    public int IdIngrediente { get; set; }
    public decimal CantBruta { get; set; }
    public decimal CostoUnitario { get; set; }
    public decimal Subtotal { get; set; }

    public Costo Costo { get; set; } = null!;
    public Ingrediente Ingrediente { get; set; } = null!;
}
