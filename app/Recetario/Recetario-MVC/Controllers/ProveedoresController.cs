using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecetarioMVC.Data;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

// Una sola pantalla de fichas: las altas y ediciones son modales (guía 29)
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
        return View(await ArmarPaginaAsync(busqueda));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(ProveedorFormViewModel modelo, string? busqueda)
    {
        if (!ModelState.IsValid)
        {
            var pagina = await ArmarPaginaAsync(busqueda);
            pagina.Nuevo = modelo;
            pagina.ModalAbierto = "modalCrear";
            return View(nameof(Index), pagina);
        }

        await _proveedores.CrearAsync(modelo);
        TempData["Exito"] = $"Proveedor {modelo.Nombre.Trim()} creado.";
        return RedirectToAction(nameof(Index), new { busqueda });
    }

    // El prefijo separa este formulario del de alta: los dos modales viven en
    // la misma página y, sin prefijo, un error de uno se pinta en el otro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        [Bind(Prefix = "Edicion")] ProveedorFormViewModel modelo, string? busqueda)
    {
        if (!ModelState.IsValid)
        {
            var pagina = await ArmarPaginaAsync(busqueda);
            pagina.Edicion = modelo;
            pagina.ModalAbierto = "modalEditar";
            return View(nameof(Index), pagina);
        }

        if (!await _proveedores.EditarAsync(modelo))
            return NotFound();

        TempData["Exito"] = $"Proveedor {modelo.Nombre.Trim()} actualizado.";
        return RedirectToAction(nameof(Index), new { busqueda });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, string? busqueda)
    {
        var error = await _proveedores.EliminarAsync(id);
        if (error is null)
            TempData["Exito"] = "Proveedor eliminado.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Index), new { busqueda });
    }

    private async Task<ProveedoresPaginaViewModel> ArmarPaginaAsync(string? busqueda)
    {
        return new ProveedoresPaginaViewModel
        {
            Busqueda = busqueda,
            Lista = await _proveedores.ListarAsync(busqueda)
        };
    }
}
