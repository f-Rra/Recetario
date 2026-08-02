using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Tests;

public class ComandaServiceTests
{
    // Ids del escenario base
    private const int Noquis = 1;      // Plato Principal, base 6, lleva 0,625 kg de Harina
    private const int Decoracion = 2;  // sector sin responsable propio
    private const int Harina = 1;      // stock 10
    private const int Integral = 2;    // stock 5
    private const int Perejil = 3;     // stock 4, ingrediente de la decoración

    /// <summary>
    /// Receta "Ñoquis" (6 porciones, 0,625 kg brutos de Harina) con responsable
    /// de sector, más una decoración cuyo sector no tiene responsable propio.
    /// </summary>
    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            // InMemory no soporta transacciones: la atomicidad real la cubre SQL Server
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new ApplicationDbContext(opciones);

        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var principal = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };
        var deco = new Clasificacion { IdClasificacion = 2, Nombre = "Decoración" };

        context.AddRange(
            kg, principal, deco,
            new Ingrediente { IdIngrediente = Harina, Codigo = "ING001", Descripcion = "Harina 000", IdUnidad = 1, Unidad = kg, StockActual = 10m },
            new Ingrediente { IdIngrediente = Integral, Codigo = "ING002", Descripcion = "Harina integral", IdUnidad = 1, Unidad = kg, StockActual = 5m },
            new Ingrediente { IdIngrediente = Perejil, Codigo = "ING003", Descripcion = "Perejil", IdUnidad = 1, Unidad = kg, StockActual = 4m },
            new Receta { IdReceta = Noquis, Codigo = "REC001", Nombre = "Ñoquis", IdClasificacion = 1, Clasificacion = principal, PorcionesBase = 6, Activo = true },
            new Receta { IdReceta = Decoracion, Codigo = "REC002", Nombre = "Guarnición", IdClasificacion = 2, Clasificacion = deco, PorcionesBase = 6, Activo = true },
            new Persona { IdPersona = 1, Nombre = "María", Apellido = "López", IdClasificacion = 1 },
            new ApplicationUser { Id = "u1", Nombre = "Cocina", Apellido = "Test" });

        context.IngredientesReceta.AddRange(
            new IngredienteReceta { IdReceta = Noquis, IdIngrediente = Harina, CantNeta = 0.5m, Rendimiento = 80m, CantBruta = 0.625m, IdUnidad = 1 },
            new IngredienteReceta { IdReceta = Decoracion, IdIngrediente = Perejil, CantNeta = 0.1m, Rendimiento = 100m, CantBruta = 0.1m, IdUnidad = 1 });

        context.SaveChanges();
        return context;
    }

    private static CarritoComanda Carrito(int comensales, params CarritoItem[] items) =>
        new() { Comensales = comensales, Items = items.ToList() };

    private static CarritoItem ItemNoquis(params ModificacionCarrito[] modificaciones) => new()
    {
        IdReceta = Noquis,
        Nombre = "Ñoquis",
        Clasificacion = "Plato Principal",
        IdClasificacion = 1,
        PorcionesBase = 6,
        Modificaciones = modificaciones.ToList()
    };

    private static CarritoItem ItemDecoracion() => new()
    {
        IdReceta = Decoracion,
        Nombre = "Guarnición",
        Clasificacion = "Decoración",
        IdClasificacion = 2,
        PorcionesBase = 6
    };

    [Fact]
    public async Task Generar_DescuentaStockEscaladoALosComensales()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        // 12 comensales sobre base 6 → factor 2 → 1,25 kg de harina
        var resultado = await servicio.GenerarAsync(Carrito(12, ItemNoquis()), "u1");

        Assert.True(resultado.Ok);
        Assert.Single(resultado.IdsComandas);
        Assert.Equal(8.75m, (await context.Ingredientes.FindAsync(Harina))!.StockActual);
    }

    [Fact]
    public async Task Generar_VariasRecetas_RegistraUnaComandaPorReceta()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var resultado = await servicio.GenerarAsync(
            Carrito(6, ItemNoquis(), ItemDecoracion()), "u1");

        Assert.True(resultado.Ok);
        Assert.Equal(2, resultado.IdsComandas.Count);
        Assert.Equal(2, await context.Comandas.CountAsync());
        Assert.All(await context.Comandas.ToListAsync(), c => Assert.Equal(6, c.Porciones));
    }

    [Fact]
    public async Task Generar_RecetaSinResponsablePropio_HeredaElDeLaPrincipal()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var resultado = await servicio.GenerarAsync(
            Carrito(6, ItemNoquis(), ItemDecoracion()), "u1");

        Assert.True(resultado.Ok);
        var comandas = await context.Comandas.ToListAsync();
        Assert.All(comandas, c => Assert.Equal(1, c.IdPersona)); // María López
    }

    [Fact]
    public async Task Sustitucion_ConsumeElReemplazoYNoElOriginal()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var resultado = await servicio.GenerarAsync(
            Carrito(6, ItemNoquis(new ModificacionCarrito
            {
                Tipo = TipoModificacion.Sustitucion,
                IdIngredienteOriginal = Harina,
                IdIngredienteReemplazo = Integral,
                Cantidad = null // hereda la cantidad de la receta escalada
            })), "u1");

        Assert.True(resultado.Ok);
        Assert.Equal(10m, (await context.Ingredientes.FindAsync(Harina))!.StockActual);      // intacta
        Assert.Equal(4.375m, (await context.Ingredientes.FindAsync(Integral))!.StockActual); // 5 − 0,625

        var movimiento = await context.MovimientosStock.SingleAsync();
        Assert.Equal(Integral, movimiento.IdIngrediente);
        Assert.Equal(0.625m, movimiento.Cantidad);
    }

    [Fact]
    public async Task Eliminacion_NoDescuentaEseIngrediente()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var resultado = await servicio.GenerarAsync(
            Carrito(6, ItemNoquis(new ModificacionCarrito
            {
                Tipo = TipoModificacion.Eliminacion,
                IdIngredienteOriginal = Harina
            })), "u1");

        Assert.True(resultado.Ok);
        Assert.Equal(10m, (await context.Ingredientes.FindAsync(Harina))!.StockActual);
        Assert.Empty(context.MovimientosStock);
        Assert.Single(context.Modificaciones);
    }

    [Fact]
    public async Task Adicion_DescuentaLaCantidadIndicadaParaLaComanda()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var resultado = await servicio.GenerarAsync(
            Carrito(6, ItemNoquis(new ModificacionCarrito
            {
                Tipo = TipoModificacion.Adicion,
                IdIngredienteReemplazo = Perejil,
                Cantidad = 1.5m // total para toda la comanda
            })), "u1");

        Assert.True(resultado.Ok);
        Assert.Equal(2.5m, (await context.Ingredientes.FindAsync(Perejil))!.StockActual); // 4 − 1,5
        Assert.Equal(9.375m, (await context.Ingredientes.FindAsync(Harina))!.StockActual); // sigue la receta
    }

    [Fact]
    public async Task Generar_SinStockSuficiente_BloqueaSinRegistrarNada()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        // 6 comensales → 0,625 kg; 200 comensales → 20,83 kg y solo hay 10
        var resultado = await servicio.GenerarAsync(Carrito(200, ItemNoquis()), "u1");

        Assert.False(resultado.Ok);
        Assert.Equal("Harina 000", Assert.Single(resultado.Faltantes).Ingrediente);
        Assert.Empty(context.Comandas);
        Assert.Empty(context.MovimientosStock);
        Assert.Equal(10m, (await context.Ingredientes.FindAsync(Harina))!.StockActual);
    }

    [Fact]
    public async Task Generar_CarritoVacioOComensalesInvalidos_Falla()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var vacio = await servicio.GenerarAsync(Carrito(6), "u1");
        var sinComensales = await servicio.GenerarAsync(Carrito(0, ItemNoquis()), "u1");

        Assert.False(vacio.Ok);
        Assert.False(sinComensales.Ok);
        Assert.Empty(context.Comandas);
    }

    [Fact]
    public async Task Generar_SinNingunResponsable_Falla()
    {
        using var context = CrearContexto();
        context.Personas.RemoveRange(context.Personas);
        context.SaveChanges();
        var servicio = new ComandaService(context);

        var resultado = await servicio.GenerarAsync(Carrito(6, ItemNoquis()), "u1");

        Assert.False(resultado.Ok);
        Assert.Contains("responsable", resultado.Error, StringComparison.OrdinalIgnoreCase);
        Assert.Empty(context.Comandas);
    }

    [Fact]
    public async Task Generar_ArmaLasSeccionesDelPdfConIngredientesModificados()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var resultado = await servicio.GenerarAsync(
            Carrito(6, ItemNoquis(new ModificacionCarrito
            {
                Tipo = TipoModificacion.Sustitucion,
                IdIngredienteOriginal = Harina,
                NombreOriginal = "Harina 000",
                IdIngredienteReemplazo = Integral,
                NombreReemplazo = "Harina integral"
            })), "u1");

        var seccion = Assert.Single(resultado.Secciones);
        Assert.Equal("Ñoquis", seccion.Receta);
        Assert.Equal("REC001", seccion.Codigo);
        Assert.Equal("María López", seccion.Responsable);

        // El PDF lleva el ingrediente ya sustituido...
        var ingrediente = Assert.Single(seccion.Ingredientes);
        Assert.Equal("Harina integral", ingrediente.Ingrediente);
        Assert.Equal(0.625m, ingrediente.Cantidad);

        // ...y el detalle de qué se cambió
        Assert.Contains("Sustituir Harina 000 por Harina integral", Assert.Single(seccion.Modificaciones));
    }

    [Fact]
    public async Task ObtenerIngredientesPreview_EscalaYMarcaSiNoAlcanza()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var alcanza = await servicio.ObtenerIngredientesPreviewAsync(Noquis, 12);
        var noAlcanza = await servicio.ObtenerIngredientesPreviewAsync(Noquis, 200);

        Assert.Equal(1.25m, alcanza!.Ingredientes.Single().Cantidad);
        Assert.True(alcanza.Ingredientes.Single().Alcanza);
        Assert.False(noAlcanza!.Ingredientes.Single().Alcanza);
    }

    // ---------- Revisión previa (guía 18) ----------

    [Fact]
    public async Task Revisar_AvisaElFaltanteSinTocarNada()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        // 200 comensales sobre base 6 → 20,83 kg de harina y solo hay 10
        var revision = await servicio.RevisarAsync(Carrito(200, ItemNoquis()));

        Assert.False(revision.SePuedeGenerar);
        var faltante = Assert.Single(revision.Faltantes);
        Assert.Equal("Harina 000", faltante.Ingrediente);
        Assert.Equal(10m, faltante.Disponible);
        Assert.True(faltante.Necesario > faltante.Disponible);
        Assert.Empty(context.Comandas);
        Assert.Empty(context.MovimientosStock);
    }

    [Fact]
    public async Task Revisar_ConStockSuficiente_NoAvisaFaltantes()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var revision = await servicio.RevisarAsync(Carrito(12, ItemNoquis()));

        Assert.True(revision.SePuedeGenerar);
        Assert.Empty(revision.Faltantes);
    }

    [Fact]
    public async Task Revisar_SumaElMismoIngredienteEntreRecetas()
    {
        using var context = CrearContexto();
        // La guarnición también lleva harina: por separado alcanza, juntas no
        context.IngredientesReceta.Add(new IngredienteReceta
        {
            IdReceta = Decoracion,
            IdIngrediente = Harina,
            CantNeta = 0.5m,
            Rendimiento = 100m,
            CantBruta = 0.5m,
            IdUnidad = 1
        });
        context.SaveChanges();
        var servicio = new ComandaService(context);

        // 60 comensales: 6,25 kg de los ñoquis + 5 kg de la guarnición = 11,25 > 10
        var soloNoquis = await servicio.RevisarAsync(Carrito(60, ItemNoquis()));
        var juntas = await servicio.RevisarAsync(Carrito(60, ItemNoquis(), ItemDecoracion()));

        Assert.True(soloNoquis.SePuedeGenerar);
        Assert.False(juntas.SePuedeGenerar);
        Assert.Equal(11.25m, Assert.Single(juntas.Faltantes).Necesario);
    }

    [Fact]
    public async Task Revisar_CarritoVacio_NoAvisaNada()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var revision = await servicio.RevisarAsync(Carrito(200));

        Assert.True(revision.SePuedeGenerar);
        Assert.Empty(revision.Incompletas);
    }

    [Fact]
    public async Task Revisar_MarcaLasRecetasIncompletasSinBloquear()
    {
        using var context = CrearContexto();
        // Los ñoquis quedan completos; la guarnición sigue sin procedimiento
        context.Procedimientos.Add(new Procedimiento
        {
            IdProcedimiento = 1,
            IdReceta = Noquis,
            NroPaso = 1,
            Descripcion = "Hervir hasta que floten."
        });
        context.SaveChanges();
        var servicio = new ComandaService(context);

        var revision = await servicio.RevisarAsync(Carrito(6, ItemNoquis(), ItemDecoracion()));

        // Avisa, pero no impide generar: el faltante es lo único que bloquea
        Assert.True(revision.SePuedeGenerar);
        var aviso = Assert.Single(revision.Incompletas);
        Assert.Contains("Guarnición", aviso);
        Assert.Contains("procedimiento", aviso);
    }

    [Fact]
    public async Task ListarCatalogo_BuscaTambienPorIngrediente()
    {
        using var context = CrearContexto();
        var servicio = new ComandaService(context);

        var porIngrediente = await servicio.ListarCatalogoAsync("Perejil", null);
        var porNombre = await servicio.ListarCatalogoAsync("Ñoq", null);

        // La guarnición aparece por lo que lleva, y se aclara cuál fue
        var guarnicion = Assert.Single(porIngrediente);
        Assert.Equal("Guarnición", guarnicion.Nombre);
        Assert.Equal("Perejil", guarnicion.IngredienteCoincidente);

        // Cuando coincide por nombre no hace falta explicar nada
        Assert.Null(Assert.Single(porNombre).IngredienteCoincidente);
    }

    [Fact]
    public async Task ListarCatalogo_TraeLosConteosParaMarcarLasIncompletas()
    {
        using var context = CrearContexto();
        context.Procedimientos.Add(new Procedimiento
        {
            IdProcedimiento = 1,
            IdReceta = Noquis,
            NroPaso = 1,
            Descripcion = "Hervir hasta que floten."
        });
        context.SaveChanges();
        var servicio = new ComandaService(context);

        var catalogo = await servicio.ListarCatalogoAsync(null, null);

        var noquis = catalogo.Single(r => r.IdReceta == Noquis);
        var guarnicion = catalogo.Single(r => r.IdReceta == Decoracion);

        Assert.True(noquis.Completa);
        Assert.False(guarnicion.Completa);
        Assert.Equal("No tiene procedimiento cargado", guarnicion.Advertencia);
    }

    [Fact]
    public async Task ListarCatalogo_SoloRecetasActivasYFiltraPorClasificacion()
    {
        using var context = CrearContexto();
        var receta = await context.Recetas.FindAsync(Decoracion);
        receta!.Activo = false;
        context.SaveChanges();
        var servicio = new ComandaService(context);

        var todas = await servicio.ListarCatalogoAsync(null, null);
        var porSector = await servicio.ListarCatalogoAsync(null, 1);
        // InMemory compara respetando mayúsculas; contra SQL Server la búsqueda
        // además las ignora por la collation de la base
        var porTexto = await servicio.ListarCatalogoAsync("Ñoq", null);
        var sinResultados = await servicio.ListarCatalogoAsync("pizza", null);

        Assert.Single(todas); // la inactiva no aparece
        Assert.Single(porSector);
        Assert.Single(porTexto);
        Assert.Empty(sinResultados);
    }
}
