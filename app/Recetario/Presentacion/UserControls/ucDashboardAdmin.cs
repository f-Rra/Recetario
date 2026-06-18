using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Dominio;
using Negocio;
using Presentacion.Helpers;

namespace Presentacion.UserControls
{
    public partial class ucDashboardAdmin : UserControl
    {
        private readonly Usuario _usuario;
        private readonly StockNegocio _stockNegocio = new StockNegocio();
        private readonly IngredienteNegocio _ingredienteNegocio = new IngredienteNegocio();
        private readonly RecetaNegocio _recetaNegocio = new RecetaNegocio();
        private readonly CostoNegocio _costoNegocio = new CostoNegocio();

        public ucDashboardAdmin(Usuario usuario)
        {
            InitializeComponent();
            _usuario = usuario;
            dgvStockCritico.AutoGenerateColumns = false;
            dgvHistorial.AutoGenerateColumns = false;
            CargarCombos();
            CargarStockCritico();
            CargarHistorial();
        }

        private void CargarCombos()
        {
            try
            {
                cboTipoMov.DisplayMember = "Nombre";
                cboTipoMov.ValueMember = "IdTipoMovimiento";
                cboTipoMov.DataSource = _stockNegocio.ListarTipos();

                cboIngredienteMov.DisplayMember = "Descripcion";
                cboIngredienteMov.ValueMember = "IdIngrediente";
                cboIngredienteMov.DataSource = _ingredienteNegocio.Listar();

                cboRecetaCosto.DisplayMember = "Nombre";
                cboRecetaCosto.ValueMember = "IdReceta";
                cboRecetaCosto.DataSource = _recetaNegocio.Listar();

                ActualizarUnidadMovimiento();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void CargarStockCritico()
        {
            try
            {
                dgvStockCritico.DataSource = _stockNegocio.ListarStockCritico();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void CargarHistorial()
        {
            try
            {
                dgvHistorial.DataSource = _stockNegocio.ListarMovimientos();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void cboIngredienteMov_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActualizarUnidadMovimiento();
        }

        private void ActualizarUnidadMovimiento()
        {
            lblUnidadMov.Text = cboIngredienteMov.SelectedItem is Ingrediente ingrediente
                ? ingrediente.Abreviatura
                : "-";
        }

        private void btnRegistrarMov_Click(object sender, EventArgs e)
        {
            if (!(cboTipoMov.SelectedItem is TipoMovimiento tipo))
            {
                MensajesUI.MostrarAdvertencia("Elegí el tipo de movimiento.");
                return;
            }
            if (!(cboIngredienteMov.SelectedItem is Ingrediente ingrediente))
            {
                MensajesUI.MostrarAdvertencia("Elegí el ingrediente.");
                return;
            }
            if (!decimal.TryParse(txtCantMov.Text, out decimal cantidad) || cantidad <= 0)
            {
                MensajesUI.MostrarAdvertencia("Ingresá una cantidad válida mayor a 0.");
                return;
            }

            try
            {
                MovimientoStock movimiento = new MovimientoStock
                {
                    IdIngrediente = ingrediente.IdIngrediente,
                    IdTipoMovimiento = tipo.IdTipoMovimiento,
                    Cantidad = cantidad,
                    IdUnidad = ingrediente.IdUnidad,
                    IdUsuario = _usuario.IdUsuario,
                    Observaciones = string.IsNullOrWhiteSpace(txtObs.Text) ? null : txtObs.Text.Trim()
                };

                _stockNegocio.RegistrarMovimiento(movimiento);
                MensajesUI.MostrarExito("Movimiento registrado correctamente.");

                txtCantMov.Clear();
                txtObs.Clear();
                CargarStockCritico();
                CargarHistorial();
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            if (!(cboRecetaCosto.SelectedItem is Receta receta))
            {
                MensajesUI.MostrarAdvertencia("Elegí una receta.");
                return;
            }
            if (!int.TryParse(txtPorciones.Text, out int porciones) || porciones <= 0)
            {
                MensajesUI.MostrarAdvertencia("Ingresá una cantidad de porciones válida mayor a 0.");
                return;
            }

            try
            {
                Costo costo = _costoNegocio.CalcularCosto(receta.IdReceta, porciones, _usuario.IdUsuario);
                if (costo != null)
                {
                    lblResultado.Text = $"Total: ${costo.CostoTotal:N2}   Unitario: ${costo.CostoUnitario:N2}";
                }
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
        }

        private void btnGenerarCostoPDF_Click(object sender, EventArgs e)
        {
            if (!(cboRecetaCosto.SelectedItem is Receta receta))
            {
                MensajesUI.MostrarAdvertencia("Elegí una receta.");
                return;
            }
            if (!int.TryParse(txtPorciones.Text, out int porciones) || porciones <= 0)
            {
                MensajesUI.MostrarAdvertencia("Ingresá una cantidad de porciones válida mayor a 0.");
                return;
            }

            try
            {
                Costo costo = _costoNegocio.CalcularCosto(receta.IdReceta, porciones, _usuario.IdUsuario);
                if (costo == null)
                {
                    return;
                }

                List<DetalleCosto> detalle = _costoNegocio.ObtenerDetalle(receta.IdReceta);

                using (SaveFileDialog dialogo = new SaveFileDialog())
                {
                    dialogo.Filter = "Archivos PDF (*.pdf)|*.pdf";
                    dialogo.FileName = $"Costo_{DateTime.Now:yyyyMMdd_HHmm}.pdf";

                    if (dialogo.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    GeneradorPDF.GenerarCosto(dialogo.FileName, receta.Nombre, porciones, detalle, costo.CostoTotal, costo.CostoUnitario);
                    lblResultado.Text = $"Total: ${costo.CostoTotal:N2}   Unitario: ${costo.CostoUnitario:N2}";
                    System.Diagnostics.Process.Start(dialogo.FileName);
                }
            }
            catch (NegocioException ex)
            {
                MensajesUI.ManejarExcepcion(ex);
            }
            catch (Exception ex)
            {
                MensajesUI.ManejarExcepcion(ex, "generar el PDF de costo");
            }
        }
    }
}
