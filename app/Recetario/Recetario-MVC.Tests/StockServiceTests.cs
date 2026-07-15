using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Tests;

public class StockServiceTests
{
    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new ApplicationDbContext(opciones);
        var unidad = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        context.AddRange(
            unidad,
            new Ingrediente
            {
                IdIngrediente = 1,
                Codigo = "ING001",
                Descripcion = "Harina 000",
                IdUnidad = 1,
                Unidad = unidad,
                StockActual = 10m
            },
            new ApplicationUser { Id = "u1", Nombre = "Admin", Apellido = "Test" });
        context.SaveChanges();
        return context;
    }

    private static MovimientoFormViewModel Movimiento(TipoMovimiento tipo, decimal cantidad) => new()
    {
        IdIngrediente = 1,
        Tipo = tipo,
        Cantidad = cantidad
    };

    [Fact]
    public async Task Entrada_SumaYAuditaConElUsuario()
    {
        using var context = CrearContexto();
        var servicio = new StockService(context);

        var error = await servicio.RegistrarMovimientoAsync(Movimiento(TipoMovimiento.Entrada, 5.5m), "u1");

        Assert.Null(error);
        Assert.Equal(15.5m, (await context.Ingredientes.SingleAsync()).StockActual);
        var movimiento = await context.MovimientosStock.SingleAsync();
        Assert.Equal(TipoMovimiento.Entrada, movimiento.Tipo);
        Assert.Equal("u1", movimiento.UsuarioId);
    }

    [Fact]
    public async Task Salida_RestaYPermiteQuedarNegativo()
    {
        using var context = CrearContexto();
        var servicio = new StockService(context);

        var error = await servicio.RegistrarMovimientoAsync(Movimiento(TipoMovimiento.Salida, 12m), "u1");

        Assert.Null(error);
        Assert.Equal(-2m, (await context.Ingredientes.SingleAsync()).StockActual);
    }

    [Fact]
    public async Task Ajuste_SeteaElStockAlValorContado()
    {
        using var context = CrearContexto();
        var servicio = new StockService(context);

        // Semántica del trigger viejo: "conté y hay 3,25"
        var error = await servicio.RegistrarMovimientoAsync(Movimiento(TipoMovimiento.Ajuste, 3.25m), "u1");

        Assert.Null(error);
        Assert.Equal(3.25m, (await context.Ingredientes.SingleAsync()).StockActual);
    }

    [Fact]
    public async Task Ajuste_ACero_EsValido()
    {
        using var context = CrearContexto();
        var servicio = new StockService(context);

        var error = await servicio.RegistrarMovimientoAsync(Movimiento(TipoMovimiento.Ajuste, 0m), "u1");

        Assert.Null(error);
        Assert.Equal(0m, (await context.Ingredientes.SingleAsync()).StockActual);
    }

    [Fact]
    public async Task CantidadCeroEnEntradaOSalida_FallaSinTocarNada()
    {
        using var context = CrearContexto();
        var servicio = new StockService(context);

        var errorEntrada = await servicio.RegistrarMovimientoAsync(Movimiento(TipoMovimiento.Entrada, 0m), "u1");
        var errorInexistente = await servicio.RegistrarMovimientoAsync(new MovimientoFormViewModel
        {
            IdIngrediente = 99,
            Tipo = TipoMovimiento.Entrada,
            Cantidad = 1m
        }, "u1");

        Assert.NotNull(errorEntrada);
        Assert.NotNull(errorInexistente);
        Assert.Equal(10m, (await context.Ingredientes.SingleAsync()).StockActual);
        Assert.Empty(context.MovimientosStock);
    }

    [Fact]
    public async Task Historial_FiltraPorIngredienteYTipo()
    {
        using var context = CrearContexto();
        context.Ingredientes.Add(new Ingrediente
        {
            IdIngrediente = 2,
            Codigo = "ING002",
            Descripcion = "Aceite",
            IdUnidad = 1,
            StockActual = 5m
        });
        context.SaveChanges();
        var servicio = new StockService(context);
        await servicio.RegistrarMovimientoAsync(Movimiento(TipoMovimiento.Entrada, 1m), "u1");
        await servicio.RegistrarMovimientoAsync(Movimiento(TipoMovimiento.Salida, 2m), "u1");
        await servicio.RegistrarMovimientoAsync(new MovimientoFormViewModel
        {
            IdIngrediente = 2,
            Tipo = TipoMovimiento.Entrada,
            Cantidad = 3m
        }, "u1");

        var todos = await servicio.HistorialAsync(null, null, null, null);
        var soloHarina = await servicio.HistorialAsync(1, null, null, null);
        var soloEntradas = await servicio.HistorialAsync(null, TipoMovimiento.Entrada, null, null);

        Assert.Equal(3, todos.Count);
        Assert.Equal(2, soloHarina.Count);
        Assert.All(soloHarina, m => Assert.Equal("Harina 000", m.Ingrediente));
        Assert.Equal(2, soloEntradas.Count);
        Assert.All(soloEntradas, m => Assert.Equal(TipoMovimiento.Entrada, m.Tipo));
    }

    [Fact]
    public async Task AltaDeIngredienteConStockInicial_GeneraElMovimientoAuditado()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context);

        await servicio.CrearAsync(new IngredienteFormViewModel
        {
            Descripcion = "Tomate perita",
            IdUnidad = 1,
            StockActual = 7m,
            StockMinimo = 2m
        }, "u1");

        var movimiento = await context.MovimientosStock.SingleAsync();
        Assert.Equal(TipoMovimiento.Entrada, movimiento.Tipo);
        Assert.Equal(7m, movimiento.Cantidad);
        Assert.Equal("Stock inicial", movimiento.Observaciones);
        Assert.Equal("u1", movimiento.UsuarioId);
    }
}
