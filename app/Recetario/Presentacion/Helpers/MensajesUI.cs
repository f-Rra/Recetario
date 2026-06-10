using System;
using System.Windows.Forms;
using Negocio;

namespace Presentacion.Helpers
{
    public static class MensajesUI
    {
        public static void MostrarError(string mensaje)
        {
            MessageBox.Show(mensaje, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MostrarError(string mensaje, Exception ex)
        {
            string mensajeCompleto = mensaje;

            if (ex != null)
            {
                mensajeCompleto += $"\n\nDetalle técnico: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"[UI ERROR] {mensaje}");
                System.Diagnostics.Debug.WriteLine($"[UI EXCEPTION] {ex}");
            }

            MessageBox.Show(mensajeCompleto, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MostrarAdvertencia(string mensaje)
        {
            MessageBox.Show(mensaje, "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void MostrarInformacion(string mensaje)
        {
            MessageBox.Show(mensaje, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void MostrarExito(string mensaje)
        {
            MessageBox.Show(mensaje, "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool MostrarConfirmacion(string mensaje)
        {
            DialogResult resultado = MessageBox.Show(
                mensaje,
                "Confirmación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            return resultado == DialogResult.Yes;
        }

        public static void ManejarExcepcion(NegocioException ex)
        {
            string mensaje = ex.Message;

            if (!string.IsNullOrEmpty(ex.Operacion))
            {
                mensaje = $"Error al {ex.Operacion}.\n\n{ex.Message}";
            }

            MostrarError(mensaje);
        }

        public static void ManejarExcepcion(Exception ex, string operacion)
        {
            System.Diagnostics.Debug.WriteLine($"[UI ERROR] {operacion}");
            System.Diagnostics.Debug.WriteLine($"[UI EXCEPTION] {ex}");

            MostrarError($"Error al {operacion}.", ex);
        }
    }
}
