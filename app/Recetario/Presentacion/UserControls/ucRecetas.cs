using System;
using System.Windows.Forms;
using Dominio;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.UserControls
{
    public partial class ucRecetas : UserControl
    {
        private readonly RecetaNegocio _recetaNegocio = new RecetaNegocio();
        private readonly ClasificacionNegocio _clasificacionNegocio = new ClasificacionNegocio();
        private readonly IngredienteNegocio _ingredienteNegocio = new IngredienteNegocio();
        private readonly ProcedimientoNegocio _procedimientoNegocio = new ProcedimientoNegocio();
        private int _idSeleccionado = 0;

        public ucRecetas()
        {
            InitializeComponent();
            dgvRecetas.AutoGenerateColumns = false;
            dgvIngRec.AutoGenerateColumns = false;
            dgvProc.AutoGenerateColumns = false;
            CargarClasificaciones();
            CargarIngredientesCombo();
            CargarRecetas();
        }

        private void CargarClasificaciones()
        {
            try
            {
                cboRClasificacion.DisplayMember = "Nombre";
                cboRClasificacion.ValueMember = "IdClasificacion";
                cboRClasificacion.DataSource = _clasificacionNegocio.Listar();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void CargarIngredientesCombo()
        {
            try
            {
                cboIngrediente.DisplayMember = "Descripcion";
                cboIngrediente.ValueMember = "IdIngrediente";
                cboIngrediente.DataSource = _ingredienteNegocio.Listar();
                ActualizarUnidadIngrediente();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void CargarRecetas()
        {
            try
            {
                dgvRecetas.DataSource = null;
                dgvRecetas.DataSource = _recetaNegocio.Listar();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void dgvRecetas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvRecetas.CurrentRow != null && dgvRecetas.CurrentRow.DataBoundItem is Receta receta)
            {
                _idSeleccionado = receta.IdReceta;
                txtRCodigo.Text = receta.Codigo;
                txtRNombre.Text = receta.Nombre;
                cboRClasificacion.SelectedValue = receta.IdClasificacion;
                txtRPorciones.Text = receta.PorcionesBase.ToString();
                CargarDetalle();
            }
        }

        private void CargarDetalle()
        {
            if (_idSeleccionado == 0)
            {
                dgvIngRec.DataSource = null;
                dgvProc.DataSource = null;
                return;
            }

            try
            {
                dgvIngRec.DataSource = _recetaNegocio.ListarIngredientes(_idSeleccionado);
                dgvProc.DataSource = _procedimientoNegocio.ListarPorReceta(_idSeleccionado);
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void SeleccionarReceta(int idReceta)
        {
            foreach (DataGridViewRow fila in dgvRecetas.Rows)
            {
                if (fila.DataBoundItem is Receta receta && receta.IdReceta == idReceta)
                {
                    dgvRecetas.CurrentCell = fila.Cells[0];
                    return;
                }
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarReceta();
        }

        private void LimpiarReceta()
        {
            _idSeleccionado = 0;
            txtRCodigo.Clear();
            txtRNombre.Clear();
            txtRPorciones.Clear();
            if (cboRClasificacion.Items.Count > 0)
            {
                cboRClasificacion.SelectedIndex = 0;
            }
            dgvRecetas.ClearSelection();
            dgvIngRec.DataSource = null;
            dgvProc.DataSource = null;
            txtRCodigo.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRCodigo.Text))
            {
                MensajesUI.MostrarAdvertencia("Ingresá el código.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtRNombre.Text))
            {
                MensajesUI.MostrarAdvertencia("Ingresá el nombre.");
                return;
            }
            if (!(cboRClasificacion.SelectedItem is Clasificacion clasificacion))
            {
                MensajesUI.MostrarAdvertencia("Elegí la clasificación.");
                return;
            }
            if (!int.TryParse(txtRPorciones.Text, out int porciones) || porciones <= 0)
            {
                MensajesUI.MostrarAdvertencia("Ingresá una cantidad de porciones base válida mayor a 0.");
                return;
            }

            try
            {
                Receta receta = new Receta
                {
                    IdReceta = _idSeleccionado,
                    Codigo = txtRCodigo.Text.Trim(),
                    Nombre = txtRNombre.Text.Trim(),
                    IdClasificacion = clasificacion.IdClasificacion,
                    PorcionesBase = porciones
                };

                if (_idSeleccionado == 0)
                {
                    int nuevoId = _recetaNegocio.Agregar(receta);
                    MensajesUI.MostrarExito("Receta agregada. Ahora podés cargar sus ingredientes y pasos.");
                    CargarRecetas();
                    SeleccionarReceta(nuevoId);
                }
                else
                {
                    int id = _idSeleccionado;
                    _recetaNegocio.Modificar(receta);
                    MensajesUI.MostrarExito("Receta modificada correctamente.");
                    CargarRecetas();
                    SeleccionarReceta(id);
                }
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MensajesUI.MostrarAdvertencia("Seleccioná una receta de la lista.");
                return;
            }

            if (!MensajesUI.MostrarConfirmacion("¿Dar de baja la receta seleccionada?"))
            {
                return;
            }

            try
            {
                _recetaNegocio.Eliminar(_idSeleccionado);
                MensajesUI.MostrarExito("Receta dada de baja.");
                CargarRecetas();
                LimpiarReceta();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void cboIngrediente_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarUnidadIngrediente();
        }

        private void ActualizarUnidadIngrediente()
        {
            lblUniIng.Text = cboIngrediente.SelectedItem is Ingrediente ingrediente
                ? ingrediente.Abreviatura
                : "-";
        }

        private void btnAgregarIng_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MensajesUI.MostrarAdvertencia("Guardá la receta antes de cargar ingredientes.");
                return;
            }
            if (!(cboIngrediente.SelectedItem is Ingrediente ingrediente))
            {
                MensajesUI.MostrarAdvertencia("Elegí un ingrediente.");
                return;
            }
            if (!decimal.TryParse(txtNeta.Text, out decimal cantNeta) || cantNeta <= 0)
            {
                MensajesUI.MostrarAdvertencia("Ingresá una cantidad neta válida mayor a 0.");
                return;
            }
            if (!decimal.TryParse(txtRend.Text, out decimal rendimiento) || rendimiento <= 0 || rendimiento > 100)
            {
                MensajesUI.MostrarAdvertencia("El rendimiento debe estar entre 1 y 100.");
                return;
            }

            try
            {
                _recetaNegocio.AgregarIngrediente(new IngredienteReceta
                {
                    IdReceta = _idSeleccionado,
                    IdIngrediente = ingrediente.IdIngrediente,
                    CantNeta = cantNeta,
                    Rendimiento = rendimiento,
                    IdUnidad = ingrediente.IdUnidad
                });

                CargarDetalle();
                txtNeta.Clear();
                txtRend.Text = "100";
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void btnQuitarIng_Click(object sender, EventArgs e)
        {
            if (dgvIngRec.CurrentRow != null && dgvIngRec.CurrentRow.DataBoundItem is IngredienteReceta ingredienteReceta)
            {
                try
                {
                    _recetaNegocio.QuitarIngrediente(ingredienteReceta.IdReceta, ingredienteReceta.IdIngrediente);
                    CargarDetalle();
                }
                catch (NegocioException ex)
                {
                    MensajesUI.ManejarExcepcion(ex);
                }
            }
            else
            {
                MensajesUI.MostrarAdvertencia("Seleccioná un ingrediente de la receta.");
            }
        }

        private void btnAgregarPaso_Click(object sender, EventArgs e)
        {
            if (_idSeleccionado == 0)
            {
                MensajesUI.MostrarAdvertencia("Guardá la receta antes de cargar pasos.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtPaso.Text))
            {
                MensajesUI.MostrarAdvertencia("Ingresá la descripción del paso.");
                return;
            }

            try
            {
                _procedimientoNegocio.Agregar(new Procedimiento
                {
                    IdReceta = _idSeleccionado,
                    NroPaso = dgvProc.Rows.Count + 1,
                    Descripcion = txtPaso.Text.Trim()
                });

                CargarDetalle();
                txtPaso.Clear();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void btnQuitarPaso_Click(object sender, EventArgs e)
        {
            if (dgvProc.CurrentRow != null && dgvProc.CurrentRow.DataBoundItem is Procedimiento procedimiento)
            {
                try
                {
                    _procedimientoNegocio.Eliminar(procedimiento.IdProcedimiento);
                    CargarDetalle();
                }
                catch (NegocioException ex)
                {
                    MensajesUI.ManejarExcepcion(ex);
                }
            }
            else
            {
                MensajesUI.MostrarAdvertencia("Seleccioná un paso del procedimiento.");
            }
        }
    }
}
