using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Tests;

public class DashboardServiceTests
{
    private static readonly DateOnly Hoy = DateOnly.FromDateTime(DateTime.Today);

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);

        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var clasificacion = new Clasificacion { IdClasificacion = 1, Nombre = "Entrada" };

        context.AddRange(
            kg, clasificacion,
            new ApplicationUser { Id = "u1", Nombre = "Admin", Apellido = "Test" },
            new Persona { IdPersona = 1, Nombre = "Juan", Apellido = "Pérez", IdClasificacion = 1 },
            new Proveedor { IdProveedor = 1, Nombre = "Proveedor Uno" },
            // 10 kg × $100 = $1.000
            new Ingrediente { IdIngrediente = 1, Codigo = "ING001", Descripcion = "Harina", IdUnidad = 1, Unidad = kg, Deposito = Deposito.Bodega, StockActual = 10m, StockMinimo = 4m },
            // 2 kg × $50 = $100, y está bajo el mínimo
            new Ingrediente { IdIngrediente = 2, Codigo = "ING002", Descripcion = "Lechuga", IdUnidad = 1, Unidad = kg, Deposito = Deposito.Camara, StockActual = 2m, StockMinimo = 5m },
            // Sin precio: no se puede valorizar
            new Ingrediente { IdIngrediente = 3, Codigo = "ING003", Descripcion = "Sal", IdUnidad = 1, Unidad = kg, Deposito = Deposito.Bodega, StockActual = 8m, StockMinimo = 1m },
            new Receta { IdReceta = 1, Codigo = "REC001", Nombre = "Sopa", IdClasificacion = 1, Clasificacion = clasificacion, PorcionesBase = 10, Activo = true });

        context.PreciosIngrediente.AddRange(
            new PrecioIngrediente { IdPrecio = 1, IdIngrediente = 1, IdProveedor = 1, Precio = 80m, FechaVigencia = Hoy.AddMonths(-2) },
            // Más reciente: es el que manda
            new PrecioIngrediente { IdPrecio = 2, IdIngrediente = 1, IdProveedor = 1, Precio = 100m, FechaVigencia = Hoy },
            new PrecioIngrediente { IdPrecio = 3, IdIngrediente = 2, IdProveedor = 1, Precio = 50m, FechaVigencia = Hoy });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task Resumen_ValorizaElInventarioAlPrecioVigente()
    {
        using var context = CrearContexto();
        var servicio = new DashboardService(context);

        var resumen = await servicio.ObtenerResumenAsync();

        // 10 × $100 + 2 × $50 = $1.100. La sal no entra: no tiene precio
        Assert.Equal(1100m, resumen.ValorInventario);
        Assert.Equal(2, resumen.IngredientesValorizados);
        Assert.Equal(3, resumen.TotalIngredientes);
    }

    [Fact]
    public async Task Resumen_CuentaLosIngredientesPorDeposito()
    {
        using var context = CrearContexto();
        var servicio = new DashboardService(context);

        var resumen = await servicio.ObtenerResumenAsync();

        Assert.Equal(2, resumen.IngredientesBodega);
        Assert.Equal(1, resumen.IngredientesCamara);
    }

    [Fact]
    public async Task Resumen_MarcaLasRecetasIncompletas()
    {
        using var context = CrearContexto();
        var servicio = new DashboardService(context);

        // La única receta no tiene ingredientes ni pasos
        var sinContenido = await servicio.ObtenerResumenAsync();
        Assert.Equal(1, sinContenido.RecetasActivas);
        Assert.Equal(1, sinContenido.RecetasIncompletas);

        context.IngredientesReceta.Add(new IngredienteReceta
        {
            IdReceta = 1, IdIngrediente = 1, CantNeta = 1m, Rendimiento = 100m, CantBruta = 1m, IdUnidad = 1
        });
        context.Procedimientos.Add(new Procedimiento
        {
            IdProcedimiento = 1, IdReceta = 1, NroPaso = 1, Descripcion = "Hervir."
        });
        context.SaveChanges();

        var completa = await servicio.ObtenerResumenAsync();
        Assert.Equal(0, completa.RecetasIncompletas);
    }

    [Fact]
    public async Task Resumen_SumaLasPorcionesDelMesYComparaConElAnterior()
    {
        using var context = CrearContexto();
        var mesAnterior = new DateOnly(Hoy.Year, Hoy.Month, 1).AddDays(-1);
        context.Comandas.AddRange(
            new Comanda { IdComanda = 1, IdReceta = 1, IdPersona = 1, UsuarioId = "u1", Fecha = Hoy, Porciones = 120 },
            new Comanda { IdComanda = 2, IdReceta = 1, IdPersona = 1, UsuarioId = "u1", Fecha = Hoy, Porciones = 80 },
            new Comanda { IdComanda = 3, IdReceta = 1, IdPersona = 1, UsuarioId = "u1", Fecha = mesAnterior, Porciones = 100 });
        context.SaveChanges();
        var servicio = new DashboardService(context);

        var resumen = await servicio.ObtenerResumenAsync();

        Assert.Equal(200, resumen.PorcionesDelMes);
        Assert.Equal(100, resumen.PorcionesMesAnterior);
        Assert.Equal(100m, resumen.VariacionPorciones); // duplicó la producción
    }

    [Fact]
    public async Task Resumen_SinProduccionElMesAnterior_NoInventaUnaVariacion()
    {
        using var context = CrearContexto();
        context.Comandas.Add(new Comanda
        {
            IdComanda = 1, IdReceta = 1, IdPersona = 1, UsuarioId = "u1", Fecha = Hoy, Porciones = 50
        });
        context.SaveChanges();
        var servicio = new DashboardService(context);

        var resumen = await servicio.ObtenerResumenAsync();

        Assert.Equal(50, resumen.PorcionesDelMes);
        Assert.Null(resumen.VariacionPorciones);
    }

    [Fact]
    public async Task Actividad_ResumeLaComandaYNoRepiteSusConsumos()
    {
        using var context = CrearContexto();
        context.Comandas.Add(new Comanda
        {
            IdComanda = 7, IdReceta = 1, IdPersona = 1, UsuarioId = "u1", Fecha = Hoy, Porciones = 50
        });
        context.MovimientosStock.AddRange(
            // Los consumos de la comanda no van al feed: ya está la comanda
            new MovimientoStock { IdMovimiento = 1, IdIngrediente = 1, Tipo = TipoMovimiento.Salida, Cantidad = 5m, IdUnidad = 1, Fecha = DateTime.Now, UsuarioId = "u1", Observaciones = "Consumo comanda #7" },
            new MovimientoStock { IdMovimiento = 2, IdIngrediente = 2, Tipo = TipoMovimiento.Salida, Cantidad = 1m, IdUnidad = 1, Fecha = DateTime.Now, UsuarioId = "u1", Observaciones = "Consumo comanda #7" },
            // Este sí: es una reposición manual
            new MovimientoStock { IdMovimiento = 3, IdIngrediente = 1, Tipo = TipoMovimiento.Entrada, Cantidad = 20m, IdUnidad = 1, Fecha = DateTime.Now, UsuarioId = "u1", Observaciones = "Compra semanal" });
        context.SaveChanges();
        var servicio = new DashboardService(context);

        var resumen = await servicio.ObtenerResumenAsync();

        Assert.Equal(2, resumen.Actividad.Count);
        Assert.Contains(resumen.Actividad, a => a.Tipo == "comanda" && a.Titulo.Contains("Sopa"));
        Assert.Contains(resumen.Actividad, a => a.Tipo == "entrada" && a.Detalle.Contains("Compra semanal"));
        Assert.DoesNotContain(resumen.Actividad, a => a.Detalle.Contains("Consumo comanda"));
    }
}
