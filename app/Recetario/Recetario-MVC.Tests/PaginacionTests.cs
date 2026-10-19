using Microsoft.EntityFrameworkCore;
using RecetarioMVC.Data;
using RecetarioMVC.Models;
using RecetarioMVC.Services;

namespace RecetarioMVC.Tests;

public class PaginacionTests
{
    /// <summary>30 ingredientes numerados, para ver dónde corta cada página.</summary>
    private static ApplicationDbContext CrearContexto(int cuantos = 30)
    {
        var opciones = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new ApplicationDbContext(opciones);
        var kg = new Unidad { IdUnidad = 1, Nombre = "Kilogramo", Abreviatura = "kg" };
        context.Add(kg);

        for (var i = 1; i <= cuantos; i++)
        {
            context.Ingredientes.Add(new Ingrediente
            {
                IdIngrediente = i,
                Codigo = $"ING{i:000}",
                // El nombre ordena igual que el id: así se puede predecir el corte
                Descripcion = $"Ingrediente {i:000}",
                IdUnidad = 1,
                Unidad = kg,
                StockActual = 10m,
                StockMinimo = 1m
            });
        }

        context.SaveChanges();
        return context;
    }

    [Fact]
    public async Task PrimeraPagina_TraeElTamanoYElTotalCompleto()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        var pagina = await servicio.ListarPaginadoAsync(null, 1);

        Assert.Equal(25, pagina.Items.Count);
        Assert.Equal(30, pagina.Total);      // el total es de todo, no de la página
        Assert.Equal(2, pagina.TotalPaginas);
        Assert.Equal(1, pagina.Desde);
        Assert.Equal(25, pagina.Hasta);
        Assert.False(pagina.HayAnterior);
        Assert.True(pagina.HaySiguiente);
    }

    [Fact]
    public async Task LasPaginas_NoRepitenNiSalteanFilas()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        var primera = await servicio.ListarPaginadoAsync(null, 1);
        var segunda = await servicio.ListarPaginadoAsync(null, 2);

        var todos = primera.Items.Concat(segunda.Items).Select(i => i.IdIngrediente).ToList();

        Assert.Equal(30, todos.Count);
        Assert.Equal(30, todos.Distinct().Count());          // ninguno repetido
        Assert.Equal(Enumerable.Range(1, 30), todos);        // ninguno salteado
    }

    [Fact]
    public async Task UltimaPagina_TraeSoloElResto()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        var segunda = await servicio.ListarPaginadoAsync(null, 2);

        Assert.Equal(5, segunda.Items.Count);
        Assert.Equal(26, segunda.Desde);
        Assert.Equal(30, segunda.Hasta);
        Assert.True(segunda.HayAnterior);
        Assert.False(segunda.HaySiguiente);
    }

    [Fact]
    public async Task PaginaFueraDeRango_CaeEnLaUltimaQueExiste()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        // Un enlace viejo, o filtrar estando en la página 5
        var lejos = await servicio.ListarPaginadoAsync(null, 99);
        var negativa = await servicio.ListarPaginadoAsync(null, -3);

        Assert.Equal(2, lejos.Pagina);
        Assert.NotEmpty(lejos.Items);
        Assert.Equal(1, negativa.Pagina);
    }

    [Fact]
    public async Task ElFiltro_CambiaElTotalYLasPaginas()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        // Los nombres van de "Ingrediente 001" a "Ingrediente 030":
        // "Ingrediente 01" matchea del 010 al 019, diez en total
        var filtrada = await servicio.ListarPaginadoAsync("Ingrediente 01", 1);

        Assert.Equal(10, filtrada.Total);
        Assert.Equal(1, filtrada.TotalPaginas);
        Assert.False(filtrada.HaySiguiente);
    }

    [Fact]
    public async Task SinResultados_NoRompeNiInventaPaginas()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        var vacia = await servicio.ListarPaginadoAsync("no existe", 1);

        Assert.Empty(vacia.Items);
        Assert.Equal(0, vacia.Total);
        Assert.Equal(1, vacia.TotalPaginas);
        Assert.Equal(0, vacia.Desde);
        Assert.Equal(0, vacia.Hasta);
    }

    [Fact]
    public async Task SinPaginar_SigueTrayendoTodo()
    {
        using var context = CrearContexto();
        var servicio = new IngredienteService(context, RelojDePrueba.Cualquiera());

        // El stock muestra los dos depósitos a la vez: necesita la lista entera
        var todos = await servicio.ListarAsync(null);

        Assert.Equal(30, todos.Count);
    }
}
