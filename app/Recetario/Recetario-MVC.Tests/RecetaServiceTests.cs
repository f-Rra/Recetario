using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Tests;

public class RecetaServiceTests
{
    private const int Noquis = 1;      // Plato Principal, lleva Harina
    private const int Ensalada = 2;    // Entrada, lleva Lechuga, sin procedimiento

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);

        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var principal = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };
        var entrada = new Clasificacion { IdClasificacion = 2, Nombre = "Entrada" };

        context.AddRange(
            kg, principal, entrada,
            new Ingrediente { IdIngrediente = 1, Codigo = "ING001", Descripcion = "Harina 000", IdUnidad = 1, Unidad = kg },
            new Ingrediente { IdIngrediente = 2, Codigo = "ING002", Descripcion = "Lechuga", IdUnidad = 1, Unidad = kg },
            new Receta { IdReceta = Noquis, Codigo = "REC001", Nombre = "Ñoquis", IdClasificacion = 1, Clasificacion = principal, PorcionesBase = 6 },
            new Receta { IdReceta = Ensalada, Codigo = "REC002", Nombre = "Ensalada César", IdClasificacion = 2, Clasificacion = entrada, PorcionesBase = 10 });

        context.IngredientesReceta.AddRange(
            new IngredienteReceta { IdReceta = Noquis, IdIngrediente = 1, CantNeta = 0.5m, Rendimiento = 100m, CantBruta = 0.5m, IdUnidad = 1 },
            new IngredienteReceta { IdReceta = Ensalada, IdIngrediente = 2, CantNeta = 1m, Rendimiento = 100m, CantBruta = 1m, IdUnidad = 1 });

        // Solo los ñoquis tienen procedimiento: la ensalada queda incompleta
        context.Procedimientos.Add(new Procedimiento
        {
            IdProcedimiento = 1,
            IdReceta = Noquis,
            NroPaso = 1,
            Descripcion = "Hervir hasta que floten."
        });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task Listar_BuscaPorNombreYPorIngrediente()
    {
        using var context = CrearContexto();
        var servicio = new RecetaService(context);

        // InMemory compara respetando mayúsculas; contra SQL Server la búsqueda
        // además las ignora por la collation de la base
        var porNombre = await servicio.ListarAsync("Ensalada", null);
        var porIngrediente = await servicio.ListarAsync("Lechuga", null);
        var sinResultados = await servicio.ListarAsync("pizza", null);

        Assert.Equal("Ensalada César", Assert.Single(porNombre).Nombre);
        Assert.Equal("Ensalada César", Assert.Single(porIngrediente).Nombre);
        Assert.Empty(sinResultados);
    }

    [Fact]
    public async Task Listar_AclaraElIngredienteSoloCuandoNoCoincideElNombre()
    {
        using var context = CrearContexto();
        var servicio = new RecetaService(context);

        var porIngrediente = await servicio.ListarAsync("Lechuga", null);
        var porNombre = await servicio.ListarAsync("Ensalada", null);

        Assert.Equal("Lechuga", Assert.Single(porIngrediente).IngredienteCoincidente);
        Assert.Null(Assert.Single(porNombre).IngredienteCoincidente);
    }

    [Fact]
    public async Task Listar_FiltraPorClasificacionYCombinaConLaBusqueda()
    {
        using var context = CrearContexto();
        var servicio = new RecetaService(context);

        var entradas = await servicio.ListarAsync(null, 2);
        var entradasConHarina = await servicio.ListarAsync("Harina", 2);

        Assert.Equal("Ensalada César", Assert.Single(entradas).Nombre);
        Assert.Empty(entradasConHarina); // la harina está en la otra clasificación
    }

    [Fact]
    public async Task Listar_TraeLosConteosParaMarcarLasIncompletas()
    {
        using var context = CrearContexto();
        var servicio = new RecetaService(context);

        var recetas = await servicio.ListarAsync(null, null);

        var noquis = recetas.Single(r => r.IdReceta == Noquis);
        var ensalada = recetas.Single(r => r.IdReceta == Ensalada);

        Assert.True(noquis.Completa);
        Assert.False(ensalada.Completa);
        Assert.Equal("sin procedimiento", ensalada.AdvertenciaCorta);
    }
}
