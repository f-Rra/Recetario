using System.Text.Json;
using RecetarioMVC.ViewModels;

namespace RecetarioMVC.Helpers;

/// <summary>
/// Guarda el pedido en curso en la sesión del servidor. Equivale al carrito
/// en memoria del WinForms: se pierde al cerrar sesión y no toca la base.
/// </summary>
public static class CarritoSesion
{
    private const string Clave = "ComanderaCarrito";
    public const int ComensalesPorDefecto = 1;

    public static CarritoComanda Obtener(ISession sesion)
    {
        var json = sesion.GetString(Clave);
        if (string.IsNullOrEmpty(json))
            return new CarritoComanda { Comensales = ComensalesPorDefecto };

        return JsonSerializer.Deserialize<CarritoComanda>(json)
               ?? new CarritoComanda { Comensales = ComensalesPorDefecto };
    }

    public static void Guardar(ISession sesion, CarritoComanda carrito) =>
        sesion.SetString(Clave, JsonSerializer.Serialize(carrito));

    public static void Vaciar(ISession sesion, int comensales) =>
        Guardar(sesion, new CarritoComanda { Comensales = comensales });
}
