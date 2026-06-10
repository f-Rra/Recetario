using System.Windows.Forms;
using Dominio;

namespace Presentacion.UserControls
{
    public partial class ucDashboardCocina : UserControl
    {
        private readonly Usuario _usuario;

        public ucDashboardCocina(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
        }
    }
}
