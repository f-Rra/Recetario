using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Tests;

// Dos personas dando de alta a la vez calculan el mismo código (el más alto + 1). La base
// rechaza a la segunda por el índice único, y eso terminaba en un error. Como en
// StockConcurrenciaTests, la carrera se arma a mano: justo antes del guardado, "otra
// persona" crea un registro con ese mismo código y el guardado falla, que es lo que haría
// SQL Server (la base en memoria no aplica el índice único).
public class CodigoRepetidoTests
{
    private sealed class OtraPersonaSeLlevaElCodigo : SaveChangesInterceptor
    {
        private readonly Func<string, Task> _crearConEseCodigo;
        private readonly bool _repetirElCodigo;
        public int Choques { get; private set; }

        public OtraPersonaSeLlevaElCodigo(Func<string, Task> crearConEseCodigo, bool repetirElCodigo = true)
        {
            _crearConEseCodigo = crearConEseCodigo;
            _repetirElCodigo = repetirElCodigo;
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            var nuevo = eventData.Context!.ChangeTracker.Entries()
                .FirstOrDefault(e => e.State == EntityState.Added && e.Entity is Receta or Ingrediente);

            if (Choques == 0 && nuevo is not null)
            {
                Choques++;
                if (_repetirElCodigo)
                    await _crearConEseCodigo((string)nuevo.Property("Codigo").CurrentValue!);
                throw new DbUpdateException("La base rechazó el guardado.");
            }

            return result;
        }
    }

    private static DbContextOptionsBuilder<ApplicationDbContext> Opciones(string baseDeDatos) =>
        new DbContextOptionsBuilder<ApplicationDbContext>().UseInMemoryDatabase(baseDeDatos);

    // Una base con REC001 e ING001 ya cargados: el próximo código de cada uno es el 002
    private static string CrearBase()
    {
        var nombre = Guid.NewGuid().ToString();
        using var context = new ApplicationDbContext(Opciones(nombre).Options);
        context.AddRange(
            new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" },
            new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" },
            new ApplicationUser { Id = "u1", Nombre = "Admin", Apellido = "Test" },
            new Receta { Codigo = "REC001", Nombre = "Ñoquis", IdClasificacion = 1, PorcionesBase = 6, Activo = true },
            new Ingrediente { Codigo = "ING001", Descripcion = "Harina", IdUnidad = 1 });
        context.SaveChanges();
        return nombre;
    }

    private static RecetaFormViewModel NuevaReceta() => new()
    {
        Nombre = "Milanesas", IdClasificacion = 1, PorcionesBase = 4, Activo = true
    };

    private static IngredienteFormViewModel NuevoIngrediente() => new()
    {
        Descripcion = "Azúcar", IdUnidad = 1, Deposito = Deposito.Bodega
    };

    [Fact]
    public async Task CrearReceta_SiOtraPersonaSeLlevaElCodigo_UsaElSiguiente()
    {
        var baseDeDatos = CrearBase();
        var otraPersona = new OtraPersonaSeLlevaElCodigo(async codigo =>
        {
            using var otro = new ApplicationDbContext(Opciones(baseDeDatos).Options);
            otro.Recetas.Add(new Receta { Codigo = codigo, Nombre = "Tarta", IdClasificacion = 1, PorcionesBase = 8, Activo = true });
            await otro.SaveChangesAsync();
        });
        using var context = new ApplicationDbContext(Opciones(baseDeDatos).AddInterceptors(otraPersona).Options);
        var servicio = new RecetaService(context);

        var id = await servicio.CrearAsync(NuevaReceta());

        using var lectura = new ApplicationDbContext(Opciones(baseDeDatos).Options);
        var recetas = await lectura.Recetas.OrderBy(r => r.Codigo).ToListAsync();
        Assert.Equal(new[] { "REC001", "REC002", "REC003" }, recetas.Select(r => r.Codigo));
        Assert.Equal("Tarta", recetas[1].Nombre);        // la de la otra persona se quedó con el 002
        Assert.Equal("Milanesas", recetas[2].Nombre);    // esta pasó al 003
        Assert.Equal(id, recetas[2].IdReceta);
    }

    [Fact]
    public async Task CrearIngrediente_SiOtraPersonaSeLlevaElCodigo_UsaElSiguiente()
    {
        var baseDeDatos = CrearBase();
        var otraPersona = new OtraPersonaSeLlevaElCodigo(async codigo =>
        {
            using var otro = new ApplicationDbContext(Opciones(baseDeDatos).Options);
            otro.Ingredientes.Add(new Ingrediente { Codigo = codigo, Descripcion = "Sal", IdUnidad = 1 });
            await otro.SaveChangesAsync();
        });
        using var context = new ApplicationDbContext(Opciones(baseDeDatos).AddInterceptors(otraPersona).Options);
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        await servicio.CrearAsync(NuevoIngrediente(), "u1");

        using var lectura = new ApplicationDbContext(Opciones(baseDeDatos).Options);
        var ingredientes = await lectura.Ingredientes.OrderBy(i => i.Codigo).ToListAsync();
        Assert.Equal(new[] { "ING001", "ING002", "ING003" }, ingredientes.Select(i => i.Codigo));
        Assert.Equal("Sal", ingredientes[1].Descripcion);
        Assert.Equal("Azúcar", ingredientes[2].Descripcion);
    }

    [Fact]
    public async Task CrearReceta_SiElGuardadoFallaPorOtroMotivo_NoReintenta()
    {
        var baseDeDatos = CrearBase();
        // El guardado falla, pero nadie usó el código: no es una carrera
        var falla = new OtraPersonaSeLlevaElCodigo(_ => Task.CompletedTask, repetirElCodigo: false);
        using var context = new ApplicationDbContext(Opciones(baseDeDatos).AddInterceptors(falla).Options);
        var servicio = new RecetaService(context);

        await Assert.ThrowsAsync<DbUpdateException>(() => servicio.CrearAsync(NuevaReceta()));

        Assert.Equal(1, falla.Choques);
        using var lectura = new ApplicationDbContext(Opciones(baseDeDatos).Options);
        Assert.Equal("REC001", (await lectura.Recetas.SingleAsync()).Codigo);
    }
}
