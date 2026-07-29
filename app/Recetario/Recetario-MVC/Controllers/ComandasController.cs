using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.Services.Pdf;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Controllers;

[Authorize] // Cocina y Admin
public class ComandasController : Controller
{
    private const string ClavePdfPendiente = "ComanderaPdfPendiente";

    private readonly IComandaService _comandas;
    private readonly UserManager<ApplicationUser> _userManager;

    public ComandasController(IComandaService comandas, UserManager<ApplicationUser> userManager)
    {
        _comandas = comandas;
        _userManager = userManager;
    }

    // ================= Comandera =================

    [HttpGet]
    public async Task<IActionResult> Comandera(string? busqueda, int? clasificacion)
    {
        var carrito = CarritoSesion.Obtener(HttpContext.Session);

        var modelo = new ComanderaViewModel
        {
            Busqueda = busqueda,
            IdClasificacion = clasificacion,
            Catalogo = await _comandas.ListarCatalogoAsync(busqueda, clasificacion),
            Carrito = carrito,
            Responsables = await _comandas.ResolverResponsablesAsync(carrito)
        };

        foreach (var item in carrito.Items)
            modelo.IngredientesPorItem[item.IdReceta] =
                await _comandas.ListarIngredientesDeRecetaAsync(item.IdReceta);

        var enCarrito = carrito.Items.Select(i => i.IdReceta).ToHashSet();
        foreach (var receta in modelo.Catalogo)
            receta.EnCarrito = enCarrito.Contains(receta.IdReceta);

        await CargarCombosAsync(clasificacion);
        ViewBag.HayPdfPendiente = HttpContext.Session.Get(ClavePdfPendiente) is not null;
        return View(modelo);
    }

