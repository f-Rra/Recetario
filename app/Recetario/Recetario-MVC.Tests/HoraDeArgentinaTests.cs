using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Tests;

// Las fechas y horas que se guardan tienen que ser las de Argentina, no las de la máquina
// donde corre la aplicación. Un servidor en la nube corre en UTC, tres horas adelantado: a
// las 23:30 de Argentina ya es el día siguiente. Todos los tests usan ese momento justo.
public class HoraDeArgentinaTests
{
    private const int Noquis = 1;   // base 6, lleva 0,625 kg de Harina
    private const int Harina = 1;   // stock 10, en bodega, a $1.200,50

    // Son las 23:30 del 10 de marzo en Argentina; en UTC ya son las 02:30 del 11
    private static TimeProvider RelojDeLaNoche() => RelojDePrueba.EnArgentina(2026, 3, 10, 23, 30);

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new ApplicationDbContext(opciones);

        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var principal = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };

        context.AddRange(
            kg, principal,
            new Ingrediente { IdIngrediente = Harina, Codigo = "ING001", Descripcion = "Harina 000", IdUnidad = 1, Unidad = kg, Deposito = Deposito.Bodega, StockActual = 10m },
            new Proveedor { IdProveedor = 1, Nombre = "Molinos del Sur" },
            new Receta { IdReceta = Noquis, Codigo = "REC001", Nombre = "Ñoquis", IdClasificacion = 1, Clasificacion = principal, PorcionesBase = 6, Activo = true },
            new Persona { IdPersona = 1, Nombre = "María", Apellido = "López", IdClasificacion = 1 },
            new ApplicationUser { Id = "u1", Nombre = "Cocina", Apellido = "Test" });

        context.IngredientesReceta.Add(
            new IngredienteReceta { IdReceta = Noquis, IdIngrediente = Harina, CantNeta = 0.5m, Rendimiento = 80m, CantBruta = 0.625m, IdUnidad = 1 });
        context.PreciosIngrediente.Add(
            new PrecioIngrediente { IdPrecio = 1, IdIngrediente = Harina, IdProveedor = 1, Precio = 1200.50m, FechaVigencia = new DateOnly(2026, 3, 1) });

        context.SaveChanges();
        return context;
    }

    private static CarritoComanda CarritoDeNoquis() => new()
    {
        Comensales = 6,
        Items =
        [
            new CarritoItem
            {
                IdReceta = Noquis,
                Nombre = "Ñoquis",
                Clasificacion = "Plato Principal",
                IdClasificacion = 1,
                PorcionesBase = 6
            }
        ]
    };

    [Fact]
    public async Task Generar_GuardaLaFechaDeArgentinaEnLaComandaYEnSusMovimientos()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context, RelojDeLaNoche());

        var resultado = await servicio.GenerarAsync(CarritoDeNoquis(), "u1");

        Assert.True(resultado.Ok);
        Assert.Equal(new DateOnly(2026, 3, 10), (await context.Comandas.SingleAsync()).Fecha);
        Assert.Equal(new DateTime(2026, 3, 10, 23, 30, 0), (await context.MovimientosStock.SingleAsync()).Fecha);
    }

    [Fact]
    public async Task RegistrarMovimiento_GuardaLaHoraDeArgentina()
    {
        using var context = CrearContexto();
        var servicio = new StockService(context, RelojDeLaNoche());

        await servicio.RegistrarMovimientoAsync(
            new MovimientoFormViewModel { IdIngrediente = Harina, Tipo = TipoMovimiento.Entrada, Cantidad = 5m }, "u1");

        Assert.Equal(new DateTime(2026, 3, 10, 23, 30, 0), (await context.MovimientosStock.SingleAsync()).Fecha);
    }

    [Fact]
    public async Task GuardarInventario_GuardaLaHoraDeArgentina()
    {
        using var context = CrearContexto();
        var servicio = new StockService(context, RelojDeLaNoche());

        // El conteo dice 7 kg y el sistema 10: genera un ajuste
        await servicio.GuardarInventarioAsync(
            Deposito.Bodega, [new InventarioItem { IdIngrediente = Harina, Contado = 7m }], "u1");

        Assert.Equal(new DateTime(2026, 3, 10, 23, 30, 0), (await context.MovimientosStock.SingleAsync()).Fecha);
    }

    [Fact]
    public async Task CrearIngrediente_ConStockInicial_GuardaElMovimientoConLaHoraDeArgentina()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDeLaNoche());

        await servicio.CrearAsync(new IngredienteFormViewModel
        {
            Descripcion = "Sal fina",
            IdUnidad = 1,
            Deposito = Deposito.Bodega,
            StockActual = 3m,
            StockMinimo = 1m
        }, "u1");

        var movimiento = await context.MovimientosStock.SingleAsync(m => m.Observaciones == "Stock inicial");
        Assert.Equal(new DateTime(2026, 3, 10, 23, 30, 0), movimiento.Fecha);
    }

    [Fact]
    public async Task RegistrarCosteo_GuardaLaFechaDeArgentina()
    {
        using var context = CrearContexto();
        var servicio = new CosteoService(context, RelojDeLaNoche());

        var idCosto = await servicio.RegistrarAsync(Noquis, 6, "u1");

        Assert.NotNull(idCosto);
        Assert.Equal(new DateOnly(2026, 3, 10), (await context.Costos.SingleAsync()).Fecha);
    }

    [Fact]
    public async Task Resumen_CuentaLasPorcionesDelMesDeArgentinaAunqueEnUtcYaSeaElSiguiente()
    {
        // Es el último día de julio a las 23:30 en Argentina, pero en UTC ya son las 02:30 del
        // 1 de agosto: una comanda del 31 de julio tiene que seguir contando en el mes
        using var context = CrearContexto();
        context.Comandas.Add(new Comanda
        {
            IdReceta = Noquis, Fecha = new DateOnly(2026, 7, 31), Porciones = 4, UsuarioId = "u1", IdPersona = 1
        });
        await context.SaveChangesAsync();
        var servicio = new DashboardService(context, RelojDePrueba.EnArgentina(2026, 7, 31, 23, 30));

        var resumen = await servicio.ObtenerResumenAsync();

        Assert.Equal(4, resumen.PorcionesDelMes);
    }
}
