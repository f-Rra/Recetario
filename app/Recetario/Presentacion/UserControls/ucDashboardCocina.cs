using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dominio;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.UserControls
{
    public partial class ucDashboardCocina : UserControl
    {
        private readonly Usuario _usuario;
        private readonly RecetaNegocio _recetaNegocio = new RecetaNegocio();
        private readonly ClasificacionNegocio _clasificacionNegocio = new ClasificacionNegocio();

        public ucDashboardCocina(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            dgvRecetas.AutoGenerateColumns = false;
            CargarClasificaciones();
        }

        private void CargarClasificaciones()
        {
            try
            {
                List<Clasificacion> clasificaciones = _clasificacionNegocio.Listar();
                clasificaciones.Insert(0, new Clasificacion { IdClasificacion = 0, Nombre = "Todas" });

                cboClasificacion.DisplayMember = "Nombre";
                cboClasificacion.ValueMember = "IdClasificacion";
                cboClasificacion.DataSource = clasificaciones;
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void cboClasificacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarRecetas();
        }

        private void CargarRecetas()
        {
            if (!(cboClasificacion.SelectedValue is int idClasificacion))
            {
                return;
            }

            try
            {
                List<Receta> recetas = idClasificacion == 0
                    ? _recetaNegocio.Listar()
                    : _recetaNegocio.ListarPorClasificacion(idClasificacion);

                dgvRecetas.DataSource = recetas;
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }
    }
}
