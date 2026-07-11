namespace RecetarioMVC.Models;

public class Receta
{
    public int IdReceta { get; set; }
    public string Codigo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public int IdClasificacion { get; set; }
    public int PorcionesBase { get; set; }
    public bool Activo { get; set; } = true;
    public string? Imagen { get; set; }

    public Clasificacion Clasificacion { get; set; } = null!;
    public ICollection<IngredienteReceta> Ingredientes { get; set; } = new List<IngredienteReceta>();
    public ICollection<Procedimiento> Procedimientos { get; set; } = new List<Procedimiento>();
}
