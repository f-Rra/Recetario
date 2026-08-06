using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

// Una sola pantalla agrupada por rol: el alta y el reseteo de contraseña son
// modales, así que todo vuelve al Index (guía 31)
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
        return View(await ArmarPaginaAsync());
    }

    // Los campos del modal se llaman "Nuevo.*" porque la vista es la pantalla
    // entera: sin el prefijo, el binder no encuentra nada
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear([Bind(Prefix = "Nuevo")] CrearUsuarioViewModel modelo)
    {
        if (modelo.Rol is not (DbSeeder.RolAdmin or DbSeeder.RolCocina))
            ModelState.AddModelError(nameof(modelo.Rol), "Elegí un rol válido.");

        if (ModelState.IsValid)
        {
            var usuario = new ApplicationUser
            {
                UserName = modelo.Email,
                Email = modelo.Email,
                EmailConfirmed = true,
                Nombre = modelo.Nombre,
                Apellido = modelo.Apellido
            };

            var resultado = await _userManager.CreateAsync(usuario, modelo.Password);
            if (resultado.Succeeded)
            {
                await _userManager.AddToRoleAsync(usuario, modelo.Rol);
                TempData["Exito"] = $"Usuario {usuario.NombreCompleto} creado.";
                return RedirectToAction(nameof(Index));
            }

            // Contraseña débil, email repetido: lo dice Identity, no nosotros
            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        var pagina = await ArmarPaginaAsync();
        pagina.Nuevo = modelo;
        pagina.ModalAbierto = "modalCrear";
        return View(nameof(Index), pagina);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RestablecerPassword(
        [Bind(Prefix = "NuevaPassword")] RestablecerPasswordViewModel modelo)
    {
        var usuario = await _userManager.FindByIdAsync(modelo.Id);
        if (usuario is null)
            return NotFound();

        if (ModelState.IsValid)
        {
            // Sin mails de por medio: el token se genera y se consume acá mismo
            var token = await _userManager.GeneratePasswordResetTokenAsync(usuario);
            var resultado = await _userManager.ResetPasswordAsync(usuario, token, modelo.Password);

            if (resultado.Succeeded)
            {
                TempData["Exito"] = $"Contraseña de {usuario.NombreCompleto} actualizada.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var error in resultado.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
        }

        var pagina = await ArmarPaginaAsync();
        pagina.NuevaPassword = modelo;
        pagina.ModalAbierto = "modalPassword";
        return View(nameof(Index), pagina);
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

    private async Task<UsuariosPaginaViewModel> ArmarPaginaAsync()
    {
        var idActual = _userManager.GetUserId(User);
        var usuarios = await _userManager.Users
            .OrderBy(u => u.Apellido).ThenBy(u => u.Nombre)
            .ToListAsync();

        var pagina = new UsuariosPaginaViewModel();

        foreach (var usuario in usuarios)
        {
            var roles = await _userManager.GetRolesAsync(usuario);
            var rol = roles.FirstOrDefault();

            var item = new UsuarioListaViewModel
            {
                Id = usuario.Id,
                NombreCompleto = usuario.NombreCompleto,
                Email = usuario.Email ?? string.Empty,
                Rol = rol ?? "—",
                Activo = usuario.Activo,
                EsUsuarioActual = usuario.Id == idActual
            };

            switch (rol)
            {
                case DbSeeder.RolAdmin: pagina.Administradores.Add(item); break;
                case DbSeeder.RolCocina: pagina.Cocina.Add(item); break;
                default: pagina.SinRol.Add(item); break;
            }
        }

        return pagina;
    }
}
