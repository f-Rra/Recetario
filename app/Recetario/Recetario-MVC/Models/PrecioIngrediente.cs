namespace RecetarioMVC.Models;

/// <summary>
/// Precio de un ingrediente según proveedor, con historial: cada cambio de
/// precio es una fila nueva. Mejora sobre el schema viejo, cuya PK compuesta
/// (ingrediente, proveedor) pisaba el precio anterior.
/// </summary>
public class PrecioIngrediente
{
    public int IdPrecio { get; set; }
    public int IdIngrediente { get; set; }
    public int IdProveedor { get; set; }
    public decimal Precio { get; set; }
    public DateOnly FechaVigencia { get; set; }

    public Ingrediente Ingrediente { get; set; } = null!;
    public Proveedor Proveedor { get; set; } = null!;
}
