using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Controllers;

[Authorize(Roles = DbSeeder.RolAdmin)]
public class CostosController : Controller
{
    private readonly ICosteoService _costeo;
    private readonly UserManager<ApplicationUser> _userManager;

    public CostosController(ICosteoService costeo, UserManager<ApplicationUser> userManager)
    {
        _costeo = costeo;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Costear(int id)
    {
        var modelo = await _costeo.ObtenerPaginaAsync(id);
        if (modelo is null)
            return NotFound();

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Calcular(int idReceta, int porciones)
    {
        var modelo = await _costeo.ObtenerPaginaAsync(idReceta);
        if (modelo is null)
            return NotFound();

        if (porciones <= 0)
        {
            TempData["Error"] = "Las porciones deben ser mayores a cero.";
            return RedirectToAction(nameof(Costear), new { id = idReceta });
        }

        modelo.Porciones = porciones;
        modelo.Resultado = await _costeo.CalcularAsync(idReceta, porciones);
        return View(nameof(Costear), modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(int idReceta, int porciones)
    {
        var usuarioId = _userManager.GetUserId(User)!;
        var idCosto = await _costeo.RegistrarAsync(idReceta, porciones, usuarioId);

        if (idCosto is null)
            TempData["Error"] = "No se pudo registrar el costeo. Verificá que todos los ingredientes tengan precio.";
        else
            TempData["Exito"] = "Costeo registrado en el historial.";

        return RedirectToAction(nameof(Costear), new { id = idReceta });
    }
}
