using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecetarioMVC.Controllers;
using RecetarioMVC.Models;

namespace RecetarioMVC.Tests;

// Un 404 mostraba una hoja en blanco y un error del servidor, la plantilla en inglés
// del proyecto nuevo. Ahora los dos usan la misma página, con un texto para cada caso.
public class PaginasDeErrorTests
{
    private static HomeController CrearControlador() => new(null!)
    {
        ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() }
    };

    [Fact]
    public void PaginaNoEncontrada_ExplicaQueLaDireccionNoExiste()
    {
        var vista = Assert.IsType<ViewResult>(CrearControlador().Estado(404));

        Assert.Equal("Error", vista.ViewName);
        var modelo = Assert.IsType<ErrorViewModel>(vista.Model);
        Assert.Equal(404, modelo.Codigo);
        Assert.Equal("No encontramos esa página", modelo.Titulo);
        Assert.False(modelo.ShowRequestId);
    }

    [Fact]
    public void FormularioVencido_PideRecargarLaPantalla()
    {
        var modelo = (ErrorViewModel)Assert.IsType<ViewResult>(CrearControlador().Estado(400)).Model!;

        Assert.Equal(400, modelo.Codigo);
        Assert.Contains("recargala", modelo.Mensaje);
    }

    [Fact]
    public void CodigoSinTextoPropio_MuestraElMensajeGeneralConSuCodigo()
    {
        var modelo = (ErrorViewModel)Assert.IsType<ViewResult>(CrearControlador().Estado(405)).Model!;

        Assert.Equal(405, modelo.Codigo);
        Assert.Equal("Algo salió mal", modelo.Titulo);
    }

    [Fact]
    public void ErrorDelServidor_TraeElCodigoParaRastrearlo()
    {
        var vista = Assert.IsType<ViewResult>(CrearControlador().Error());

        var modelo = Assert.IsType<ErrorViewModel>(vista.Model);
        Assert.Equal(500, modelo.Codigo);
        Assert.Equal("Algo salió mal", modelo.Titulo);
        Assert.True(modelo.ShowRequestId);
    }
}
