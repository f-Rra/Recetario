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
        private readonly ComandaNegocio _comandaNegocio = new ComandaNegocio();
        private readonly PersonaNegocio _personaNegocio = new PersonaNegocio();
        private readonly ProcedimientoNegocio _procedimientoNegocio = new ProcedimientoNegocio();

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

        private List<Receta> ObtenerRecetasSeleccionadas()
        {
            List<Receta> seleccionadas = new List<Receta>();
            foreach (DataGridViewRow fila in dgvRecetas.Rows)
            {
                bool marcado = Convert.ToBoolean(fila.Cells["colSeleccion"].Value ?? false);
                if (marcado && fila.DataBoundItem is Receta receta)
                {
                    seleccionadas.Add(receta);
                }
            }
            return seleccionadas;
        }

        private string ObtenerNombreResponsable(int idClasificacion)
        {
            Persona responsable = _personaNegocio.ObtenerResponsablePorClasificacion(idClasificacion);
            return responsable != null ? responsable.NombreCompleto : "Sin asignar";
        }

        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            List<Receta> seleccionadas = ObtenerRecetasSeleccionadas();

            if (seleccionadas.Count == 0)
            {
                MensajesUI.MostrarAdvertencia("Seleccioná al menos una receta.");
                return;
            }

            int comensales = (int)nudComensales.Value;

            if (!MensajesUI.MostrarConfirmacion($"¿Registrar {seleccionadas.Count} comanda(s) para {comensales} comensales?"))
            {
                return;
            }

            try
            {
                foreach (Receta receta in seleccionadas)
                {
                    _comandaNegocio.RegistrarComanda(receta.IdReceta, comensales, _usuario.IdUsuario);
                }

                MensajesUI.MostrarExito("Comanda(s) registrada(s) correctamente.");
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            List<Receta> seleccionadas = ObtenerRecetasSeleccionadas();

            if (seleccionadas.Count == 0)
            {
                MensajesUI.MostrarAdvertencia("Seleccioná al menos una receta.");
                return;
            }

            int comensales = (int)nudComensales.Value;

            try
            {
                List<SeccionComanda> secciones = new List<SeccionComanda>();
                foreach (Receta receta in seleccionadas)
                {
                    secciones.Add(new SeccionComanda
                    {
                        NombreReceta = receta.Nombre,
                        Sector = receta.NombreClasificacion,
                        Responsable = ObtenerNombreResponsable(receta.IdClasificacion),
                        Ingredientes = _comandaNegocio.AjustarReceta(receta.IdReceta, comensales),
                        Procedimientos = _procedimientoNegocio.ListarPorReceta(receta.IdReceta)
                    });
                }

                using (SaveFileDialog dialogo = new SaveFileDialog())
                {
                    dialogo.Filter = "Archivos PDF (*.pdf)|*.pdf";
                    dialogo.FileName = $"Comanda_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

                    if (dialogo.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    GeneradorPDF.GenerarComanda(dialogo.FileName, comensales, secciones);
                    MensajesUI.MostrarExito("Comanda generada correctamente.");
                    System.Diagnostics.Process.Start(dialogo.FileName);
                }
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
            catch (Exception ex)
            {
                MensajesUI.ManejarExcepcion(ex, "generar el PDF de la comanda");
            }
        }
    }
}
