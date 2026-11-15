namespace RecetarioMVC.Models;

public class Comanda
{
    public int IdComanda { get; set; }
    public int IdReceta { get; set; }
    public DateOnly Fecha { get; set; }
    public int Porciones { get; set; }
    public string UsuarioId { get; set; } = string.Empty;
    public int IdPersona { get; set; }

    public Receta Receta { get; set; } = null!;
    public ApplicationUser Usuario { get; set; } = null!;
    public Persona Persona { get; set; } = null!;
    public ICollection<Modificacion> Modificaciones { get; set; } = new List<Modificacion>();

    /// <summary>Lo que se cocinó: los ingredientes con las modificaciones ya aplicadas.</summary>
    public ICollection<IngredienteComanda> Ingredientes { get; set; } = new List<IngredienteComanda>();

    /// <summary>El procedimiento de la receta tal como estaba al generar la comanda.</summary>
    public ICollection<PasoComanda> Pasos { get; set; } = new List<PasoComanda>();
}
