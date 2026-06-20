using System;
using System.Windows.Forms;
using Dominio;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.UserControls
{
    public partial class ucIngredientes : UserControl
    {
        private readonly IngredienteNegocio _ingredienteNegocio = new IngredienteNegocio();
        private readonly UnidadNegocio _unidadNegocio = new UnidadNegocio();
        private int _idSeleccionado = 0;

        public ucIngredientes()
        {
            InitializeComponent();
            dgvIngredientes.AutoGenerateColumns = false;
            CargarUnidades();
            CargarIngredientes();
        }

        private void CargarUnidades()
        {
            try
            {
                cboUnidad.DisplayMember = "Nombre";
                cboUnidad.ValueMember = "IdUnidad";
                cboUnidad.DataSource = _unidadNegocio.Listar();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void CargarIngredientes()
        {
            try
            {
                dgvIngredientes.DataSource = null;
                dgvIngredientes.DataSource = _ingredienteNegocio.Listar();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void dgvIngredientes_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvIngredientes.CurrentRow != null && dgvIngredientes.CurrentRow.Selected && dgvIngredientes.CurrentRow.DataBoundItem is Ingrediente ingrediente)
            {
                _idSeleccionado = ingrediente.IdIngrediente;
                txtCodigo.Text = ingrediente.Codigo;
                txtDescripcion.Text = ingrediente.Descripcion;
                cboUnidad.SelectedValue = ingrediente.IdUnidad;
                txtStockMinimo.Text = ingrediente.StockMinimo.ToString("0.##");
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            _idSeleccionado = 0;
            txtDescripcion.Clear();
            txtStockMinimo.Clear();
            if (cboUnidad.Items.Count > 0)
            {
                cboUnidad.SelectedIndex = 0;
            }
            dgvIngredientes.ClearSelection();

            try
            {
                txtCodigo.Text = _ingredienteNegocio.ObtenerProximoCodigo();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }

            txtDescripcion.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCodigo.Text))
            {
                MensajesUI.MostrarAdvertencia("Ingresá el código.");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtDescripcion.Text))
            {
                MensajesUI.MostrarAdvertencia("Ingresá la descripción.");
                return;
            }
            if (!(cboUnidad.SelectedItem is Unidad unidad))
            {
                MensajesUI.MostrarAdvertencia("Elegí la unidad.");
                return;
            }
            if (!decimal.TryParse(txtStockMinimo.Text, out decimal stockMinimo) || stockMinimo < 0)
            {
                MensajesUI.MostrarAdvertencia("Ingresá un stock mínimo válido (0 o mayor).");
                return;
            }

            try
            {
                Ingrediente ingrediente = new Ingrediente
                {
                    IdIngrediente = _idSeleccionado,
                    Codigo = txtCodigo.Text.Trim(),
                    Descripcion = txtDescripcion.Text.Trim(),
                    IdUnidad = unidad.IdUnidad,
                    StockMinimo = stockMinimo
                };

                if (_idSeleccionado == 0)
                {
                    _ingredienteNegocio.Agregar(ingrediente);
                    MensajesUI.MostrarExito("Ingrediente agregado correctamente.");
                }
                else
                {
                    _ingredienteNegocio.Modificar(ingrediente);
                    MensajesUI.MostrarExito("Ingrediente modificado correctamente.");
                }

                CargarIngredientes();
                LimpiarFormulario();
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
                MensajesUI.MostrarAdvertencia("Seleccioná un ingrediente de la lista.");
                return;
            }

            if (!MensajesUI.MostrarConfirmacion("¿Eliminar el ingrediente seleccionado?"))
            {
                return;
            }

            try
            {
                _ingredienteNegocio.Eliminar(_idSeleccionado);
                MensajesUI.MostrarExito("Ingrediente eliminado correctamente.");
                CargarIngredientes();
                LimpiarFormulario();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }
    }
}
