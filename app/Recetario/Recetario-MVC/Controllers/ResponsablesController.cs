using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Data;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

[Authorize(Roles = DbSeeder.RolAdmin)]
public class ResponsablesController : Controller
{
    private readonly IPersonaService _personas;

    public ResponsablesController(IPersonaService personas)
    {
        _personas = personas;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _personas.ListarAsync());
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        await CargarSectoresAsync();
        return View(new PersonaFormViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(PersonaFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarSectoresAsync();
            return View(modelo);
        }

        await _personas.CrearAsync(modelo);
        TempData["Exito"] = $"Responsable {modelo.Nombre.Trim()} {modelo.Apellido.Trim()} creado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var modelo = await _personas.ObtenerAsync(id);
        if (modelo is null)
            return NotFound();

        await CargarSectoresAsync();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(PersonaFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarSectoresAsync();
            return View(modelo);
        }

        if (!await _personas.EditarAsync(modelo))
            return NotFound();

        TempData["Exito"] = "Responsable actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var error = await _personas.EliminarAsync(id);
        if (error is null)
            TempData["Exito"] = "Responsable eliminado.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Index));
    }

    private async Task CargarSectoresAsync()
    {
        var sectores = await _personas.ListarSectoresAsync();
        ViewBag.Sectores = sectores
            .Select(c => new SelectListItem(c.Nombre, c.IdClasificacion.ToString()))
            .ToList();
    }
}
