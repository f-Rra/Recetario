using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Dominio;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion
{
    public partial class frmModificacion : Form
    {
        #region Campos y constructor

        private readonly Receta _receta;
        private readonly ComandaNegocio _comandaNegocio = new ComandaNegocio();
        private readonly RecetaNegocio _recetaNegocio = new RecetaNegocio();
        private readonly IngredienteNegocio _ingredienteNegocio = new IngredienteNegocio();
        private readonly UnidadNegocio _unidadNegocio = new UnidadNegocio();
        private readonly BindingList<Modificacion> _modificaciones;

        public List<Modificacion> Modificaciones => new List<Modificacion>(_modificaciones);

        public frmModificacion(Receta receta, List<Modificacion> modificacionesExistentes)
        {
            InitializeComponent();
            _receta = receta;
            _modificaciones = new BindingList<Modificacion>(new List<Modificacion>(modificacionesExistentes));
            lblReceta.Text = $"Receta: {receta.Nombre}";
            dgvMods.AutoGenerateColumns = false;
            dgvMods.DataSource = _modificaciones;
            CargarCombos();
        }

        #endregion

        #region Carga de datos

        private void CargarCombos()
        {
            try
            {
                List<TipoModificacion> tipos = _comandaNegocio.ListarTiposModificacion();
                cboTipo.DisplayMember = "Nombre";
                cboTipo.ValueMember = "IdTipoModificacion";
                cboTipo.DataSource = tipos;

                List<IngredienteReceta> ingredientesReceta = _recetaNegocio.ListarIngredientes(_receta.IdReceta);
                cboOriginal.DisplayMember = "NombreIngrediente";
                cboOriginal.ValueMember = "IdIngrediente";
                cboOriginal.DataSource = ingredientesReceta;

                List<Ingrediente> ingredientes = _ingredienteNegocio.Listar();
                cboReemplazo.DisplayMember = "Descripcion";
                cboReemplazo.ValueMember = "IdIngrediente";
                cboReemplazo.DataSource = ingredientes;

                List<Unidad> unidades = _unidadNegocio.Listar();
                cboUnidad.DisplayMember = "Nombre";
                cboUnidad.ValueMember = "IdUnidad";
                cboUnidad.DataSource = unidades;
                cboUnidad.Enabled = false;

                ActualizarCamposPorTipo();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        #endregion

        #region Eventos

        private void cboTipo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarCamposPorTipo();
        }

        private void cboOriginal_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarUnidad();
        }

        private void cboReemplazo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarUnidad();
        }

        private void ActualizarCamposPorTipo()
        {
            if (!(cboTipo.SelectedItem is TipoModificacion tipo))
            {
                return;
            }

            cboOriginal.Enabled = tipo.Nombre != "adicion";
            cboReemplazo.Enabled = tipo.Nombre != "eliminacion";

            switch (tipo.Nombre)
            {
                case "adicion":
                    lblOriginal.Text = "Ingr. original";
                    lblReemplazo.Text = "Ingr. a agregar";
                    break;
                case "eliminacion":
                    lblOriginal.Text = "Ingr. a quitar";
                    lblReemplazo.Text = "Ingr. reemplazo";
                    break;
                default:
                    lblOriginal.Text = "Ingr. original";
                    lblReemplazo.Text = "Ingr. reemplazo";
                    break;
            }

            ActualizarUnidad();
        }

        private void ActualizarUnidad()
        {
            if (!(cboTipo.SelectedItem is TipoModificacion tipo))
            {
                return;
            }

            int? idUnidad = null;

            if (tipo.Nombre == "eliminacion")
            {
                if (cboOriginal.SelectedItem is IngredienteReceta original)
                {
                    idUnidad = original.IdUnidad;
                }
            }
            else
            {
                if (cboReemplazo.SelectedItem is Ingrediente reemplazo)
                {
                    idUnidad = reemplazo.IdUnidad;
                }
            }

            if (idUnidad.HasValue)
            {
                cboUnidad.SelectedValue = idUnidad.Value;
            }
        }

        #endregion

        #region Acciones

        private void btnAgregarMod_Click(object sender, EventArgs e)
        {
            if (!(cboTipo.SelectedItem is TipoModificacion tipo))
            {
                MensajesUI.MostrarAdvertencia("Elegí un tipo de modificación.");
                return;
            }

            bool usaOriginal = tipo.Nombre != "adicion";
            bool usaReemplazo = tipo.Nombre != "eliminacion";

            if (usaOriginal && !(cboOriginal.SelectedItem is IngredienteReceta))
            {
                MensajesUI.MostrarAdvertencia("Elegí el ingrediente original.");
                return;
            }
            if (usaReemplazo && !(cboReemplazo.SelectedItem is Ingrediente))
            {
                MensajesUI.MostrarAdvertencia("Elegí el ingrediente de reemplazo.");
                return;
            }

            if (tipo.Nombre == "sustitucion")
            {
                IngredienteReceta original = (IngredienteReceta)cboOriginal.SelectedItem;
                Ingrediente reemplazo = (Ingrediente)cboReemplazo.SelectedItem;

                if (original.IdIngrediente == reemplazo.IdIngrediente)
                {
                    MensajesUI.MostrarAdvertencia("No podés sustituir un ingrediente por sí mismo.");
                    return;
                }
            }

            if (!decimal.TryParse(txtCantidad.Text, out decimal cantidad) || cantidad <= 0)
            {
                MensajesUI.MostrarAdvertencia("Ingresá una cantidad válida mayor a 0.");
                return;
            }
            if (!(cboUnidad.SelectedItem is Unidad unidad))
            {
                MensajesUI.MostrarAdvertencia("No se pudo determinar la unidad del ingrediente.");
                return;
            }

            Modificacion m = new Modificacion
            {
                IdTipoModificacion = tipo.IdTipoModificacion,
                Tipo = tipo.Nombre,
                Cantidad = cantidad,
                IdUnidad = unidad.IdUnidad,
                Abreviatura = unidad.Abreviatura
            };

            if (usaOriginal)
            {
                IngredienteReceta original = (IngredienteReceta)cboOriginal.SelectedItem;
                m.IdIngredienteOriginal = original.IdIngrediente;
                m.IngredienteOriginal = original.NombreIngrediente;
            }

            if (usaReemplazo)
            {
                Ingrediente reemplazo = (Ingrediente)cboReemplazo.SelectedItem;
                m.IdIngredienteReemplazo = reemplazo.IdIngrediente;
                m.IngredienteReemplazo = reemplazo.Descripcion;
            }

            _modificaciones.Add(m);
        }

        private void btnQuitarMod_Click(object sender, EventArgs e)
        {
            if (dgvMods.CurrentRow != null && dgvMods.CurrentRow.DataBoundItem is Modificacion m)
            {
                _modificaciones.Remove(m);
            }
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion
    }
}
