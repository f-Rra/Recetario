using System;
using System.Windows.Forms;
using Dominio;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.UserControls
{
    public partial class ucProveedores : UserControl
    {
        private readonly ProveedorNegocio _proveedorNegocio = new ProveedorNegocio();
        private int _idSeleccionado = 0;

        public ucProveedores()
        {
            InitializeComponent();
            dgvProveedores.AutoGenerateColumns = false;
            CargarProveedores();
        }

        private void CargarProveedores()
        {
            try
            {
                dgvProveedores.DataSource = null;
                dgvProveedores.DataSource = _proveedorNegocio.Listar();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void dgvProveedores_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProveedores.CurrentRow != null && dgvProveedores.CurrentRow.DataBoundItem is Proveedor proveedor)
            {
                _idSeleccionado = proveedor.IdProveedor;
                txtNombre.Text = proveedor.Nombre;
                txtContacto.Text = proveedor.Contacto;
                txtTelefono.Text = proveedor.Telefono;
                txtEmail.Text = proveedor.Email;
                txtDireccion.Text = proveedor.Direccion;
            }
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            LimpiarFormulario();
        }

        private void LimpiarFormulario()
        {
            _idSeleccionado = 0;
            txtNombre.Clear();
            txtContacto.Clear();
            txtTelefono.Clear();
            txtEmail.Clear();
            txtDireccion.Clear();
            dgvProveedores.ClearSelection();
            txtNombre.Focus();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text))
            {
                MensajesUI.MostrarAdvertencia("Ingresá el nombre del proveedor.");
                return;
            }

            try
            {
                Proveedor proveedor = new Proveedor
                {
                    IdProveedor = _idSeleccionado,
                    Nombre = txtNombre.Text.Trim(),
                    Contacto = LimpiarOpcional(txtContacto.Text),
                    Telefono = LimpiarOpcional(txtTelefono.Text),
                    Email = LimpiarOpcional(txtEmail.Text),
                    Direccion = LimpiarOpcional(txtDireccion.Text)
                };

                if (_idSeleccionado == 0)
                {
                    _proveedorNegocio.Agregar(proveedor);
                    MensajesUI.MostrarExito("Proveedor agregado correctamente.");
                }
                else
                {
                    _proveedorNegocio.Modificar(proveedor);
                    MensajesUI.MostrarExito("Proveedor modificado correctamente.");
                }

                CargarProveedores();
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
                MensajesUI.MostrarAdvertencia("Seleccioná un proveedor de la lista.");
                return;
            }

            if (!MensajesUI.MostrarConfirmacion("¿Eliminar el proveedor seleccionado?"))
            {
                return;
            }

            try
            {
                _proveedorNegocio.Eliminar(_idSeleccionado);
                MensajesUI.MostrarExito("Proveedor eliminado correctamente.");
                CargarProveedores();
                LimpiarFormulario();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private static string LimpiarOpcional(string texto)
        {
            return string.IsNullOrWhiteSpace(texto) ? null : texto.Trim();
        }
    }
}
