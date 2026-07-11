namespace RecetarioMVC.Models;

public class Procedimiento
{
    public int IdProcedimiento { get; set; }
    public int IdReceta { get; set; }
    public int NroPaso { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public Receta Receta { get; set; } = null!;
}
