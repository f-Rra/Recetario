using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using RecetarioMVC.Controllers;

namespace RecetarioMVC.Tests;

// Después de ingresar, el usuario vuelve a la página que quería abrir. Esa dirección viaja
// en la URL del inicio de sesión, así que puede traer cualquier cosa.
public class DireccionDeRetornoTests
{
    // UrlHelperBase trae la regla real de "es una dirección de esta aplicación";
    // armar direcciones a partir de rutas no hace falta para estos tests
    private sealed class UrlDePrueba(ActionContext contexto) : UrlHelperBase(contexto)
    {
        public override string? Action(UrlActionContext actionContext) => "/";
        public override string? RouteUrl(UrlRouteContext routeContext) => "/";
    }

    private static AccesoController CrearControlador()
    {
        var contexto = new ActionContext(new DefaultHttpContext(), new RouteData(), new ActionDescriptor());
        return new AccesoController(null!, null!) { Url = new UrlDePrueba(contexto) };
    }

    [Theory]
    [InlineData("/Comandas/Comandera")]
    [InlineData("/Stock?estado=critico")]
    [InlineData("/")]
    public void DireccionDeLaAplicacion_VuelveAEsaPagina(string returnUrl)
    {
        var resultado = CrearControlador().RedirigirTrasIngresar(returnUrl);

        Assert.Equal(returnUrl, Assert.IsType<LocalRedirectResult>(resultado).Url);
    }

    [Theory]
    [InlineData("https://ejemplo.invalid/")]
    [InlineData("//ejemplo.invalid/robar")]
    [InlineData("/\ejemplo.invalid")]
    [InlineData("javascript:alert(1)")]
    [InlineData("")]
    [InlineData(null)]
    public void DireccionExternaOVacia_VaAlInicio(string? returnUrl)
    {
        var resultado = CrearControlador().RedirigirTrasIngresar(returnUrl);

        var redireccion = Assert.IsType<RedirectToActionResult>(resultado);
        Assert.Equal("Index", redireccion.ActionName);
        Assert.Equal("Home", redireccion.ControllerName);
    }
}
