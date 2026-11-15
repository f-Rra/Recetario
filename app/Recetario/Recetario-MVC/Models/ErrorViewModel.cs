namespace RecetarioMVC.Models;

/// <summary>Lo que muestra la página de error: qué pasó y, si hay, un código para rastrearlo.</summary>
public class ErrorViewModel
{
    /// <summary>Código HTTP de la respuesta (404, 500…).</summary>
    public int Codigo { get; set; }

    public string Titulo { get; set; } = string.Empty;
    public string Mensaje { get; set; } = string.Empty;

    /// <summary>Identificador del pedido, para buscarlo en el registro del servidor.</summary>
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

    public static ErrorViewModel Para(int codigo, string? requestId = null) => codigo switch
    {
        404 => new ErrorViewModel
        {
            Codigo = 404,
            Titulo = "No encontramos esa página",
            Mensaje = "La dirección no existe o el registro que buscabas ya no está."
        },
        400 => new ErrorViewModel
        {
            Codigo = 400,
            Titulo = "No se pudo procesar el pedido",
            Mensaje = "El formulario venció o llegó incompleto. Volvé a la pantalla anterior, recargala y probá de nuevo."
        },
        _ => new ErrorViewModel
        {
            Codigo = codigo,
            Titulo = "Algo salió mal",
            Mensaje = "Ocurrió un error inesperado y no se guardó ningún cambio a medias. Probá de nuevo en un momento.",
            RequestId = requestId
        }
    };
}
