using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Data;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

// Una sola pantalla por sectores: las altas y ediciones son modales (guía 30)
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
        return View(await ArmarPaginaAsync());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(PersonaFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            var pagina = await ArmarPaginaAsync();
            pagina.Nuevo = modelo;
            pagina.ModalAbierto = "modalCrear";
            return View(nameof(Index), pagina);
        }

        await _personas.CrearAsync(modelo);
        TempData["Exito"] = $"Responsable {modelo.Nombre.Trim()} {modelo.Apellido.Trim()} creado.";
        return RedirectToAction(nameof(Index));
    }

    // El prefijo separa este formulario del de alta: los dos modales viven en
    // la misma página y, sin prefijo, un error de uno se pinta en el otro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar([Bind(Prefix = "Edicion")] PersonaFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            var pagina = await ArmarPaginaAsync();
            pagina.Edicion = modelo;
            pagina.ModalAbierto = "modalEditar";
            return View(nameof(Index), pagina);
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

    private async Task<ResponsablesPaginaViewModel> ArmarPaginaAsync()
    {
        var sectores = await _personas.ListarSectoresAsync();
        ViewBag.Sectores = sectores
            .Select(c => new SelectListItem(c.Nombre, c.IdClasificacion.ToString()))
            .ToList();

        return new ResponsablesPaginaViewModel
        {
            Datos = await _personas.ListarPorSectorAsync()
        };
    }
}
