using System.Windows.Forms;
using Dominio;

namespace Presentacion
{
    public partial class frmPrincipal : Form
    {
        private readonly Usuario _usuario;

        public frmPrincipal(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            lblBienvenida.Text = $"Bienvenido/a {_usuario.Persona.NombreCompleto}  ({_usuario.Rol})";
        }
    }
}
