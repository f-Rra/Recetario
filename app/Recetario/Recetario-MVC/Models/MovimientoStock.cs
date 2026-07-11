namespace RecetarioMVC.Models;

public class MovimientoStock
{
    public int IdMovimiento { get; set; }
    public int IdIngrediente { get; set; }
    public TipoMovimiento Tipo { get; set; }
    public decimal Cantidad { get; set; }
    public int IdUnidad { get; set; }
    public DateTime Fecha { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public string? Observaciones { get; set; }

    public Ingrediente Ingrediente { get; set; } = null!;
    public Unidad Unidad { get; set; } = null!;
    public ApplicationUser Usuario { get; set; } = null!;
}
