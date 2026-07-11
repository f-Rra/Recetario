namespace RecetarioMVC.Models;

/// <summary>
/// Costeo de una receta en un momento dado. El desglose por ingrediente se
/// persiste en Detalles (mejora sobre el schema viejo, que lo calculaba al
/// vuelo y perdía el histórico al cambiar los precios).
/// </summary>
public class Costo
{
    public int IdCosto { get; set; }
    public int IdReceta { get; set; }
    public DateOnly Fecha { get; set; }
    public int Porciones { get; set; }
    public decimal CostoTotal { get; set; }
    public decimal CostoUnitario { get; set; }
    public string UsuarioId { get; set; } = string.Empty;

    public Receta Receta { get; set; } = null!;
    public ApplicationUser Usuario { get; set; } = null!;
    public ICollection<CostoDetalle> Detalles { get; set; } = new List<CostoDetalle>();
}
