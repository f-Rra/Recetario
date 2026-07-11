namespace RecetarioMVC.Models;

/// <summary>
/// Responsable de sector de cocina. En el schema viejo esta tabla también
/// respaldaba a los usuarios del sistema; ahora los usuarios son
/// ApplicationUser (Identity) y Persona queda solo para las comandas.
/// </summary>
public class Persona
{
    public int IdPersona { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public int? IdClasificacion { get; set; }

    public Clasificacion? Clasificacion { get; set; }

    public string NombreCompleto => $"{Nombre} {Apellido}";
}
