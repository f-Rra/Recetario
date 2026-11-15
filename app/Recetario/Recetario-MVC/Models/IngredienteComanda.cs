namespace RecetarioMVC.Models;

/// <summary>
/// Un ingrediente de una comanda tal como se cocinó: la receta escalada a los
/// comensales con las modificaciones ya aplicadas. Se guarda al generar la comanda
/// para que el detalle y la reimpresión no dependan de cómo esté la receta después.
/// </summary>
public class IngredienteComanda
{
    public int IdIngredienteComanda { get; set; }
    public int IdComanda { get; set; }
    public int IdIngrediente { get; set; }
    public decimal Cantidad { get; set; }
    public int IdUnidad { get; set; }

    public Comanda Comanda { get; set; } = null!;
    public Ingrediente Ingrediente { get; set; } = null!;
    public Unidad Unidad { get; set; } = null!;
}
