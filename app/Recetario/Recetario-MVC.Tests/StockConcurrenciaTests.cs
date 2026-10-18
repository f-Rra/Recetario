using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Tests;

// Dos personas tocando el stock del mismo ingrediente a la vez. Una carrera real es
// imposible de repetir a pedido, así que se arma a mano: justo antes de que el servicio
// guarde su descuento, "otra persona" cambia el stock desde un segundo contexto sobre la
// misma base. Es lo que pasaría con dos pedidos que se pisan en el servidor.
//
// Nota: la base en memoria no soporta transacciones, así que acá no se puede comprobar
// que lo hecho a medias se deshaga (eso lo hace SQL Server); sí se comprueba que el stock
// quede bien y que el servicio lo vuelva a revisar.
public class StockConcurrenciaTests
{
    private const int Noquis = 1;   // base 6, lleva 0,625 kg de Harina
    private const int Harina = 1;   // stock 10

    // Se dispara una sola vez, justo antes del primer guardado que modifica un ingrediente
    private sealed class OtraPersonaCambiaElStock : SaveChangesInterceptor
    {
        private readonly Func<Task> _cambio;
        private bool _hecho;

        public OtraPersonaCambiaElStock(Func<Task> cambio) => _cambio = cambio;

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var contexto = eventData.Context!;
            contexto.ChangeTracker.DetectChanges();

            if (!_hecho && contexto.ChangeTracker.Entries<Ingrediente>().Any(e => e.State == EntityState.Modified))
            {
                _hecho = true;
                await _cambio();
            }

            return result;
        }
    }

    private static DbContextOptionsBuilder<ApplicationDbContext> Opciones(string baseDeDatos) =>
        new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(baseDeDatos)
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

    // Siembra una base con un ingrediente (Harina, stock 10) y devuelve su nombre para
    // abrir otros contextos sobre la misma
    private static string CrearBase()
    {
        var nombre = Guid.NewGuid().ToString();
        using var context = new ApplicationDbContext(Opciones(nombre).Options);

        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        var principal = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };

        context.AddRange(
            kg, principal,
            new Ingrediente { IdIngrediente = Harina, Codigo = "ING001", Descripcion = "Harina 000", IdUnidad = 1, Unidad = kg, StockActual = 10m },
            new Receta { IdReceta = Noquis, Codigo = "REC001", Nombre = "Ñoquis", IdClasificacion = 1, Clasificacion = principal, PorcionesBase = 6, Activo = true },
            new Persona { IdPersona = 1, Nombre = "María", Apellido = "López", IdClasificacion = 1 },
            new ApplicationUser { Id = "u1", Nombre = "Cocina", Apellido = "Test" });

        context.IngredientesReceta.Add(
            new IngredienteReceta { IdReceta = Noquis, IdIngrediente = Harina, CantNeta = 0.5m, Rendimiento = 80m, CantBruta = 0.625m, IdUnidad = 1 });

        context.SaveChanges();
        return nombre;
    }

    // "Otra persona" mueve el stock de la Harina: cantidad negativa descuenta, positiva suma
    private static async Task CambiarStockAsync(string baseDeDatos, decimal cantidad)
    {
        using var otro = new ApplicationDbContext(Opciones(baseDeDatos).Options);
        var harina = await otro.Ingredientes.FindAsync(Harina);
        harina!.StockActual += cantidad;
        await otro.SaveChangesAsync();
    }

    private static async Task<decimal> StockDeHarinaAsync(string baseDeDatos)
    {
        using var lector = new ApplicationDbContext(Opciones(baseDeDatos).Options);
        return (await lector.Ingredientes.FindAsync(Harina))!.StockActual;
    }

    // 12 comensales sobre base 6 → factor 2 → 1,25 kg, que redondea a 1,5 kg de harina
    private static CarritoComanda CarritoDeNoquis() => new()
    {
        Comensales = 12,
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
    public async Task Generar_SiOtraComandaSeLlevaElStockEnElMedio_NoLoDejaNegativoYAvisaLoQueFalta()
    {
        var baseDeDatos = CrearBase();
        // Entre el chequeo de stock y el descuento, otra comanda se lleva 9 kg: quedan 1 y esta necesita 1,5
        var interceptor = new OtraPersonaCambiaElStock(() => CambiarStockAsync(baseDeDatos, -9m));
        using var contexto = new ApplicationDbContext(Opciones(baseDeDatos).AddInterceptors(interceptor).Options);
        var servicio = new ComandaService(contexto);

        var resultado = await servicio.GenerarAsync(CarritoDeNoquis(), "u1");

        Assert.False(resultado.Ok);
        var faltante = Assert.Single(resultado.Faltantes);
        Assert.Equal(1.5m, faltante.Necesario);
        Assert.Equal(1m, faltante.Disponible);
        Assert.Equal(1m, await StockDeHarinaAsync(baseDeDatos));
    }

    [Fact]
    public async Task Generar_SiOtraComandaSeLlevaUnPocoEnElMedio_DescuentaSobreElStockNuevo()
    {
        var baseDeDatos = CrearBase();
        // Otra comanda se lleva 1 kg en el medio: quedan 9, todavía alcanza
        var interceptor = new OtraPersonaCambiaElStock(() => CambiarStockAsync(baseDeDatos, -1m));
        using var contexto = new ApplicationDbContext(Opciones(baseDeDatos).AddInterceptors(interceptor).Options);
        var servicio = new ComandaService(contexto);

        var resultado = await servicio.GenerarAsync(CarritoDeNoquis(), "u1");

        Assert.True(resultado.Ok);
        Assert.Single(resultado.IdsComandas);
        // 10 - 1 (la otra comanda) - 1,5 (esta). Sin el control, esta pisaba el descuento
        // de la otra y quedaban 8,5.
        Assert.Equal(7.5m, await StockDeHarinaAsync(baseDeDatos));
    }

    [Fact]
    public async Task RegistrarMovimiento_SiOtraPersonaCambiaElStockEnElMedio_SeAplicaSobreElValorNuevo()
    {
        var baseDeDatos = CrearBase();
        // Una entrada de 5 kg mientras otra persona carga una entrada de 3
        var interceptor = new OtraPersonaCambiaElStock(() => CambiarStockAsync(baseDeDatos, 3m));
        using var contexto = new ApplicationDbContext(Opciones(baseDeDatos).AddInterceptors(interceptor).Options);
        var servicio = new StockService(contexto);

        var error = await servicio.RegistrarMovimientoAsync(
            new MovimientoFormViewModel { IdIngrediente = Harina, Tipo = TipoMovimiento.Entrada, Cantidad = 5m }, "u1");

        Assert.Null(error);
        // 10 + 3 + 5. Sin el control quedaban 15 y la entrada de 3 se perdía.
        Assert.Equal(18m, await StockDeHarinaAsync(baseDeDatos));
    }
}
