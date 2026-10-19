using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Infrastructure;
using RecetarioMVC.Controllers;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using UglyToad.PdfPig;

namespace RecetarioMVC.Tests;

// Lo mismo que HoraDeArgentinaTests, pero para lo que se le muestra al usuario: el filtro
// "hoy" del historial, el nombre del PDF que se descarga, el pie de los informes y la fecha
// que propone el formulario de precios. Todos usan las 23:30 de Argentina, cuando en UTC ya
// es el día siguiente.
public class HoraDeArgentinaEnPantallasTests
{
    // QuestPDF pide elegir la licencia antes de generar un PDF; la aplicación lo hace en Program.cs
    static HoraDeArgentinaEnPantallasTests() => QuestPDF.Settings.License = LicenseType.Community;

    // Son las 23:30 del 10 de marzo en Argentina; en UTC ya son las 02:30 del 11
    private static TimeProvider RelojDeLaNoche() => RelojDePrueba.EnArgentina(2026, 3, 10, 23, 30);

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);

        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var principal = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };

        context.AddRange(
            kg, principal,
            new Ingrediente { IdIngrediente = 1, Codigo = "ING001", Descripcion = "Harina 000", IdUnidad = 1, Unidad = kg, Deposito = Deposito.Bodega, StockActual = 10m },
            new Proveedor { IdProveedor = 1, Nombre = "Molinos del Sur" },
            new Receta { IdReceta = 1, Codigo = "REC001", Nombre = "Ñoquis", IdClasificacion = 1, Clasificacion = principal, PorcionesBase = 6, Activo = true },
            new ApplicationUser { Id = "u1", Nombre = "Cocina", Apellido = "Test" });

        context.IngredientesReceta.Add(
            new IngredienteReceta { IdReceta = 1, IdIngrediente = 1, CantNeta = 0.5m, Rendimiento = 80m, CantBruta = 0.625m, IdUnidad = 1 });
        context.PreciosIngrediente.Add(
            new PrecioIngrediente { IdPrecio = 1, IdIngrediente = 1, IdProveedor = 1, Precio = 1200.50m, FechaVigencia = new DateOnly(2026, 3, 1) });

        context.SaveChanges();
        return context;
    }

    // Una sesión mínima, en memoria: el controlador guarda ahí el PDF para descargarlo en el
    // pedido siguiente
    private sealed class SesionEnMemoria : ISession
    {
        private readonly Dictionary<string, byte[]> _datos = new();

        public bool IsAvailable => true;
        public string Id => "prueba";
        public IEnumerable<string> Keys => _datos.Keys;
        public void Clear() => _datos.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _datos.Remove(key);
        public void Set(string key, byte[] value) => _datos[key] = value;
        public bool TryGetValue(string key, [System.Diagnostics.CodeAnalysis.NotNullWhen(true)] out byte[]? value) => _datos.TryGetValue(key, out value);
    }

    [Fact]
    public async Task HistorialDeComandas_PorDefectoMuestraElDiaDeArgentina()
    {
        using var context = CrearContexto();
        var reloj = RelojDeLaNoche();
        var controlador = new ComandasController(new ComandaService(context, reloj), null!, reloj);

        await controlador.Index(desde: null, hasta: null, receta: null);

        Assert.Equal(new DateOnly(2026, 3, 10), controlador.ViewData["Desde"]);
        Assert.Equal(new DateOnly(2026, 3, 10), controlador.ViewData["Hasta"]);
    }

    [Fact]
    public void DescargarPdf_PoneLaHoraDeArgentinaEnElNombreDelArchivo()
    {
        var reloj = RelojDeLaNoche();
        var sesion = new SesionEnMemoria();
        sesion.Set("ComanderaPdfPendiente", [1, 2, 3]);
        var controlador = new ComandasController(null!, null!, reloj)
        {
            ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext { Session = sesion } }
        };

        var archivo = Assert.IsType<FileContentResult>(controlador.DescargarPdf());

        Assert.Equal("comanda-20260310-2330.pdf", archivo.FileDownloadName);
    }

    [Fact]
    public async Task PdfDeCosteo_ElPieDiceLaHoraDeArgentina()
    {
        using var context = CrearContexto();
        var reloj = RelojDeLaNoche();
        var costeos = new CosteoService(context, reloj);
        var idCosto = await costeos.RegistrarAsync(idReceta: 1, porciones: 6, usuarioId: "u1");
        var controlador = new CostosController(costeos, null!, null!, reloj);

        var archivo = Assert.IsType<FileContentResult>(await controlador.Pdf(idCosto!.Value));

        // Se lee el texto del PDF generado, como lo vería quien lo abre
        using var documento = PdfDocument.Open(archivo.FileContents);
        var texto = string.Join(" ", documento.GetPages().Select(p => p.Text));
        Assert.Contains("10/03/2026 23:30", texto);
    }

    [Fact]
    public async Task DetalleDeIngrediente_ElFormularioDePreciosProponeElDiaDeArgentina()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDeLaNoche());

        var detalle = await servicio.ObtenerDetalleAsync(1);

        Assert.Equal(new DateOnly(2026, 3, 10), detalle!.NuevoPrecio.FechaVigencia);
    }
}
