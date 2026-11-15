using RecetarioMVC.Helpers;
using RecetarioMVC.Models;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Tests;

// Las modificaciones de una misma receta no se pueden contradecir entre sí.
// Receta de ejemplo: lleva Harina (1) y Leche (2); Manteca (3) y Crema (4) no son de la receta.
public class ReglasModificacionTests
{
    private const int Harina = 1, Leche = 2, Manteca = 3, Crema = 4;

    private static ModificacionCarrito Quitar(int id, string nombre) => new()
    {
        Tipo = TipoModificacion.Eliminacion,
        IdIngredienteOriginal = id,
        NombreOriginal = nombre
    };

    private static ModificacionCarrito Sustituir(int id, string nombre, int idNuevo, string nombreNuevo) => new()
    {
        Tipo = TipoModificacion.Sustitucion,
        IdIngredienteOriginal = id,
        NombreOriginal = nombre,
        IdIngredienteReemplazo = idNuevo,
        NombreReemplazo = nombreNuevo
    };

    private static ModificacionCarrito Agregar(int id, string nombre) => new()
    {
        Tipo = TipoModificacion.Adicion,
        IdIngredienteReemplazo = id,
        NombreReemplazo = nombre,
        Cantidad = 0.5m
    };

    [Fact]
    public void SinModificacionesPrevias_CualquieraEsValida()
    {
        var ninguna = new List<ModificacionCarrito>();

        Assert.Null(ReglasModificacion.Validar(ninguna, Quitar(Harina, "Harina")));
        Assert.Null(ReglasModificacion.Validar(ninguna, Sustituir(Leche, "Leche", Crema, "Crema")));
        Assert.Null(ReglasModificacion.Validar(ninguna, Agregar(Manteca, "Manteca")));
    }

    [Fact]
    public void ModificacionesSobreIngredientesDistintos_Conviven()
    {
        var existentes = new List<ModificacionCarrito> { Quitar(Harina, "Harina"), Agregar(Manteca, "Manteca") };

        Assert.Null(ReglasModificacion.Validar(existentes, Sustituir(Leche, "Leche", Crema, "Crema")));
    }

    [Fact]
    public void SustituirUnIngredienteYaQuitado_SeRechaza()
    {
        var existentes = new List<ModificacionCarrito> { Quitar(Harina, "Harina") };

        var error = ReglasModificacion.Validar(existentes, Sustituir(Harina, "Harina", Manteca, "Manteca"));

        Assert.NotNull(error);
        Assert.Contains("Harina", error);
    }

    [Fact]
    public void QuitarUnIngredienteYaSustituido_SeRechaza()
    {
        var existentes = new List<ModificacionCarrito> { Sustituir(Leche, "Leche", Crema, "Crema") };

        Assert.NotNull(ReglasModificacion.Validar(existentes, Quitar(Leche, "Leche")));
    }

    [Fact]
    public void RepetirLaMismaModificacion_SeRechaza()
    {
        var existentes = new List<ModificacionCarrito> { Quitar(Harina, "Harina") };

        Assert.NotNull(ReglasModificacion.Validar(existentes, Quitar(Harina, "Harina")));
    }

    [Fact]
    public void SustituirUnIngredientePorSiMismo_SeRechaza()
    {
        var error = ReglasModificacion.Validar(
            new List<ModificacionCarrito>(), Sustituir(Harina, "Harina", Harina, "Harina"));

        Assert.NotNull(error);
        Assert.Contains("sí mismo", error);
    }

    [Fact]
    public void AgregarUnIngredienteYaQuitado_SeRechaza()
    {
        var existentes = new List<ModificacionCarrito> { Quitar(Harina, "Harina") };

        Assert.NotNull(ReglasModificacion.Validar(existentes, Agregar(Harina, "Harina")));
    }

    [Fact]
    public void QuitarUnIngredienteYaAgregado_SeRechaza()
    {
        var existentes = new List<ModificacionCarrito> { Agregar(Harina, "Harina") };

        Assert.NotNull(ReglasModificacion.Validar(existentes, Quitar(Harina, "Harina")));
        Assert.NotNull(ReglasModificacion.Validar(existentes, Sustituir(Harina, "Harina", Manteca, "Manteca")));
    }

    [Fact]
    public void AgregarElIngredienteQueEntroComoReemplazo_EsValido()
    {
        // Leche por Crema, y además un poco más de Crema: no se contradicen
        var existentes = new List<ModificacionCarrito> { Sustituir(Leche, "Leche", Crema, "Crema") };

        Assert.Null(ReglasModificacion.Validar(existentes, Agregar(Crema, "Crema")));
    }
}
