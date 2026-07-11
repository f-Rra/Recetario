namespace RecetarioMVC.Models;

/// <summary>
/// Modificación de una comanda respecto de la receta base: sustituir,
/// agregar o eliminar un ingrediente.
/// </summary>
public class Modificacion
{
    public int IdModificacion { get; set; }
    public int IdComanda { get; set; }
    public TipoModificacion Tipo { get; set; }
    public int? IdIngredienteOriginal { get; set; }
    public int? IdIngredienteReemplazo { get; set; }
    public decimal Cantidad { get; set; }
    public int IdUnidad { get; set; }

    public Comanda Comanda { get; set; } = null!;
    public Ingrediente? IngredienteOriginal { get; set; }
    public Ingrediente? IngredienteReemplazo { get; set; }
    public Unidad Unidad { get; set; } = null!;
}
