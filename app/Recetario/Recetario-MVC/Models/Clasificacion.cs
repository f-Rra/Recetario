namespace RecetarioMVC.Models;

/// <summary>
/// Clasificación de recetas que también funciona como sector de cocina
/// (Entrada, Plato Principal, Postre...). Las personas responsables de
/// sector referencian su clasificación.
/// </summary>
public class Clasificacion
{
    public int IdClasificacion { get; set; }
    public string Nombre { get; set; } = string.Empty;

    public ICollection<Receta> Recetas { get; set; } = new List<Receta>();
    public ICollection<Persona> Personas { get; set; } = new List<Persona>();
}
