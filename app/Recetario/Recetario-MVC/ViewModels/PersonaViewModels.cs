using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class PersonaFormViewModel
{
    public int IdPersona { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre.")]
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el apellido.")]
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [Display(Name = "Sector")]
    public int? IdClasificacion { get; set; }
}

public class PersonaListaItem
{
    public int IdPersona { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Telefono { get; set; }
    public int? IdClasificacion { get; set; }
    public string Sector { get; set; } = string.Empty;
    public int CantidadComandas { get; set; }
}

/// <summary>
/// Un sector con quienes lo cubren. Los sectores vacíos vienen igual: son los
/// que hacen fallar la generación de una comanda, así que hay que verlos.
/// </summary>
public class SectorConResponsables
{
    public int IdClasificacion { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public List<PersonaListaItem> Responsables { get; set; } = new();

    public bool SinCubrir => Responsables.Count == 0;
}

public class ResponsablesPorSector
{
    public List<SectorConResponsables> Sectores { get; set; } = new();

    /// <summary>Gente cargada sin sector: no se le asigna ninguna comanda.</summary>
    public List<PersonaListaItem> SinSector { get; set; } = new();

    public int Total => Sectores.Sum(s => s.Responsables.Count) + SinSector.Count;
    public int SectoresSinCubrir => Sectores.Count(s => s.SinCubrir);
}

/// <summary>La pantalla: los sectores y los formularios de sus modales.</summary>
public class ResponsablesPaginaViewModel
{
    public ResponsablesPorSector Datos { get; set; } = new();

    public PersonaFormViewModel Nuevo { get; set; } = new();
    public PersonaFormViewModel Edicion { get; set; } = new();

    /// <summary>Se reabre el modal cuando el formulario vuelve con errores.</summary>
    public string? ModalAbierto { get; set; }
}
