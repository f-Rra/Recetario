using System.ComponentModel.DataAnnotations;

namespace RecetarioMVC.ViewModels;

public class CambiarPasswordViewModel
{
    [Required(ErrorMessage = "Ingresá tu contraseña actual.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña actual")]
    public string PasswordActual { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresá la contraseña nueva.")]
    [DataType(DataType.Password)]
    [Display(Name = "Contraseña nueva")]
    public string PasswordNueva { get; set; } = string.Empty;

    [Required(ErrorMessage = "Repetí la contraseña nueva.")]
    [DataType(DataType.Password)]
    [Compare(nameof(PasswordNueva), ErrorMessage = "Las contraseñas no coinciden.")]
    [Display(Name = "Confirmar contraseña nueva")]
    public string ConfirmarPassword { get; set; } = string.Empty;
}
