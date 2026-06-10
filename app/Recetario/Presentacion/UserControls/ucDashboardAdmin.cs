using System.Windows.Forms;
using Dominio;

namespace Presentacion.UserControls
{
    public partial class ucDashboardAdmin : UserControl
    {
        private readonly Usuario _usuario;

        public ucDashboardAdmin(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
        }
    }
}
