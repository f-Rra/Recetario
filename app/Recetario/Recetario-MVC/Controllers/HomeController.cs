using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly IDashboardService _dashboardService;

    public HomeController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    public async Task<IActionResult> Index()
    {
        // Para Cocina la pantalla de trabajo es la comandera
        if (!User.IsInRole(DbSeeder.RolAdmin))
            return RedirectToAction("Comandera", "Comandas");

        return View(await _dashboardService.ObtenerResumenAsync());
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(ErrorViewModel.Para(
            StatusCodes.Status500InternalServerError,
            Activity.Current?.Id ?? HttpContext.TraceIdentifier));
    }

    /// <summary>
    /// Página para las respuestas sin contenido (un 404 por una dirección que no existe o
    /// por un registro que no está, un 400 por un formulario vencido). Sin esto el
    /// navegador mostraba una hoja en blanco.
    /// </summary>
    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Estado(int id)
    {
        return View(nameof(Error), ErrorViewModel.Para(id));
    }
}
