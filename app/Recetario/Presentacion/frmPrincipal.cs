using System.Windows.Forms;
using Dominio;
using Presentacion.Gestores;
using Presentacion.UserControls;

namespace Presentacion
{
    public partial class frmPrincipal : Form
    {
        private readonly Usuario _usuario;
        private readonly GestorNavegacion _navegacion;

        public frmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            _navegacion = new GestorNavegacion(pnlContenido);
            lblBienvenida.Text = $"Bienvenido/a {_usuario.Persona.NombreCompleto}  ({_usuario.Rol})";
            CargarDashboard();
        }

        private void CargarDashboard()
        {
            if (_usuario.Rol == "admin")
            {
                _navegacion.Mostrar(new ucDashboardAdmin(_usuario));
            }
            else
            {
                _navegacion.Mostrar(new ucDashboardCocina(_usuario));
            }
        }
    }
}
