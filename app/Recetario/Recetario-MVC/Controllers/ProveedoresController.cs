using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecetarioMVC.Data;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

[Authorize(Roles = DbSeeder.RolAdmin)]
public class ProveedoresController : Controller
{
    private readonly IProveedorService _proveedores;

    public ProveedoresController(IProveedorService proveedores)
    {
        _proveedores = proveedores;
    }

    public async Task<IActionResult> Index(string? busqueda)
    {
        ViewData["Busqueda"] = busqueda;
        return View(await _proveedores.ListarAsync(busqueda));
    }

    [HttpGet]
    public IActionResult Crear() => View(new ProveedorFormViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProveedorFormViewModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        await _proveedores.CrearAsync(modelo);
        TempData["Exito"] = $"Proveedor {modelo.Nombre.Trim()} creado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var modelo = await _proveedores.ObtenerAsync(id);
        if (modelo is null)
            return NotFound();

        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(ProveedorFormViewModel modelo)
    {
        if (!ModelState.IsValid)
            return View(modelo);

        if (!await _proveedores.EditarAsync(modelo))
            return NotFound();

        TempData["Exito"] = $"Proveedor {modelo.Nombre.Trim()} actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var error = await _proveedores.EliminarAsync(id);
        if (error is null)
            TempData["Exito"] = "Proveedor eliminado.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Index));
    }
}
