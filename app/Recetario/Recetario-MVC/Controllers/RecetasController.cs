using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Data;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

// Index y Detalle son de consulta (Cocina incluida); el resto exige Admin
[Authorize]
public class RecetasController : Controller
{
    private readonly IRecetaService _recetas;

    public RecetasController(IRecetaService recetas)
    {
        _recetas = recetas;
    }

    public async Task<IActionResult> Index(string? busqueda, int? clasificacion)
    {
        return View(await ArmarPaginaAsync(busqueda, clasificacion));
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var modelo = await _recetas.ObtenerDetalleAsync(id);
        if (modelo is null)
            return NotFound();

        return View(modelo);
    }

    // El alta abre en modal desde el listado, pero crear una receta es solo el
    // primer paso: al guardar se sigue a la edición, que es donde está el
    // trabajo real (ingredientes y procedimiento)
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> Crear(
        [Bind(Prefix = "Nueva")] RecetaFormViewModel modelo, string? busqueda, int? clasificacion)
    {
        if (!ModelState.IsValid)
        {
            var pagina = await ArmarPaginaAsync(busqueda, clasificacion);
            pagina.Nueva = modelo;
            pagina.ModalAbierto = "modalCrear";
            return View(nameof(Index), pagina);
        }

        var id = await _recetas.CrearAsync(modelo);
        TempData["Exito"] = $"Receta {modelo.Nombre.Trim()} creada. Ahora cargá los ingredientes y el procedimiento.";
        return RedirectToAction(nameof(Editar), new { id });
    }

    [HttpGet]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> Editar(int id)
    {
        var modelo = await _recetas.ObtenerDetalleAsync(id);
        if (modelo is null)
            return NotFound();

        await CargarCombosEdicionAsync(id);
        return View(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> EditarDatos(RecetaFormViewModel datos)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Revisá los datos de la receta: " + string.Join(" ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Editar), new { id = datos.IdReceta });
        }

        if (!await _recetas.EditarAsync(datos))
            return NotFound();

        TempData["Exito"] = "Datos de la receta actualizados.";
        return RedirectToAction(nameof(Editar), new { id = datos.IdReceta });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> AgregarIngrediente(IngredienteRecetaFormViewModel nuevoIngrediente)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = string.Join(" ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Editar), new { id = nuevoIngrediente.IdReceta });
        }

        var error = await _recetas.AgregarIngredienteAsync(nuevoIngrediente);
        if (error is null)
            TempData["Exito"] = "Ingrediente agregado.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Editar), new { id = nuevoIngrediente.IdReceta });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> QuitarIngrediente(int idReceta, int idIngrediente)
    {
        if (await _recetas.QuitarIngredienteAsync(idReceta, idIngrediente))
            TempData["Exito"] = "Ingrediente quitado.";
        else
            TempData["Error"] = "El ingrediente no está en la receta.";

        return RedirectToAction(nameof(Editar), new { id = idReceta });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> AgregarPaso(PasoFormViewModel nuevoPaso)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = string.Join(" ",
                ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return RedirectToAction(nameof(Editar), new { id = nuevoPaso.IdReceta });
        }

        if (!await _recetas.AgregarPasoAsync(nuevoPaso))
            return NotFound();

        TempData["Exito"] = "Paso agregado.";
        return RedirectToAction(nameof(Editar), new { id = nuevoPaso.IdReceta });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> QuitarPaso(int idProcedimiento, int idReceta)
    {
        if (await _recetas.QuitarPasoAsync(idProcedimiento, idReceta))
            TempData["Exito"] = "Paso quitado.";
        else
            TempData["Error"] = "El paso no existe.";

        return RedirectToAction(nameof(Editar), new { id = idReceta });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = DbSeeder.RolAdmin)]
    public async Task<IActionResult> Eliminar(int id)
    {
        var error = await _recetas.EliminarAsync(id);
        if (error is null)
            TempData["Exito"] = "Receta eliminada.";
        else
            TempData["Error"] = error;

        return RedirectToAction(nameof(Index));
    }

    private async Task<RecetasPaginaViewModel> ArmarPaginaAsync(string? busqueda, int? clasificacion)
    {
        await CargarClasificacionesAsync(clasificacion);

        return new RecetasPaginaViewModel
        {
            Busqueda = busqueda,
            IdClasificacion = clasificacion,
            Lista = await _recetas.ListarAsync(busqueda, clasificacion),
            // El modal de alta ya trae el código que le va a tocar
            Nueva = new RecetaFormViewModel { Codigo = await _recetas.GenerarCodigoAsync() }
        };
    }

    private async Task CargarClasificacionesAsync(int? seleccionada)
    {
        var clasificaciones = await _recetas.ListarClasificacionesAsync();
        ViewBag.Clasificaciones = clasificaciones
            .Select(c => new SelectListItem(c.Nombre, c.IdClasificacion.ToString(),
                c.IdClasificacion == seleccionada))
            .ToList();
    }

    private async Task CargarCombosEdicionAsync(int idReceta)
    {
        await CargarClasificacionesAsync(null);
        var ingredientes = await _recetas.ListarIngredientesAsync();
        ViewBag.Ingredientes = ingredientes
            .Select(i => new SelectListItem($"{i.Descripcion} ({i.Unidad.Abreviatura})", i.IdIngrediente.ToString()))
            .ToList();
    }
}
