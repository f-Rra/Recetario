using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Tests;

public class CosteoServiceTests
{
    /// <summary>
    /// Contexto InMemory aislado por test, con el escenario base:
    /// receta "Ñoquis" (6 porciones) con 0,625 kg brutos de Harina a $1.200,50.
    /// Mismo caso cargado en desarrollo, verificable a mano:
    /// total = 0,625 × 1.200,50 = 750,3125 · unitario = 750,3125 / 6 = 125,0521.
    /// </summary>
    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);

        var unidad = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var clasificacion = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };
        var harina = new Ingrediente
        {
            IdIngrediente = 1,
            Codigo = "ING001",
            Descripcion = "Harina 000",
            IdUnidad = 1,
            Unidad = unidad
        };
        var proveedor = new Proveedor { IdProveedor = 1, Nombre = "Molinos del Sur" };
        var receta = new Receta
        {
            IdReceta = 1,
            Codigo = "REC001",
            Nombre = "Ñoquis",
            IdClasificacion = 1,
            Clasificacion = clasificacion,
            PorcionesBase = 6
        };

        context.AddRange(unidad, clasificacion, harina, proveedor, receta);
        context.IngredientesReceta.Add(new IngredienteReceta
        {
            IdReceta = 1,
            IdIngrediente = 1,
            CantNeta = 0.5m,
            Rendimiento = 80m,
            CantBruta = 0.625m,
            IdUnidad = 1
        });
        context.PreciosIngrediente.Add(new PrecioIngrediente
        {
            IdPrecio = 1,
            IdIngrediente = 1,
            IdProveedor = 1,
            Precio = 1200.50m,
            FechaVigencia = new DateOnly(2026, 7, 1)
        });
        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task Calcular_CasoBase_CoincideConValorCalculadoAMano()
    {
        using var context = CrearContexto();
        var servicio = new CosteoService(context);

        var resultado = await servicio.CalcularAsync(idReceta: 1, porciones: 6);

        Assert.NotNull(resultado);
        Assert.True(resultado.Ok);
        Assert.Equal(750.3125m, resultado.CostoTotal);
        Assert.Equal(125.0521m, resultado.CostoUnitario);
        var detalle = Assert.Single(resultado.Detalles);
        Assert.Equal(0.625m, detalle.CantBruta);
        Assert.Equal(1200.50m, detalle.PrecioUnitario);
    }

    [Fact]
    public async Task Calcular_UsaElPrecioDeFechaMasReciente()
    {
        using var context = CrearContexto();
        context.PreciosIngrediente.Add(new PrecioIngrediente
        {
            IdPrecio = 2,
            IdIngrediente = 1,
            IdProveedor = 1,
            Precio = 2000m,
            FechaVigencia = new DateOnly(2026, 7, 10)
        });
        context.SaveChanges();
        var servicio = new CosteoService(context);

        var resultado = await servicio.CalcularAsync(1, 6);

        Assert.NotNull(resultado);
        Assert.Equal(2000m, resultado.Detalles.Single().PrecioUnitario);
    }

    [Fact]
    public async Task Calcular_ConFechasEmpatadas_GanaElUltimoCargado()
    {
        using var context = CrearContexto();
        context.PreciosIngrediente.Add(new PrecioIngrediente
        {
            IdPrecio = 2,
            IdIngrediente = 1,
            IdProveedor = 1,
            Precio = 1500m,
            FechaVigencia = new DateOnly(2026, 7, 1) // misma fecha que el precio 1
        });
        context.SaveChanges();
        var servicio = new CosteoService(context);

        var resultado = await servicio.CalcularAsync(1, 6);

        Assert.NotNull(resultado);
        Assert.Equal(1500m, resultado.Detalles.Single().PrecioUnitario);
    }

    [Fact]
    public async Task Calcular_ElDobleDePorciones_DuplicaElTotalYMantieneElUnitario()
    {
        using var context = CrearContexto();
        var servicio = new CosteoService(context);

        var basePorciones = await servicio.CalcularAsync(1, 6);
        var doble = await servicio.CalcularAsync(1, 12);

        Assert.NotNull(basePorciones);
        Assert.NotNull(doble);
        Assert.Equal(basePorciones.CostoTotal * 2, doble.CostoTotal);
        Assert.Equal(basePorciones.CostoUnitario, doble.CostoUnitario); // invariante
        Assert.Equal(1.25m, doble.Detalles.Single().CantBruta);         // 0,625 × 2
    }

    [Fact]
    public async Task Calcular_IngredienteSinPrecio_BloqueaEInformaCual()
    {
        using var context = CrearContexto();
        context.Ingredientes.Add(new Ingrediente
        {
            IdIngrediente = 2,
            Codigo = "ING002",
            Descripcion = "Tomate perita",
            IdUnidad = 1
        });
        context.IngredientesReceta.Add(new IngredienteReceta
        {
            IdReceta = 1,
            IdIngrediente = 2,
            CantNeta = 1m,
            Rendimiento = 100m,
            CantBruta = 1m,
            IdUnidad = 1
        });
        context.SaveChanges();
        var servicio = new CosteoService(context);

        var resultado = await servicio.CalcularAsync(1, 6);

        Assert.NotNull(resultado);
        Assert.False(resultado.Ok);
        Assert.Equal("Tomate perita", Assert.Single(resultado.IngredientesSinPrecio));
        Assert.Equal(0m, resultado.CostoTotal); // nunca se costea incompleto
    }

    [Fact]
    public async Task Calcular_RecetaInexistenteOPorcionesInvalidas_DevuelveNull()
    {
        using var context = CrearContexto();
        var servicio = new CosteoService(context);

        Assert.Null(await servicio.CalcularAsync(idReceta: 99, porciones: 6));
        Assert.Null(await servicio.CalcularAsync(idReceta: 1, porciones: 0));
        Assert.Null(await servicio.CalcularAsync(idReceta: 1, porciones: -3));
    }

    [Fact]
    public async Task Registrar_PersisteCabeceraYDesglose()
    {
        using var context = CrearContexto();
        context.Users.Add(new ApplicationUser { Id = "u1", Nombre = "Admin", Apellido = "Test" });
        context.SaveChanges();
        var servicio = new CosteoService(context);

        var idCosto = await servicio.RegistrarAsync(1, 6, "u1");

        Assert.NotNull(idCosto);
        var costo = await context.Costos.Include(c => c.Detalles).SingleAsync();
        Assert.Equal(750.3125m, costo.CostoTotal);
        Assert.Equal(125.0521m, costo.CostoUnitario);
        Assert.Equal(6, costo.Porciones);
        Assert.Equal("u1", costo.UsuarioId);
        var detalle = Assert.Single(costo.Detalles);
        Assert.Equal(0.625m, detalle.CantBruta);
        Assert.Equal(750.3125m, detalle.Subtotal);
    }

    [Fact]
    public async Task Registrar_ConIngredienteSinPrecio_NoPersisteNada()
    {
        using var context = CrearContexto();
        context.PreciosIngrediente.RemoveRange(context.PreciosIngrediente);
        context.SaveChanges();
        var servicio = new CosteoService(context);

        var idCosto = await servicio.RegistrarAsync(1, 6, "u1");

        Assert.Null(idCosto);
        Assert.Empty(context.Costos);
    }
}
