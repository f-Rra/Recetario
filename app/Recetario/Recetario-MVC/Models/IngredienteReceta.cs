namespace RecetarioMVC.Models;

/// <summary>
/// Ingrediente que compone una receta. CantBruta = CantNeta / (Rendimiento/100):
/// lo que hay que comprar considerando el desperdicio del ingrediente.
/// </summary>
public class IngredienteReceta
{
    public int IdReceta { get; set; }
    public int IdIngrediente { get; set; }
    public decimal CantNeta { get; set; }
    public decimal Rendimiento { get; set; } = 100;
    public decimal CantBruta { get; set; }
    public int IdUnidad { get; set; }

    public Receta Receta { get; set; } = null!;
    public Ingrediente Ingrediente { get; set; } = null!;
    public Unidad Unidad { get; set; } = null!;
}
