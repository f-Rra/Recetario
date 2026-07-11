using Microsoft.AspNetCore.Identity;

namespace RecetarioMVC.Models;

public class ApplicationUser : IdentityUser
{
    public string Nombre { get; set; } = string.Empty;
    public string Apellido { get; set; } = string.Empty;
    public bool Activo { get; set; } = true;

    public string NombreCompleto => $"{Nombre} {Apellido}";
}
