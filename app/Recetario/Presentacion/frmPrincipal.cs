using System;
using System.Windows.Forms;
using Dominio;
using Presentacion.Gestores;
using Presentacion.Helpers;
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
            ConfigurarMenu();
            CargarDashboard();
        }

        private void ConfigurarMenu()
        {
            bool esAdmin = _usuario.Rol == "admin";
            menuRecetas.Visible = esAdmin;
            menuIngredientes.Visible = esAdmin;
            menuProveedores.Visible = esAdmin;
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

        private void menuInicio_Click(object sender, EventArgs e)
        {
            CargarDashboard();
        }

        private void menuIngredientes_Click(object sender, EventArgs e)
        {
            _navegacion.Mostrar(new ucIngredientes());
        }

        private void menuRecetas_Click(object sender, EventArgs e)
        {
            MensajesUI.MostrarInformacion("Próximamente.");
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            MensajesUI.MostrarInformacion("Próximamente.");
        }
    }
}
