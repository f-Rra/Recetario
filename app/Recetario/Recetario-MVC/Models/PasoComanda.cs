namespace RecetarioMVC.Models;

/// <summary>
/// Un paso del procedimiento tal como estaba cuando se generó la comanda. Se copia de la
/// receta en ese momento, igual que los ingredientes (<see cref="IngredienteComanda"/>):
/// así el detalle y la reimpresión no cambian si después se edita el procedimiento.
/// </summary>
public class PasoComanda
{
    public int IdPasoComanda { get; set; }
    public int IdComanda { get; set; }
    public int NroPaso { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public Comanda Comanda { get; set; } = null!;
}
