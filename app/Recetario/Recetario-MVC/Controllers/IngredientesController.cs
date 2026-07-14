using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Data;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

[Authorize(Roles = DbSeeder.RolAdmin)]
public class IngredientesController : Controller
{
    private readonly IIngredienteService _ingredientes;
    private readonly IPrecioIngredienteService _precios;

    public IngredientesController(IIngredienteService ingredientes, IPrecioIngredienteService precios)
    {
        _ingredientes = ingredientes;
        _precios = precios;
    }

    public async Task<IActionResult> Index(string? busqueda)
    {
        ViewData["Busqueda"] = busqueda;
        return View(await _ingredientes.ListarAsync(busqueda));
    }

    [HttpGet]
    public async Task<IActionResult> Crear()
    {
        var modelo = new IngredienteFormViewModel
        {
            Codigo = await _ingredientes.GenerarCodigoAsync()
        };
        await CargarUnidadesAsync();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(IngredienteFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarUnidadesAsync();
            return View(modelo);
        }

        await _ingredientes.CrearAsync(modelo);
        TempData["Exito"] = $"Ingrediente {modelo.Descripcion.Trim()} creado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Editar(int id)
    {
        var modelo = await _ingredientes.ObtenerAsync(id);
        if (modelo is null)
            return NotFound();

        await CargarUnidadesAsync();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(IngredienteFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarUnidadesAsync();
            return View(modelo);
        }

        if (!await _ingredientes.EditarAsync(modelo))
            return NotFound();

        TempData["Exito"] = $"Ingrediente {modelo.Descripcion.Trim()} actualizado.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id)
    {
        var error = await _ingredientes.EliminarAsync(id);
        if (error is null)
            TempData["Exito"] = "Ingrediente eliminado.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Precios(int id)
    {
        var modelo = await _precios.ObtenerHistorialAsync(id);
        if (modelo is null)
            return NotFound();

        await CargarProveedoresAsync();
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarPrecio(PrecioFormViewModel nuevoPrecio)
    {
        // El prefijo del binder es "NuevoPrecio" porque el form vive dentro
        // de IngredientePreciosViewModel
        if (!ModelState.IsValid)
        {
            var modelo = await _precios.ObtenerHistorialAsync(nuevoPrecio.IdIngrediente);
            if (modelo is null)
                return NotFound();

            modelo.NuevoPrecio = nuevoPrecio;
            await CargarProveedoresAsync();
            return View(nameof(Precios), modelo);
        }

        if (!await _precios.AgregarAsync(nuevoPrecio))
            return NotFound();

        TempData["Exito"] = "Precio registrado.";
        return RedirectToAction(nameof(Precios), new { id = nuevoPrecio.IdIngrediente });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarPrecio(int idPrecio, int idIngrediente)
    {
        if (await _precios.EliminarAsync(idPrecio))
            TempData["Exito"] = "Precio eliminado.";
        else
            TempData["Error"] = "El precio no existe.";

        return RedirectToAction(nameof(Precios), new { id = idIngrediente });
    }

    private async Task CargarProveedoresAsync()
    {
        var proveedores = await _precios.ListarProveedoresAsync();
        ViewBag.Proveedores = proveedores
            .Select(p => new SelectListItem(p.Nombre, p.IdProveedor.ToString()))
            .ToList();
    }

    private async Task CargarUnidadesAsync()
    {
        var unidades = await _ingredientes.ListarUnidadesAsync();
        ViewBag.Unidades = unidades
            .Select(u => new SelectListItem($"{u.Nombre} ({u.Abreviatura})", u.IdUnidad.ToString()))
            .ToList();
    }
}
