using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class ProveedorFormViewModel
{
    public int IdProveedor { get; set; }

    [Required(ErrorMessage = "Ingresá el nombre.")]
    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Contacto")]
    public string? Contacto { get; set; }

    [StringLength(20, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Teléfono")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Email")]
    public string? Email { get; set; }

    [StringLength(255, ErrorMessage = "Máximo {1} caracteres.")]
    [Display(Name = "Dirección")]
    public string? Direccion { get; set; }
}

public class ProveedorListaItem
{
    public int IdProveedor { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Contacto { get; set; }
    public string? Telefono { get; set; }
    public string? Email { get; set; }

    /// <summary>La ficha la muestra, así que viaja con el listado.</summary>
    public string? Direccion { get; set; }

    public int CantidadPrecios { get; set; }
}

/// <summary>La pantalla: las fichas y los formularios de sus modales.</summary>
public class ProveedoresPaginaViewModel
{
    public string? Busqueda { get; set; }
    public List<ProveedorListaItem> Lista { get; set; } = new();

    public ProveedorFormViewModel Nuevo { get; set; } = new();
    public ProveedorFormViewModel Edicion { get; set; } = new();

    /// <summary>Se reabre el modal cuando el formulario vuelve con errores.</summary>
    public string? ModalAbierto { get; set; }
}
