using System.Windows.Forms;

namespace Presentacion.Gestores
{
    public class GestorNavegacion
    {
        private readonly Panel _contenedor;

        public GestorNavegacion(Panel contenedor)
        {
            _contenedor = contenedor;
        }

        public void Mostrar(UserControl vista)
        {
            _contenedor.SuspendLayout();
            _contenedor.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            _contenedor.Controls.Add(vista);
            vista.BringToFront();
            _contenedor.ResumeLayout();
        }
    }
}