    /// <summary>Previsualización de ingredientes al seleccionar una receta del catálogo.</summary>
    [HttpGet]
    public async Task<IActionResult> IngredientesReceta(int id, int comensales)
    {
        var modelo = await _comandas.ObtenerIngredientesPreviewAsync(id, comensales);
        if (modelo is null)
            return NotFound();

        return Json(modelo);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AgregarAlCarrito(int comensales, List<int>? seleccion)
    {
        var carrito = CarritoSesion.Obtener(HttpContext.Session);
        carrito.Comensales = comensales > 0 ? comensales : carrito.Comensales;

        if (seleccion is null || seleccion.Count == 0)
        {
            TempData["Error"] = "Tildá al menos una receta del catálogo.";
            CarritoSesion.Guardar(HttpContext.Session, carrito);
            return RedirectToAction(nameof(Comandera));
        }

        var agregadas = 0;
        foreach (var idReceta in seleccion.Distinct())
        {
            if (carrito.Items.Any(i => i.IdReceta == idReceta))
                continue;

            var item = await _comandas.ObtenerParaCarritoAsync(idReceta);
            if (item is null)
                continue;

            carrito.Items.Add(item);
            agregadas++;
        }

        CarritoSesion.Guardar(HttpContext.Session, carrito);
        TempData[agregadas == 0 ? "Error" : "Exito"] = agregadas switch
        {
            0 => "Las recetas que seleccionaste ya estaban en la comanda.",
            1 => "Receta agregada a la comanda.",
            _ => $"{agregadas} recetas agregadas a la comanda."
        };
        return RedirectToAction(nameof(Comandera));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ActualizarComensales(int comensales)
    {
        var carrito = CarritoSesion.Obtener(HttpContext.Session);
        if (comensales <= 0)
            TempData["Error"] = "Los comensales deben ser mayores a cero.";
        else
            carrito.Comensales = comensales;

        CarritoSesion.Guardar(HttpContext.Session, carrito);
        return RedirectToAction(nameof(Comandera));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult QuitarDelCarrito(int idReceta)
    {
        var carrito = CarritoSesion.Obtener(HttpContext.Session);
        carrito.Items.RemoveAll(i => i.IdReceta == idReceta);
        CarritoSesion.Guardar(HttpContext.Session, carrito);
        TempData["Exito"] = "Receta quitada de la comanda.";
        return RedirectToAction(nameof(Comandera));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GuardarModificacion(ModificacionComanderaForm form)
    {
        var carrito = CarritoSesion.Obtener(HttpContext.Session);
        var item = carrito.Items.FirstOrDefault(i => i.IdReceta == form.IdReceta);

        if (item is null || form.Tipo is null)
        {
            TempData["Error"] = "No se pudo registrar la modificación.";
            return RedirectToAction(nameof(Comandera));
        }

        var error = await ValidarModificacionAsync(form, item);
        if (error is not null)
        {
            TempData["Error"] = error;
            return RedirectToAction(nameof(Comandera));
        }

        var ingredientesReceta = await _comandas.ListarIngredientesDeRecetaAsync(form.IdReceta);
        var todos = await _comandas.ListarIngredientesAsync();

        var original = ingredientesReceta.FirstOrDefault(i => i.IdIngrediente == form.IdIngredienteOriginal);
        var reemplazo = todos.FirstOrDefault(i => i.IdIngrediente == form.IdIngredienteReemplazo);

        // Se puede sustituir por un ingrediente medido en otra unidad siempre que
        // sean equivalentes (kg con g, L con ml): la cantidad se convierte sola
        if (form.Tipo == TipoModificacion.Sustitucion && original is not null && reemplazo is not null &&
            !Unidades.SonEquivalentes(original.Unidad, reemplazo.Unidad.Abreviatura))
        {
            TempData["Error"] =
                $"No se puede reemplazar {original.Ingrediente} ({original.Unidad}) por " +
                $"{reemplazo.Descripcion} ({reemplazo.Unidad.Abreviatura}): son unidades distintas.";
            return RedirectToAction(nameof(Comandera));
        }

        item.Modificaciones.Add(new ModificacionCarrito
        {
            Tipo = form.Tipo.Value,
            IdIngredienteOriginal = form.IdIngredienteOriginal,
            NombreOriginal = original?.Ingrediente,
            IdIngredienteReemplazo = form.IdIngredienteReemplazo,
            NombreReemplazo = reemplazo?.Descripcion,
            // Solo la adición trae cantidad; el resto la hereda de la receta
            Cantidad = form.Tipo == TipoModificacion.Adicion ? form.Cantidad : null,
            Unidad = reemplazo?.Unidad.Abreviatura ?? original?.Unidad
        });

        CarritoSesion.Guardar(HttpContext.Session, carrito);
        TempData["Exito"] = "Modificación agregada a la receta.";
        return RedirectToAction(nameof(Comandera));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult QuitarModificacion(int idReceta, int indice)
    {
        var carrito = CarritoSesion.Obtener(HttpContext.Session);
        var item = carrito.Items.FirstOrDefault(i => i.IdReceta == idReceta);

        if (item is not null && indice >= 0 && indice < item.Modificaciones.Count)
        {
            item.Modificaciones.RemoveAt(indice);
            CarritoSesion.Guardar(HttpContext.Session, carrito);
            TempData["Exito"] = "Modificación quitada.";
        }

        return RedirectToAction(nameof(Comandera));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Generar()
    {
        var carrito = CarritoSesion.Obtener(HttpContext.Session);
        var usuarioId = _userManager.GetUserId(User)!;
        var resultado = await _comandas.GenerarAsync(carrito, usuarioId);

        if (resultado.Faltantes.Count > 0)
        {
            TempData["Error"] = "No hay stock suficiente: " + string.Join(" ", resultado.Faltantes);
            return RedirectToAction(nameof(Comandera));
        }

        if (!resultado.Ok)
        {
            TempData["Error"] = resultado.Error;
            return RedirectToAction(nameof(Comandera));
        }

        var usuario = await _userManager.GetUserAsync(User);
        var pdf = ComandaPdf.Generar(new ComandaPdfViewModel
        {
            Fecha = DateOnly.FromDateTime(DateTime.Today),
            Comensales = carrito.Comensales,
            Usuario = usuario?.NombreCompleto ?? string.Empty,
            Secciones = resultado.Secciones
        });

        // El PDF queda para descargar en el siguiente request: así la pantalla
        // vuelve con el carrito vacío y el aviso de éxito
        HttpContext.Session.Set(ClavePdfPendiente, pdf);
        CarritoSesion.Vaciar(HttpContext.Session, carrito.Comensales);

        TempData["Exito"] = "Comanda generada. El PDF se está descargando.";
        return RedirectToAction(nameof(Comandera));
    }

    [HttpGet]
    public IActionResult DescargarPdf()
    {
        var pdf = HttpContext.Session.Get(ClavePdfPendiente);
        if (pdf is null)
            return NotFound();

        HttpContext.Session.Remove(ClavePdfPendiente);
        return File(pdf, "application/pdf", $"comanda-{DateTime.Now:yyyyMMdd-HHmm}.pdf");
    }

    // ================= Consulta =================

    public async Task<IActionResult> Index(DateOnly? desde, DateOnly? hasta, int? receta)
    {
        // Por defecto, las del día
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        desde ??= hoy;
        hasta ??= hoy;

        ViewData["Desde"] = desde;
        ViewData["Hasta"] = hasta;
        ViewData["Receta"] = receta;

        var recetas = await _comandas.ListarCatalogoAsync(null, null);
        ViewBag.Recetas = recetas
            .Select(r => new SelectListItem(r.Nombre, r.IdReceta.ToString(), r.IdReceta == receta))
            .ToList();

        return View(await _comandas.ListarAsync(desde, hasta, receta));
    }

    [HttpGet]
    public async Task<IActionResult> Detalle(int id)
    {
        var modelo = await _comandas.ObtenerDetalleAsync(id);
        if (modelo is null)
            return NotFound();

        return View(modelo);
    }

    /// <summary>Reimpresión de una comanda ya registrada.</summary>
    [HttpGet]
    public async Task<IActionResult> Pdf(int id)
    {
        var comanda = await _comandas.ObtenerDetalleAsync(id);
        if (comanda is null)
            return NotFound();

        var pdf = ComandaPdf.Generar(new ComandaPdfViewModel
        {
            Fecha = comanda.Fecha,
            Comensales = comanda.Porciones,
            Usuario = comanda.Usuario,
            Secciones =
            [
                new SeccionComandaPdf
                {
                    Receta = comanda.Receta,
                    Codigo = comanda.Codigo,
                    Sector = comanda.Clasificacion,
                    Responsable = comanda.Responsable,
                    Ingredientes = comanda.Ingredientes,
                    Modificaciones = comanda.Modificaciones.Select(m => m.Descripcion).ToList(),
                    Pasos = comanda.Pasos
                }
            ]
        });

        return File(pdf, "application/pdf", $"comanda-{comanda.IdComanda}.pdf");
    }

    // ================= Auxiliares =================

    private async Task<string?> ValidarModificacionAsync(ModificacionComanderaForm form, CarritoItem item)
    {
        return form.Tipo switch
        {
            TipoModificacion.Sustitucion when form.IdIngredienteOriginal is null || form.IdIngredienteReemplazo is null
                => "La sustitución necesita el ingrediente a reemplazar y el nuevo.",
            TipoModificacion.Adicion when form.IdIngredienteReemplazo is null
                => "Elegí el ingrediente a agregar.",
            TipoModificacion.Adicion when form.Cantidad is null or <= 0
                => "Indicá la cantidad a agregar para toda la comanda.",
            TipoModificacion.Eliminacion when form.IdIngredienteOriginal is null
                => "Elegí el ingrediente a quitar.",
            _ => await Task.FromResult<string?>(null)
        };
    }

    private async Task CargarCombosAsync(int? clasificacionSeleccionada)
    {
        var clasificaciones = await _comandas.ListarClasificacionesAsync();
        ViewBag.Clasificaciones = clasificaciones
            .Select(c => new SelectListItem(c.Nombre, c.IdClasificacion.ToString(),
                c.IdClasificacion == clasificacionSeleccionada))
            .ToList();

        var ingredientes = await _comandas.ListarIngredientesAsync();
        ViewBag.Ingredientes = ingredientes
            .Select(i => new SelectListItem($"{i.Descripcion} ({i.Unidad.Abreviatura})", i.IdIngrediente.ToString()))
            .ToList();

        // El modal filtra los reemplazos por unidad equivalente sin ir al servidor
        ViewBag.IngredientesJson = JsonSerializer.Serialize(ingredientes
            .Select(i => new { id = i.IdIngrediente, nombre = i.Descripcion, unidad = i.Unidad.Abreviatura }));
    }
}
