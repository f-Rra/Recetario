using System;
using System.Windows.Forms;
using Dominio;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion
{
    public partial class frmLogin : Form
    {
        private readonly UsuarioNegocio _usuarioNegocio = new UsuarioNegocio();

        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnIngresar_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MensajesUI.MostrarAdvertencia("Ingresá email y contraseña.");
                return;
            }

            try
            {
                Usuario usuario = _usuarioNegocio.ValidarUsuario(email, password);

                if (usuario == null)
                {
                    MensajesUI.MostrarAdvertencia("Email o contraseña incorrectos.");
                    return;
                }

                frmPrincipal principal = new frmPrincipal(usuario);
                this.Hide();
                principal.FormClosed += (s, args) => this.Close();
                principal.Show();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }
    }
}
