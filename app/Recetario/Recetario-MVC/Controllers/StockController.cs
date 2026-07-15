using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

[Authorize(Roles = DbSeeder.RolAdmin)]
public class StockController : Controller
{
    private readonly IStockService _stock;
    private readonly IIngredienteService _ingredientes;
    private readonly UserManager<ApplicationUser> _userManager;

    public StockController(
        IStockService stock,
        IIngredienteService ingredientes,
        UserManager<ApplicationUser> userManager)
    {
        _stock = stock;
        _ingredientes = ingredientes;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? busqueda)
    {
        ViewData["Busqueda"] = busqueda;
        return View(await _ingredientes.ListarAsync(busqueda));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegistrarMovimiento(MovimientoFormViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = string.Join(" ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Index));
        }

        var error = await _stock.RegistrarMovimientoAsync(modelo, _userManager.GetUserId(User)!);
        if (error is null)
            TempData["Exito"] = "Movimiento registrado y stock actualizado.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Historial(int? ingrediente, TipoMovimiento? tipo, DateOnly? desde, DateOnly? hasta)
    {
        var modelo = new HistorialFiltrosViewModel
        {
            IdIngrediente = ingrediente,
            Tipo = tipo,
            Desde = desde,
            Hasta = hasta,
            Movimientos = await _stock.HistorialAsync(ingrediente, tipo, desde, hasta)
        };

        var lista = await _ingredientes.ListarAsync(null);
        ViewBag.Ingredientes = lista
            .Select(i => new SelectListItem(i.Descripcion, i.IdIngrediente.ToString(),
                i.IdIngrediente == ingrediente))
            .ToList();

        return View(modelo);
    }
}
