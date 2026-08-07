using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

// La pantalla lleva el selector de recetas al costado: se cambia de receta sin
// volver al listado (guía 32)
[Authorize(Roles = DbSeeder.RolAdmin)]
public class CostosController : Controller
{
    private readonly ICosteoService _costeo;
    private readonly IRecetaService _recetas;
    private readonly UserManager<ApplicationUser> _userManager;

    public CostosController(
        ICosteoService costeo,
        IRecetaService recetas,
        UserManager<ApplicationUser> userManager)
    {
        _costeo = costeo;
        _recetas = recetas;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Costear(int? id)
    {
        var recetas = await _recetas.ListarAsync(null, null);

        // Sin id —o con uno que ya no existe— se abre la primera
        var elegida = id.HasValue && recetas.Any(r => r.IdReceta == id.Value)
            ? id.Value
            : recetas.FirstOrDefault()?.IdReceta;

        if (elegida is null)
            return View(new CostearPaginaViewModel());

        var modelo = await _costeo.ObtenerPaginaAsync(elegida.Value);
        if (modelo is null)
            return NotFound();

        modelo.Recetas = recetas;
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
        modelo.Recetas = await _recetas.ListarAsync(null, null);
        return View(nameof(Costear), modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Pdf(int id)
    {
        var costeo = await _costeo.ObtenerRegistradoAsync(id);
        if (costeo is null)
            return NotFound();

        var pdf = Services.Pdf.CosteoPdf.Generar(costeo);
        return File(pdf, "application/pdf", $"costeo-{costeo.Codigo}-{costeo.Fecha:yyyy-MM-dd}.pdf");
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
