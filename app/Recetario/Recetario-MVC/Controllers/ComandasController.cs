using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

[Authorize] // Cocina y Admin
public class ComandasController : Controller
{
    private readonly IComandaService _comandas;
    private readonly UserManager<ApplicationUser> _userManager;

    public ComandasController(IComandaService comandas, UserManager<ApplicationUser> userManager)
    {
        _comandas = comandas;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(DateOnly? fecha)
    {
        var dia = fecha ?? DateOnly.FromDateTime(DateTime.Today);
        ViewData["Fecha"] = dia;
        return View(await _comandas.ListarPorFechaAsync(dia));
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var modelo = await _comandas.ObtenerDetalleAsync(id);
        if (modelo is null)
            return NotFound();

        await CargarIngredientesAsync();
        return View(modelo);
    }

    [HttpGet]
    public async Task<IActionResult> Registrar()
    {
        await CargarCombosRegistroAsync();
        return View(new RegistrarComandaViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Registrar(RegistrarComandaViewModel modelo)
    {
        if (!ModelState.IsValid)
        {
            await CargarCombosRegistroAsync();
            return View(modelo);
        }

        var usuarioId = _userManager.GetUserId(User)!;
        var (idComanda, error) = await _comandas.RegistrarAsync(
            modelo.IdReceta!.Value, modelo.Porciones!.Value, usuarioId, modelo.IdPersona);

        if (error is not null)
        {
            ModelState.AddModelError(string.Empty, error);
            await CargarCombosRegistroAsync();
            return View(modelo);
        }

        TempData["Exito"] = $"Comanda #{idComanda} registrada. El stock ya fue descontado.";
        return RedirectToAction(nameof(Detalle), new { id = idComanda });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarModificacion(ModificacionFormViewModel nuevaModificacion)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = string.Join(" ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Detalle), new { id = nuevaModificacion.IdComanda });
        }

        var usuarioId = _userManager.GetUserId(User)!;
        var error = await _comandas.AgregarModificacionAsync(nuevaModificacion, usuarioId);

        if (error is null)
            TempData["Exito"] = "Modificación registrada con su ajuste de stock.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Detalle), new { id = nuevaModificacion.IdComanda });
    }

    [HttpGet]
    public async Task<IActionResult> Pdf(int id)
    {
        var comanda = await _comandas.ObtenerDetalleAsync(id);
        if (comanda is null)
            return NotFound();

        var pdf = Services.Pdf.ComandaPdf.Generar(comanda);
        return File(pdf, "application/pdf", $"comanda-{comanda.IdComanda}.pdf");
    }

    private async Task CargarCombosRegistroAsync()
    {
        var recetas = await _comandas.ListarRecetasActivasAsync();
        ViewBag.Recetas = recetas
            .Select(r => new SelectListItem($"{r.Nombre} ({r.Codigo})", r.IdReceta.ToString()))
            .ToList();

        var responsables = await _comandas.ListarResponsablesAsync();
        ViewBag.Responsables = responsables
            .Select(p => new SelectListItem(
                $"{p.Apellido}, {p.Nombre}" + (p.Clasificacion is null ? "" : $" — {p.Clasificacion.Nombre}"),
                p.IdPersona.ToString()))
            .ToList();
        ViewBag.HayResponsables = responsables.Count > 0;
    }

    private async Task CargarIngredientesAsync()
    {
        var ingredientes = await _comandas.ListarIngredientesAsync();
        ViewBag.Ingredientes = ingredientes
            .Select(i => new SelectListItem($"{i.Descripcion} ({i.Unidad.Abreviatura})", i.IdIngrediente.ToString()))
            .ToList();
    }
}
