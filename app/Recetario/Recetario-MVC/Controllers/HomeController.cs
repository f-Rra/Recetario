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
        // El dashboard de Cocina (comandas del día) llega en la guía 11
        if (!User.IsInRole(DbSeeder.RolAdmin))
            return View("Cocina");

        return View(await _dashboardService.ObtenerResumenAsync());
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
