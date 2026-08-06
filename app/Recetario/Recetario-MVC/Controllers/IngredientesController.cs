using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

// Una sola pantalla: la lista, la ficha del elegido y sus precios. Las altas y
// ediciones son modales, así que todo vuelve siempre al Index (guía 28).
[Authorize(Roles = DbSeeder.RolAdmin)]
public class IngredientesController : Controller
{
    private readonly IIngredienteService _ingredientes;
    private readonly IPrecioIngredienteService _precios;
    private readonly UserManager<ApplicationUser> _userManager;

    public IngredientesController(
        IIngredienteService ingredientes,
        IPrecioIngredienteService precios,
        UserManager<ApplicationUser> userManager)
    {
        _ingredientes = ingredientes;
        _precios = precios;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index(string? busqueda, int? id)
    {
        return View(await ArmarPaginaAsync(busqueda, id));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Crear(IngredienteFormViewModel modelo, string? busqueda)
    {
        if (!ModelState.IsValid)
        {
            // Vuelve la pantalla completa con el modal abierto y los errores
            var pagina = await ArmarPaginaAsync(busqueda, null);
            pagina.Nuevo = modelo;
            pagina.ModalAbierto = "modalCrear";
            return View(nameof(Index), pagina);
        }

        await _ingredientes.CrearAsync(modelo, _userManager.GetUserId(User)!);
        TempData["Exito"] = $"Ingrediente {modelo.Descripcion.Trim()} creado.";
        return RedirectToAction(nameof(Index), new { busqueda });
    }

    // El prefijo separa este formulario del de alta: los dos modales viven en
    // la misma página y, sin prefijo, un error de uno se pinta en el otro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Editar(
        [Bind(Prefix = "Datos")] IngredienteFormViewModel modelo, string? busqueda)
    {
        if (!ModelState.IsValid)
        {
            var pagina = await ArmarPaginaAsync(busqueda, modelo.IdIngrediente);
            if (pagina.Seleccionado is not null)
                pagina.Seleccionado.Datos = modelo;
            pagina.ModalAbierto = "modalEditar";
            return View(nameof(Index), pagina);
        }

        if (!await _ingredientes.EditarAsync(modelo))
            return NotFound();

        TempData["Exito"] = $"Ingrediente {modelo.Descripcion.Trim()} actualizado.";
        return RedirectToAction(nameof(Index), new { busqueda, id = modelo.IdIngrediente });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Eliminar(int id, string? busqueda)
    {
        var error = await _ingredientes.EliminarAsync(id);
        if (error is null)
            TempData["Exito"] = "Ingrediente eliminado.";
        else
            TempData["Error"] = error;

        // Si se eliminó, la selección deja de existir y cae en el primero
        return RedirectToAction(nameof(Index), new { busqueda, id = error is null ? null : (int?)id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarPrecio(PrecioFormViewModel nuevoPrecio, string? busqueda)
    {
        if (!ModelState.IsValid)
        {
            var pagina = await ArmarPaginaAsync(busqueda, nuevoPrecio.IdIngrediente);
            if (pagina.Seleccionado is not null)
                pagina.Seleccionado.NuevoPrecio = nuevoPrecio;
            pagina.ModalAbierto = "modalPrecio";
            return View(nameof(Index), pagina);
        }

        if (!await _precios.AgregarAsync(nuevoPrecio))
            return NotFound();

        TempData["Exito"] = "Precio registrado.";
        return RedirectToAction(nameof(Index), new { busqueda, id = nuevoPrecio.IdIngrediente });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EliminarPrecio(int idPrecio, int idIngrediente, string? busqueda)
    {
        if (await _precios.EliminarAsync(idPrecio))
            TempData["Exito"] = "Precio eliminado.";
        else
            TempData["Error"] = "El precio no existe.";

        return RedirectToAction(nameof(Index), new { busqueda, id = idIngrediente });
    }

    // ================= Auxiliares =================

    /// <summary>
    /// La lista filtrada más la ficha del elegido. Si el id no está en la
    /// lista —porque se filtró o se acaba de eliminar— cae en el primero.
    /// </summary>
    private async Task<IngredientesPaginaViewModel> ArmarPaginaAsync(string? busqueda, int? id)
    {
        var lista = await _ingredientes.ListarAsync(busqueda);

        var elegido = id.HasValue && lista.Any(i => i.IdIngrediente == id.Value)
            ? id.Value
            : lista.FirstOrDefault()?.IdIngrediente;

        var pagina = new IngredientesPaginaViewModel
        {
            Busqueda = busqueda,
            Lista = lista,
            Nuevo = new IngredienteFormViewModel { Codigo = await _ingredientes.GenerarCodigoAsync() }
        };

        if (elegido.HasValue)
        {
            pagina.Seleccionado = await _ingredientes.ObtenerDetalleAsync(elegido.Value);

            // Los precios y su noción de "vigente" siguen viviendo en su servicio
            var historial = await _precios.ObtenerHistorialAsync(elegido.Value);
            if (pagina.Seleccionado is not null && historial is not null)
                pagina.Seleccionado.Precios = historial.Historial;
        }

        await CargarCombosAsync();
        return pagina;
    }

    private async Task CargarCombosAsync()
    {
        var unidades = await _ingredientes.ListarUnidadesAsync();
        ViewBag.Unidades = unidades
            .Select(u => new SelectListItem($"{u.Nombre} ({u.Abreviatura})", u.IdUnidad.ToString()))
            .ToList();

        var proveedores = await _precios.ListarProveedoresAsync();
        ViewBag.Proveedores = proveedores
            .Select(p => new SelectListItem(p.Nombre, p.IdProveedor.ToString()))
            .ToList();
    }
}
