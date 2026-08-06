using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class CrearUsuarioViewModel
{
    [Required(ErrorMessage = "Ingresá el nombre.")]
    [StringLength(100)]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el apellido.")]
    [StringLength(100)]
    [Display(Name = "Apellido")]
    public string Apellido { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá el email.")]
    [EmailAddress(ErrorMessage = "El email no tiene un formato válido.")]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá la contraseña inicial.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña inicial")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Elegí un rol.")]
    [Display(Name = "Rol")]
    public string Rol { get; set; } = string.Empty;
}

public class UsuarioListaViewModel
{
    public string Id { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public bool EsUsuarioActual { get; set; }
}

/// <summary>
/// Nueva contraseña puesta por un admin. Es la única forma de recuperar una
/// cuenta: el sistema no manda mails, así que sin esto quien la olvida queda
/// afuera para siempre.
/// </summary>
public class RestablecerPasswordViewModel
{
    public string Id { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá la contraseña nueva.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña nueva")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>La pantalla: los usuarios por rol y los formularios de sus modales.</summary>
public class UsuariosPaginaViewModel
{
    public List<UsuarioListaViewModel> Administradores { get; set; } = new();
    public List<UsuarioListaViewModel> Cocina { get; set; } = new();

    /// <summary>Cuentas sin rol: no pueden hacer nada hasta que se les asigne uno.</summary>
    public List<UsuarioListaViewModel> SinRol { get; set; } = new();

    public CrearUsuarioViewModel Nuevo { get; set; } = new();
    public RestablecerPasswordViewModel NuevaPassword { get; set; } = new();

    /// <summary>Se reabre el modal cuando el formulario vuelve con errores.</summary>
    public string? ModalAbierto { get; set; }
}
