using System;
using System.Windows.Forms;

namespace Presentacion
{
    internal static class Program
    {
        // Punto de entrada de la aplicación.
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // El formulario de inicio (frmLogin) se agrega el 9/6.
            // Application.Run(new frmLogin());
        }
    }
}
