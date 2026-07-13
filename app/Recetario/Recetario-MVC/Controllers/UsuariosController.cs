using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

[Authorize(Roles = DbSeeder.RolAdmin)]
public class UsuariosController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UsuariosController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var idActual = _userManager.GetUserId(User);
        var usuarios = await _userManager.Users
            .OrderBy(u => u.Apellido).ThenBy(u => u.Nombre)
            .ToListAsync();

        var modelo = new List<UsuarioListaViewModel>();
        foreach (var usuario in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            modelo.Add(new UsuarioListaViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email ?? string.Empty,
                Rol = roles.FirstOrDefault() ?? "—",
                Activo = usuario.Activo,
                EsUsuarioActual = usuario.Id == idActual
            });
        }

        return View(modelo);
    }

    [HttpGet]
    public IActionResult Crear() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(CrearUsuarioViewModel modelo)
    {
        if (modelo.Rol is not (DbSeeder.RolAdmin or DbSeeder.RolCocina))
            ModelState.AddModelError(nameof(modelo.Rol), "Elegí un rol válido.");

        if (!ModelState.IsValid)
            return View(modelo);

        var usuario = new ApplicationUser
        {
            UserName = modelo.Email,
            Email = modelo.Email,
            EmailConfirmed = true,
            Nombre = modelo.Nombre,
            Apellido = modelo.Apellido
        };

        var resultado = await _userManager.CreateAsync(usuario, modelo.Password);
        if (!resultado.Succeeded)
        {
            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(modelo);
        }

        await _userManager.AddToRoleAsync(usuario, modelo.Rol);
        TempData["Exito"] = $"Usuario {usuario.NombreCompleto} creado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> CambiarEstado(string id)
    {
        if (id == _userManager.GetUserId(User))
        {
            TempData["Error"] = "No podés desactivar tu propio usuario.";
            return RedirectToAction(nameof(Index));
        }

        var usuario = await _userManager.FindByIdAsync(id);
        if (usuario is null)
            return NotFound();

        usuario.Activo = !usuario.Activo;
        await _userManager.UpdateAsync(usuario);
        TempData["Exito"] = $"Usuario {usuario.NombreCompleto} {(usuario.Activo ? "activado" : "desactivado")}.";
        return RedirectToAction(nameof(Index));
    }
}
