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
    private readonly IComandaService _comandaService;

    public HomeController(IDashboardService dashboardService, IComandaService comandaService)
    {
        _dashboardService = dashboardService;
        _comandaService = comandaService;
    }

    public async Task<IActionResult> Index()
    {
        if (!User.IsInRole(DbSeeder.RolAdmin))
            return View("Cocina", await _comandaService.ObtenerPanelDelDiaAsync());

        return View(await _dashboardService.ObtenerResumenAsync());
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
