using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Tests;

public class ComandaServiceTests
{
    /// <summary>
    /// Escenario base: receta "Ñoquis" (6 porciones base) con 0,625 kg brutos
    /// de Harina (stock inicial 10), responsable del sector "Plato Principal".
    /// </summary>
    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            // InMemory no soporta transacciones: se ignoran (la atomicidad real
            // la cubre SQL Server; acá se testea la lógica)
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new ApplicationDbContext(opciones);

        var unidad = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var sector = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };
        var harina = new Ingrediente
        {
            IdIngrediente = 1,
            Codigo = "ING001",
            Descripcion = "Harina 000",
            IdUnidad = 1,
            Unidad = unidad,
            StockActual = 10m
        };
        var receta = new Receta
        {
            IdReceta = 1,
            Codigo = "REC001",
            Nombre = "Ñoquis",
            IdClasificacion = 1,
            Clasificacion = sector,
            PorcionesBase = 6,
            Activo = true
        };
        var responsable = new Persona
        {
            IdPersona = 1,
            Nombre = "María",
            Apellido = "López",
            IdClasificacion = 1
        };
        var usuario = new ApplicationUser { Id = "u1", Nombre = "Cocina", Apellido = "Test" };

        context.AddRange(unidad, sector, harina, receta, responsable, usuario);
        context.IngredientesReceta.Add(new IngredienteReceta
        {
            IdReceta = 1,
            IdIngrediente = 1,
            CantNeta = 0.5m,
            Rendimiento = 80m,
            CantBruta = 0.625m,
            IdUnidad = 1
        });
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task Registrar_DescuentaStockEscaladoALasPorciones()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        // 12 porciones sobre base 6 → factor 2 → consumo 1,25 kg
        var (idComanda, error) = await servicio.RegistrarAsync(1, 12, "u1", idPersona: null);

        Assert.Null(error);
        Assert.NotNull(idComanda);
        var harina = await context.Ingredientes.SingleAsync();
        Assert.Equal(8.75m, harina.StockActual); // 10 − 1,25
    }

    [Fact]
    public async Task Registrar_GeneraElMovimientoDeSalidaAuditado()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var (idComanda, _) = await servicio.RegistrarAsync(1, 12, "u1", null);

        var movimiento = await context.MovimientosStock.SingleAsync();
        Assert.Equal(TipoMovimiento.Salida, movimiento.Tipo);
        Assert.Equal(1.25m, movimiento.Cantidad);
        Assert.Equal("u1", movimiento.UsuarioId);
        Assert.Equal($"Consumo comanda #{idComanda}", movimiento.Observaciones);
    }

    [Fact]
    public async Task Registrar_AsignaElResponsableDelSector()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var (idComanda, _) = await servicio.RegistrarAsync(1, 6, "u1", idPersona: null);

        var comanda = await context.Comandas.SingleAsync(c => c.IdComanda == idComanda);
        Assert.Equal(1, comanda.IdPersona); // María López, sector Plato Principal
    }

    [Fact]
    public async Task Registrar_SinResponsableDelSector_Falla()
    {
        using var context = CrearContexto();
        context.Personas.RemoveRange(context.Personas);
        context.SaveChanges();
        var servicio = new ComandaService(context);

        var (idComanda, error) = await servicio.RegistrarAsync(1, 6, "u1", null);

        Assert.Null(idComanda);
        Assert.Contains("responsable", error, StringComparison.OrdinalIgnoreCase);
        Assert.Empty(context.Comandas);
        Assert.Equal(10m, (await context.Ingredientes.SingleAsync()).StockActual); // sin tocar
    }

    [Fact]
    public async Task Registrar_RecetaInactivaOPorcionesInvalidas_Falla()
    {
        using var context = CrearContexto();
        var receta = await context.Recetas.SingleAsync();
        receta.Activo = false;
        context.SaveChanges();
        var servicio = new ComandaService(context);

        var inactiva = await servicio.RegistrarAsync(1, 6, "u1", null);
        var porcionesCero = await servicio.RegistrarAsync(1, 0, "u1", null);

        Assert.NotNull(inactiva.Error);
        Assert.NotNull(porcionesCero.Error);
        Assert.Empty(context.Comandas);
    }

    [Fact]
    public async Task Sustitucion_AjustaAmbosStocksYAuditaLosMovimientos()
    {
        using var context = CrearContexto();
        context.Ingredientes.Add(new Ingrediente
        {
            IdIngrediente = 2,
            Codigo = "ING002",
            Descripcion = "Harina integral",
            IdUnidad = 1,
            StockActual = 5m
        });
        context.SaveChanges();
        var servicio = new ComandaService(context);
        var (idComanda, _) = await servicio.RegistrarAsync(1, 6, "u1", null);
        var stockHarinaTrasComanda = (await context.Ingredientes.FindAsync(1))!.StockActual;

        var error = await servicio.AgregarModificacionAsync(new ModificacionFormViewModel
        {
            IdComanda = idComanda!.Value,
            Tipo = TipoModificacion.Sustitucion,
            IdIngredienteOriginal = 1,
            IdIngredienteReemplazo = 2,
            Cantidad = 0.5m
        }, "u1");

        Assert.Null(error);
        Assert.Equal(stockHarinaTrasComanda + 0.5m, (await context.Ingredientes.FindAsync(1))!.StockActual);
        Assert.Equal(4.5m, (await context.Ingredientes.FindAsync(2))!.StockActual);

        // Mejora sobre el trigger viejo: los dos ajustes quedan auditados
        var movimientosModificacion = await context.MovimientosStock
            .Where(m => m.Observaciones!.Contains("modificación"))
            .ToListAsync();
        Assert.Equal(2, movimientosModificacion.Count);
        Assert.Contains(movimientosModificacion, m => m.Tipo == TipoMovimiento.Entrada && m.IdIngrediente == 1);
        Assert.Contains(movimientosModificacion, m => m.Tipo == TipoMovimiento.Salida && m.IdIngrediente == 2);
    }

    [Fact]
    public async Task Modificacion_ValidaLosIngredientesSegunElTipo()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);
        var (idComanda, _) = await servicio.RegistrarAsync(1, 6, "u1", null);

        // Sustitución sin reemplazo
        var sinReemplazo = await servicio.AgregarModificacionAsync(new ModificacionFormViewModel
        {
            IdComanda = idComanda!.Value,
            Tipo = TipoModificacion.Sustitucion,
            IdIngredienteOriginal = 1,
            Cantidad = 0.5m
        }, "u1");

        // Adición con original (no corresponde)
        var adicionConOriginal = await servicio.AgregarModificacionAsync(new ModificacionFormViewModel
        {
            IdComanda = idComanda.Value,
            Tipo = TipoModificacion.Adicion,
            IdIngredienteOriginal = 1,
            IdIngredienteReemplazo = 1,
            Cantidad = 0.5m
        }, "u1");

        // Eliminación válida
        var eliminacionValida = await servicio.AgregarModificacionAsync(new ModificacionFormViewModel
        {
            IdComanda = idComanda.Value,
            Tipo = TipoModificacion.Eliminacion,
            IdIngredienteOriginal = 1,
            Cantidad = 0.25m
        }, "u1");

        Assert.NotNull(sinReemplazo);
        Assert.NotNull(adicionConOriginal);
        Assert.Null(eliminacionValida);
        Assert.Single(context.Modificaciones); // solo la eliminación se guardó
    }
}
