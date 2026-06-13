using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly BindingList<ItemComanda> _comanda = new BindingList<ItemComanda>();

        public ucDashboardCocina(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            dgvRecetas.AutoGenerateColumns = false;
            dgvComanda.AutoGenerateColumns = false;
            dgvComanda.DataSource = _comanda;
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

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            List<Receta> seleccionadas = ObtenerRecetasSeleccionadas();

            if (seleccionadas.Count == 0)
            {
                MensajesUI.MostrarAdvertencia("Tildá al menos una receta del catálogo.");
                return;
            }

            foreach (Receta receta in seleccionadas)
            {
                if (!EstaEnComanda(receta.IdReceta))
                {
                    _comanda.Add(new ItemComanda { Receta = receta });
                }
            }

            foreach (DataGridViewRow fila in dgvRecetas.Rows)
            {
                fila.Cells["colSeleccion"].Value = false;
            }
        }

        private bool EstaEnComanda(int idReceta)
        {
            foreach (ItemComanda item in _comanda)
            {
                if (item.Receta.IdReceta == idReceta)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnQuitar_Click(object sender, EventArgs e)
        {
            if (dgvComanda.CurrentRow != null && dgvComanda.CurrentRow.DataBoundItem is ItemComanda item)
            {
                _comanda.Remove(item);
            }
            else
            {
                MensajesUI.MostrarAdvertencia("Seleccioná un ítem de la comanda.");
            }
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if (_comanda.Count == 0)
            {
                MensajesUI.MostrarAdvertencia("La comanda está vacía.");
                return;
            }

            int comensales = (int)nudComensales.Value;

            if (!MensajesUI.MostrarConfirmacion($"¿Generar la comanda con {_comanda.Count} receta(s) para {comensales} comensales?"))
            {
                return;
            }

            using (SaveFileDialog dialogo = new SaveFileDialog())
            {
                dialogo.Filter = "Archivos PDF (*.pdf)|*.pdf";
                dialogo.FileName = $"Comanda_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

                if (dialogo.ShowDialog() != DialogResult.OK)
                {
                    return;
                }

                try
                {
                    List<SeccionComanda> secciones = new List<SeccionComanda>();
                    foreach (ItemComanda item in _comanda)
                    {
                        _comandaNegocio.RegistrarComanda(item.Receta.IdReceta, comensales, _usuario.IdUsuario);

                        secciones.Add(new SeccionComanda
                        {
                            NombreReceta = item.Receta.Nombre,
                            Sector = item.Receta.NombreClasificacion,
                            Responsable = ObtenerNombreResponsable(item.Receta.IdClasificacion),
                            Ingredientes = _comandaNegocio.AjustarReceta(item.Receta.IdReceta, comensales),
                            Procedimientos = _procedimientoNegocio.ListarPorReceta(item.Receta.IdReceta)
                        });
                    }

                    GeneradorPDF.GenerarComanda(dialogo.FileName, comensales, secciones);
                    MensajesUI.MostrarExito("Comanda generada correctamente.");
                    System.Diagnostics.Process.Start(dialogo.FileName);

                    _comanda.Clear();
                }
                catch (NegocioException ex)
                {
                    MensajesUI.ManejarExcepcion(ex);
                }
                catch (Exception ex)
                {
                    MensajesUI.ManejarExcepcion(ex, "generar la comanda");
                }
            }
        }

        private string ObtenerNombreResponsable(int idClasificacion)
        {
            Persona responsable = _personaNegocio.ObtenerResponsablePorClasificacion(idClasificacion);
            return responsable != null ? responsable.NombreCompleto : "Sin asignar";
        }
    }
}
