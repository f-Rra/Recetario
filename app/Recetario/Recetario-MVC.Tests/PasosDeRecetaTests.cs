using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Tests;

// Quitar un paso borra uno y renumera el resto, en una transacción. La base en memoria no
// tiene transacciones, así que acá se comprueba el resultado; que un fallo a mitad de
// camino deshaga todo lo hace SQL Server.
public class PasosDeRecetaTests
{
    private const int Noquis = 1;
    private const int Tarta = 2;

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        var context = new ApplicationDbContext(opciones);
        var principal = new Clasificacion { IdClasificacion = 1, Nombre = "Plato Principal" };

        context.AddRange(
            principal,
            new Receta { IdReceta = Noquis, Codigo = "REC001", Nombre = "Ñoquis", IdClasificacion = 1, Clasificacion = principal, PorcionesBase = 6 },
            new Receta { IdReceta = Tarta, Codigo = "REC002", Nombre = "Tarta", IdClasificacion = 1, Clasificacion = principal, PorcionesBase = 8 });

        context.Procedimientos.AddRange(
            new Procedimiento { IdProcedimiento = 1, IdReceta = Noquis, NroPaso = 1, Descripcion = "Hervir las papas." },
            new Procedimiento { IdProcedimiento = 2, IdReceta = Noquis, NroPaso = 2, Descripcion = "Hacer el puré." },
            new Procedimiento { IdProcedimiento = 3, IdReceta = Noquis, NroPaso = 3, Descripcion = "Sumar la harina." },
            new Procedimiento { IdProcedimiento = 4, IdReceta = Noquis, NroPaso = 4, Descripcion = "Cortar y hervir." },
            new Procedimiento { IdProcedimiento = 5, IdReceta = Tarta, NroPaso = 1, Descripcion = "Forrar el molde." });

        context.SaveChanges();
        return context;
    }

    private static Task<List<(int Nro, string Descripcion)>> PasosAsync(ApplicationDbContext context, int idReceta) =>
        context.Procedimientos.AsNoTracking()
            .Where(p => p.IdReceta == idReceta)
            .OrderBy(p => p.NroPaso)
            .Select(p => ValueTuple.Create(p.NroPaso, p.Descripcion))
            .ToListAsync();

    [Fact]
    public async Task QuitarUnPasoDelMedio_RenumeraLosQueSiguenSinDejarHuecos()
    {
        using var context = CrearContexto();
        var servicio = new RecetaService(context);

        var quitado = await servicio.QuitarPasoAsync(idProcedimiento: 2, idReceta: Noquis);

        Assert.True(quitado);
        Assert.Equal(
            new[] { (1, "Hervir las papas."), (2, "Sumar la harina."), (3, "Cortar y hervir.") },
            await PasosAsync(context, Noquis));
    }

    [Fact]
    public async Task QuitarUnPaso_NoTocaLosPasosDeOtraReceta()
    {
        using var context = CrearContexto();
        var servicio = new RecetaService(context);

        await servicio.QuitarPasoAsync(idProcedimiento: 1, idReceta: Noquis);

        Assert.Equal(new[] { (1, "Forrar el molde.") }, await PasosAsync(context, Tarta));
    }

    [Fact]
    public async Task QuitarUnPasoQueEsDeOtraReceta_NoHaceNada()
    {
        using var context = CrearContexto();
        var servicio = new RecetaService(context);

        // El paso 5 es de la Tarta, pero se pide quitarlo desde los Ñoquis
        var quitado = await servicio.QuitarPasoAsync(idProcedimiento: 5, idReceta: Noquis);

        Assert.False(quitado);
        Assert.Single(await PasosAsync(context, Tarta));
        Assert.Equal(4, (await PasosAsync(context, Noquis)).Count);
    }
}
