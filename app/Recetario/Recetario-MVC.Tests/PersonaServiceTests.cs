using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Tests;

public class PersonaServiceTests
{
    private const int Entrada = 1;
    private const int Postre = 2;   // queda sin responsable
    private const int Ensalada = 3;

    private static ApplicationDbContext CrearContexto()
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);

        context.AddRange(
            new Clasificacion { IdClasificacion = Entrada, Nombre = "Entrada" },
            new Clasificacion { IdClasificacion = Postre, Nombre = "Postre" },
            new Clasificacion { IdClasificacion = Ensalada, Nombre = "Ensalada" },
            new Persona { IdPersona = 1, Nombre = "Juan", Apellido = "Pérez", IdClasificacion = Entrada },
            new Persona { IdPersona = 2, Nombre = "Ana", Apellido = "Ruiz", IdClasificacion = Entrada },
            new Persona { IdPersona = 3, Nombre = "María", Apellido = "López", IdClasificacion = Ensalada },
            // Cargada sin sector: no se le asigna ninguna comanda
            new Persona { IdPersona = 4, Nombre = "Carlos", Apellido = "Gómez", IdClasificacion = null });

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task PorSector_AgrupaCadaResponsableEnElSuyo()
    {
        using var context = CrearContexto();
        var servicio = new PersonaService(context);

        var datos = await servicio.ListarPorSectorAsync();

        var entrada = datos.Sectores.Single(s => s.IdClasificacion == Entrada);
        var ensalada = datos.Sectores.Single(s => s.IdClasificacion == Ensalada);

        // Ordenados por apellido: López antes que Pérez, Pérez antes que Ruiz
        Assert.Equal(new[] { "Juan Pérez", "Ana Ruiz" },
            entrada.Responsables.Select(r => r.NombreCompleto));
        Assert.Equal("María López", Assert.Single(ensalada.Responsables).NombreCompleto);
    }

    [Fact]
    public async Task PorSector_TraeTambienLosSectoresVacios()
    {
        using var context = CrearContexto();
        var servicio = new PersonaService(context);

        var datos = await servicio.ListarPorSectorAsync();

        var postre = datos.Sectores.Single(s => s.IdClasificacion == Postre);
        Assert.True(postre.SinCubrir);
        Assert.Equal(1, datos.SectoresSinCubrir);
        Assert.Equal(3, datos.Sectores.Count); // están los tres, no solo los cubiertos
    }

    [Fact]
    public async Task PorSector_SeparaALosQueNoTienenSector()
    {
        using var context = CrearContexto();
        var servicio = new PersonaService(context);

        var datos = await servicio.ListarPorSectorAsync();

        Assert.Equal("Carlos Gómez", Assert.Single(datos.SinSector).NombreCompleto);
        // El total los cuenta igual: son responsables cargados
        Assert.Equal(4, datos.Total);
    }

    [Fact]
    public async Task PorSector_CuentaLasComandasDeCadaUno()
    {
        using var context = CrearContexto();
        context.AddRange(
            new Receta { IdReceta = 1, Codigo = "REC001", Nombre = "Sopa", IdClasificacion = Entrada, PorcionesBase = 10 },
            new ApplicationUser { Id = "u1", Nombre = "Admin", Apellido = "Test" });
        context.SaveChanges();
        context.Comandas.AddRange(
            new Comanda { IdComanda = 1, IdReceta = 1, IdPersona = 1, UsuarioId = "u1", Fecha = new DateOnly(2026, 8, 1), Porciones = 10 },
            new Comanda { IdComanda = 2, IdReceta = 1, IdPersona = 1, UsuarioId = "u1", Fecha = new DateOnly(2026, 8, 2), Porciones = 10 });
        context.SaveChanges();
        var servicio = new PersonaService(context);

        var datos = await servicio.ListarPorSectorAsync();

        var entrada = datos.Sectores.Single(s => s.IdClasificacion == Entrada);
        Assert.Equal(2, entrada.Responsables.Single(r => r.IdPersona == 1).CantidadComandas);
        Assert.Equal(0, entrada.Responsables.Single(r => r.IdPersona == 2).CantidadComandas);
    }
}
