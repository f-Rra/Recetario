using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

public class AccesoController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public AccesoController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Home");

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel modelo, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        if (!ModelState.IsValid)
            return View(modelo);

        // Mismo mensaje genérico para usuario inexistente, inactivo o contraseña
        // incorrecta: no revelar qué emails existen en el sistema.
        var usuario = await _userManager.FindByEmailAsync(modelo.Email);
        if (usuario is null || !usuario.Activo)
        {
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");
            return View(modelo);
        }

        var resultado = await _signInManager.PasswordSignInAsync(
            usuario, modelo.Password, modelo.Recordarme, lockoutOnFailure: true);

        if (resultado.Succeeded)
            return LocalRedirect(returnUrl ?? Url.Action("Index", "Home")!);

        if (resultado.IsLockedOut)
            ModelState.AddModelError(string.Empty,
                "Cuenta bloqueada temporalmente por intentos fallidos. Probá de nuevo en unos minutos.");
        else
            ModelState.AddModelError(string.Empty, "Email o contraseña incorrectos.");

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction(nameof(Login));
    }

    [HttpGet]
    [Authorize]
    public IActionResult CambiarPassword() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public async Task<IActionResult> CambiarPassword(CambiarPasswordViewModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        var usuario = await _userManager.GetUserAsync(User);
        if (usuario is null)
            return RedirectToAction(nameof(Login));

        var resultado = await _userManager.ChangePasswordAsync(
            usuario, modelo.PasswordActual, modelo.PasswordNueva);

        if (resultado.Succeeded)
        {
            await _signInManager.RefreshSignInAsync(usuario);
            TempData["Exito"] = "Contraseña actualizada.";
            return RedirectToAction("Index", "Home");
        }

        foreach (var error in resultado.Errors)
            ModelState.AddModelError(string.Empty, error.Description);

        return View(modelo);
    }

    [HttpGet]
    public IActionResult Denegado() => View();
}
