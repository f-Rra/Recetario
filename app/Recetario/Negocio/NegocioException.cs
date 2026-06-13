using System;
using System.Data.SqlClient;

namespace Negocio
{
    public class NegocioException : Exception
    {
        public string Operacion { get; }

        public NegocioException(string mensaje)
            : base(mensaje)
        {
        }

        public NegocioException(string mensaje, string operacion)
            : base(mensaje)
        {
            Operacion = operacion;
        }

        public NegocioException(string mensaje, Exception innerException)
            : base(mensaje, innerException)
        {
        }

        public NegocioException(string mensaje, string operacion, Exception innerException)
            : base(mensaje, innerException)
        {
            Operacion = operacion;
            LogError();
        }

        public static NegocioException FromDbException(Exception ex, string operacion)
        {
            string mensajeAmigable = ObtenerMensajeAmigable(ex);
            return new NegocioException(mensajeAmigable, operacion, ex);
        }

        private static string ObtenerMensajeAmigable(Exception ex)
        {
            if (ex is SqlException sql && sql.Number == 50000)
            {
                return ex.Message;
            }

            string mensaje = ex.Message.ToLower();

            if (mensaje.Contains("timeout"))
                return "La operación tardó demasiado tiempo. Intente nuevamente.";

            if (mensaje.Contains("connection") || mensaje.Contains("network"))
                return "No se pudo conectar a la base de datos. Verifique la conexión.";

            if (mensaje.Contains("unique") || mensaje.Contains("duplicate"))
                return "Ya existe un registro con esos datos.";

            if (mensaje.Contains("foreign key") || mensaje.Contains("reference"))
                return "No se puede completar la operación por registros relacionados.";

            if (mensaje.Contains("login failed"))
                return "Error de autenticación con la base de datos.";

            return "Ocurrió un error inesperado. Contacte al administrador.";
        }

        private void LogError()
        {
            System.Diagnostics.Debug.WriteLine($"[NEGOCIO ERROR] Operación: {Operacion}");
            System.Diagnostics.Debug.WriteLine($"[NEGOCIO ERROR] Mensaje: {Message}");
            if (InnerException != null)
            {
                System.Diagnostics.Debug.WriteLine($"[NEGOCIO ERROR] Detalle: {InnerException.Message}");
            }
        }
    }
}
