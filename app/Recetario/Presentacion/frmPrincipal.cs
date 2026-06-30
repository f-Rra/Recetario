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
        #region Campos y constructor

        private readonly Usuario _usuario;
        private readonly GestorNavegacion _navegacion;

        public frmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            _navegacion = new GestorNavegacion(pnlContenido);
            lblBienvenida.Text = $"{_usuario.Persona.NombreCompleto}  ({RolDisplay(_usuario.Rol)})";
            ConfigurarMenu();
            CargarDashboard();
        }

        #endregion

        private static string RolDisplay(string rol)
        {
            switch (rol)
            {
                case "lider": return "Cocina";
                case "admin": return "Admin";
                default:      return rol;
            }
        }

        #region Navegación

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

        private void ConfigurarMenu()
        {
            // El menú superior solo tiene sentido para el admin (ABMs + volver al panel).
            // Cocina trabaja en un único dashboard, así que no ve menú.
            bool esAdmin = _usuario.Rol == "admin";
            menuInicio.Visible = esAdmin;
            menuRecetas.Visible = esAdmin;
            menuIngredientes.Visible = esAdmin;
            menuProveedores.Visible = esAdmin;
        }

        #endregion

        #region Menú

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
            _navegacion.Mostrar(new ucRecetas());
        }

        private void menuProveedores_Click(object sender, EventArgs e)
        {
            _navegacion.Mostrar(new ucProveedores());
        }

        #endregion
    }
}
