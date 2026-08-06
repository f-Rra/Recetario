using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Tests;

public class IngredienteServiceTests
{
    private const int Lechuga = 1;   // en dos recetas, stock crítico
    private const int Harina = 2;    // sin recetas, stock ok

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);

        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var clasificacion = new Clasificacion { IdClasificacion = 1, Nombre = "Ensalada" };

        context.AddRange(
            kg, clasificacion,
            new Ingrediente
            {
                IdIngrediente = Lechuga, Codigo = "ING001", Descripcion = "Lechuga",
                IdUnidad = 1, Unidad = kg, Deposito = Deposito.Camara,
                StockActual = 2m, StockMinimo = 5m
            },
            new Ingrediente
            {
                IdIngrediente = Harina, Codigo = "ING002", Descripcion = "Harina 0000",
                IdUnidad = 1, Unidad = kg, Deposito = Deposito.Bodega,
                StockActual = 24m, StockMinimo = 6m
            },
            // "Ensalada" ordena antes que "Sopa": el detalle las devuelve así
            new Receta { IdReceta = 1, Codigo = "REC001", Nombre = "Sopa fría", IdClasificacion = 1, Clasificacion = clasificacion, PorcionesBase = 10 },
            new Receta { IdReceta = 2, Codigo = "REC002", Nombre = "Ensalada César", IdClasificacion = 1, Clasificacion = clasificacion, PorcionesBase = 10 });

        context.IngredientesReceta.AddRange(
            new IngredienteReceta { IdReceta = 1, IdIngrediente = Lechuga, CantNeta = 1m, Rendimiento = 100m, CantBruta = 1m, IdUnidad = 1 },
            new IngredienteReceta { IdReceta = 2, IdIngrediente = Lechuga, CantNeta = 2m, Rendimiento = 100m, CantBruta = 2m, IdUnidad = 1 });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task Detalle_TraeLosDatosConSuEstadoDeStock()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context);

        var detalle = await servicio.ObtenerDetalleAsync(Lechuga);

        Assert.NotNull(detalle);
        Assert.Equal("Lechuga", detalle!.Descripcion);
        Assert.Equal("ING001", detalle.Codigo);
        Assert.Equal("kg", detalle.Unidad);
        Assert.Equal(Deposito.Camara, detalle.Deposito);
        Assert.Equal(StockEstado.Critico, detalle.Estado); // 2 kg con mínimo 5
    }

    [Fact]
    public async Task Detalle_ListaLasRecetasQueLoLlevan()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context);

        var lechuga = await servicio.ObtenerDetalleAsync(Lechuga);
        var harina = await servicio.ObtenerDetalleAsync(Harina);

        Assert.Equal(new[] { "Ensalada César", "Sopa fría" }, lechuga!.Recetas);
        Assert.Empty(harina!.Recetas);
    }

    [Fact]
    public async Task Detalle_DejaElFormularioListoParaEditar()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context);

        var detalle = await servicio.ObtenerDetalleAsync(Lechuga);

        // El modal de edición se arma con esto, sin ir de nuevo a la base
        Assert.Equal(Lechuga, detalle!.Datos.IdIngrediente);
        Assert.Equal("Lechuga", detalle.Datos.Descripcion);
        Assert.Equal(1, detalle.Datos.IdUnidad);
        Assert.Equal(Deposito.Camara, detalle.Datos.Deposito);
        Assert.Equal(5m, detalle.Datos.StockMinimo);
        Assert.Equal(Lechuga, detalle.NuevoPrecio.IdIngrediente);
    }

    [Fact]
    public async Task Detalle_DeUnIngredienteQueNoExiste_EsNulo()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context);

        Assert.Null(await servicio.ObtenerDetalleAsync(999));
    }
}
