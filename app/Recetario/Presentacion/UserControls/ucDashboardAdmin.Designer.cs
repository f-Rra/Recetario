namespace Presentacion.UserControls
{
    partial class ucDashboardAdmin
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.gbStockCritico = new System.Windows.Forms.GroupBox();
            this.dgvStockCritico = new System.Windows.Forms.DataGridView();
            this.colSCCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCMinimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbHistorial = new System.Windows.Forms.GroupBox();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.colHFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHIngrediente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHObs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbMovimiento = new System.Windows.Forms.GroupBox();
            this.lblTipoMov = new System.Windows.Forms.Label();
            this.cboTipoMov = new System.Windows.Forms.ComboBox();
            this.lblIngredienteMov = new System.Windows.Forms.Label();
            this.cboIngredienteMov = new System.Windows.Forms.ComboBox();
            this.lblCantMov = new System.Windows.Forms.Label();
            this.txtCantMov = new System.Windows.Forms.TextBox();
            this.lblUnidadMov = new System.Windows.Forms.Label();
            this.lblObs = new System.Windows.Forms.Label();
            this.txtObs = new System.Windows.Forms.TextBox();
            this.btnRegistrarMov = new System.Windows.Forms.Button();
            this.gbCosto = new System.Windows.Forms.GroupBox();
            this.lblRecetaCosto = new System.Windows.Forms.Label();
            this.cboRecetaCosto = new System.Windows.Forms.ComboBox();
            this.lblPorciones = new System.Windows.Forms.Label();
            this.txtPorciones = new System.Windows.Forms.TextBox();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.btnGenerarCostoPDF = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.gbModificaciones = new System.Windows.Forms.GroupBox();
            this.dgvModificaciones = new System.Windows.Forms.DataGridView();
            this.colMFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMReceta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMOriginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMReemplazo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockCritico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModificaciones)).BeginInit();
            this.gbStockCritico.SuspendLayout();
            this.gbHistorial.SuspendLayout();
            this.gbMovimiento.SuspendLayout();
            this.gbCosto.SuspendLayout();
            this.gbModificaciones.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 10);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(232, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Panel de administración";
            //
            // gbStockCritico
            //
            this.gbStockCritico.Controls.Add(this.dgvStockCritico);
            this.gbStockCritico.Location = new System.Drawing.Point(20, 42);
            this.gbStockCritico.Name = "gbStockCritico";
            this.gbStockCritico.Padding = new System.Windows.Forms.Padding(8);
            this.gbStockCritico.Size = new System.Drawing.Size(565, 205);
            this.gbStockCritico.TabIndex = 1;
            this.gbStockCritico.TabStop = false;
            this.gbStockCritico.Text = "Stock crítico";
            //
            // dgvStockCritico
            //
            this.dgvStockCritico.AllowUserToAddRows = false;
            this.dgvStockCritico.AllowUserToDeleteRows = false;
            this.dgvStockCritico.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockCritico.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colSCCodigo,
                this.colSCDescripcion,
                this.colSCActual,
                this.colSCMinimo,
                this.colSCUnidad});
            this.dgvStockCritico.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockCritico.Location = new System.Drawing.Point(8, 24);
            this.dgvStockCritico.Name = "dgvStockCritico";
            this.dgvStockCritico.ReadOnly = true;
            this.dgvStockCritico.RowHeadersVisible = false;
            this.dgvStockCritico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockCritico.Size = new System.Drawing.Size(549, 173);
            this.dgvStockCritico.TabIndex = 0;
            //
            // colSCCodigo
            //
            this.colSCCodigo.DataPropertyName = "Codigo";
            this.colSCCodigo.HeaderText = "Código";
            this.colSCCodigo.Name = "colSCCodigo";
            this.colSCCodigo.ReadOnly = true;
            this.colSCCodigo.Width = 90;
            //
            // colSCDescripcion
            //
            this.colSCDescripcion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSCDescripcion.DataPropertyName = "Descripcion";
            this.colSCDescripcion.HeaderText = "Descripción";
            this.colSCDescripcion.Name = "colSCDescripcion";
            this.colSCDescripcion.ReadOnly = true;
            //
            // colSCActual
            //
            this.colSCActual.DataPropertyName = "StockActual";
            this.colSCActual.DefaultCellStyle.Format = "0.####";
            this.colSCActual.HeaderText = "Actual";
            this.colSCActual.Name = "colSCActual";
            this.colSCActual.ReadOnly = true;
            this.colSCActual.Width = 85;
            //
            // colSCMinimo
            //
            this.colSCMinimo.DataPropertyName = "StockMinimo";
            this.colSCMinimo.DefaultCellStyle.Format = "0.####";
            this.colSCMinimo.HeaderText = "Mínimo";
            this.colSCMinimo.Name = "colSCMinimo";
            this.colSCMinimo.ReadOnly = true;
            this.colSCMinimo.Width = 85;
            //
            // colSCUnidad
            //
            this.colSCUnidad.DataPropertyName = "Abreviatura";
            this.colSCUnidad.HeaderText = "Un.";
            this.colSCUnidad.Name = "colSCUnidad";
            this.colSCUnidad.ReadOnly = true;
            this.colSCUnidad.Width = 50;
            //
            // gbHistorial
            //
            this.gbHistorial.Controls.Add(this.dgvHistorial);
            this.gbHistorial.Location = new System.Drawing.Point(600, 42);
            this.gbHistorial.Name = "gbHistorial";
            this.gbHistorial.Padding = new System.Windows.Forms.Padding(8);
            this.gbHistorial.Size = new System.Drawing.Size(560, 205);
            this.gbHistorial.TabIndex = 2;
            this.gbHistorial.TabStop = false;
            this.gbHistorial.Text = "Historial de movimientos";
            //
            // dgvHistorial
            //
            this.dgvHistorial.AllowUserToAddRows = false;
            this.dgvHistorial.AllowUserToDeleteRows = false;
            this.dgvHistorial.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHistorial.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colHFecha,
                this.colHIngrediente,
                this.colHTipo,
                this.colHCantidad,
                this.colHUnidad,
                this.colHObs});
            this.dgvHistorial.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHistorial.Location = new System.Drawing.Point(8, 24);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowHeadersVisible = false;
            this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorial.Size = new System.Drawing.Size(544, 173);
            this.dgvHistorial.TabIndex = 0;
            //
            // colHFecha
            //
            this.colHFecha.DataPropertyName = "Fecha";
            this.colHFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            this.colHFecha.HeaderText = "Fecha";
            this.colHFecha.Name = "colHFecha";
            this.colHFecha.ReadOnly = true;
            this.colHFecha.Width = 120;
            //
            // colHIngrediente
            //
            this.colHIngrediente.DataPropertyName = "NombreIngrediente";
            this.colHIngrediente.HeaderText = "Ingrediente";
            this.colHIngrediente.Name = "colHIngrediente";
            this.colHIngrediente.ReadOnly = true;
            this.colHIngrediente.Width = 160;
            //
            // colHTipo
            //
            this.colHTipo.DataPropertyName = "TipoMovimiento";
            this.colHTipo.HeaderText = "Tipo";
            this.colHTipo.Name = "colHTipo";
            this.colHTipo.ReadOnly = true;
            this.colHTipo.Width = 60;
            //
            // colHCantidad
            //
            this.colHCantidad.DataPropertyName = "Cantidad";
            this.colHCantidad.DefaultCellStyle.Format = "0.####";
            this.colHCantidad.HeaderText = "Cant.";
            this.colHCantidad.Name = "colHCantidad";
            this.colHCantidad.ReadOnly = true;
            this.colHCantidad.Width = 60;
            //
            // colHUnidad
            //
            this.colHUnidad.DataPropertyName = "Abreviatura";
            this.colHUnidad.HeaderText = "Un.";
            this.colHUnidad.Name = "colHUnidad";
            this.colHUnidad.ReadOnly = true;
            this.colHUnidad.Width = 45;
            //
            // colHObs
            //
            this.colHObs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHObs.DataPropertyName = "Observaciones";
            this.colHObs.HeaderText = "Observaciones";
            this.colHObs.Name = "colHObs";
            this.colHObs.ReadOnly = true;
            //
            // gbMovimiento
            //
            this.gbMovimiento.Controls.Add(this.lblTipoMov);
            this.gbMovimiento.Controls.Add(this.cboTipoMov);
            this.gbMovimiento.Controls.Add(this.lblIngredienteMov);
            this.gbMovimiento.Controls.Add(this.cboIngredienteMov);
            this.gbMovimiento.Controls.Add(this.lblCantMov);
            this.gbMovimiento.Controls.Add(this.txtCantMov);
            this.gbMovimiento.Controls.Add(this.lblUnidadMov);
            this.gbMovimiento.Controls.Add(this.lblObs);
            this.gbMovimiento.Controls.Add(this.txtObs);
            this.gbMovimiento.Controls.Add(this.btnRegistrarMov);
            this.gbMovimiento.Location = new System.Drawing.Point(20, 253);
            this.gbMovimiento.Name = "gbMovimiento";
            this.gbMovimiento.Size = new System.Drawing.Size(565, 200);
            this.gbMovimiento.TabIndex = 3;
            this.gbMovimiento.TabStop = false;
            this.gbMovimiento.Text = "Registrar movimiento de stock";
            //
            // lblTipoMov
            //
            this.lblTipoMov.AutoSize = true;
            this.lblTipoMov.Location = new System.Drawing.Point(15, 30);
            this.lblTipoMov.Name = "lblTipoMov";
            this.lblTipoMov.Size = new System.Drawing.Size(30, 15);
            this.lblTipoMov.TabIndex = 0;
            this.lblTipoMov.Text = "Tipo";
            //
            // cboTipoMov
            //
            this.cboTipoMov.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoMov.Location = new System.Drawing.Point(120, 27);
            this.cboTipoMov.Name = "cboTipoMov";
            this.cboTipoMov.Size = new System.Drawing.Size(200, 23);
            this.cboTipoMov.TabIndex = 1;
            //
            // lblIngredienteMov
            //
            this.lblIngredienteMov.AutoSize = true;
            this.lblIngredienteMov.Location = new System.Drawing.Point(15, 60);
            this.lblIngredienteMov.Name = "lblIngredienteMov";
            this.lblIngredienteMov.Size = new System.Drawing.Size(67, 15);
            this.lblIngredienteMov.TabIndex = 2;
            this.lblIngredienteMov.Text = "Ingrediente";
            //
            // cboIngredienteMov
            //
            this.cboIngredienteMov.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIngredienteMov.Location = new System.Drawing.Point(120, 57);
            this.cboIngredienteMov.Name = "cboIngredienteMov";
            this.cboIngredienteMov.Size = new System.Drawing.Size(420, 23);
            this.cboIngredienteMov.TabIndex = 3;
            this.cboIngredienteMov.SelectedIndexChanged += new System.EventHandler(this.cboIngredienteMov_SelectedIndexChanged);
            //
            // lblCantMov
            //
            this.lblCantMov.AutoSize = true;
            this.lblCantMov.Location = new System.Drawing.Point(15, 92);
            this.lblCantMov.Name = "lblCantMov";
            this.lblCantMov.Size = new System.Drawing.Size(58, 15);
            this.lblCantMov.TabIndex = 4;
            this.lblCantMov.Text = "Cantidad";
            //
            // txtCantMov
            //
            this.txtCantMov.Location = new System.Drawing.Point(120, 89);
            this.txtCantMov.Name = "txtCantMov";
            this.txtCantMov.Size = new System.Drawing.Size(120, 23);
            this.txtCantMov.TabIndex = 5;
            //
            // lblUnidadMov
            //
            this.lblUnidadMov.AutoSize = true;
            this.lblUnidadMov.Location = new System.Drawing.Point(250, 92);
            this.lblUnidadMov.Name = "lblUnidadMov";
            this.lblUnidadMov.Size = new System.Drawing.Size(12, 15);
            this.lblUnidadMov.TabIndex = 6;
            this.lblUnidadMov.Text = "-";
            //
            // lblObs
            //
            this.lblObs.AutoSize = true;
            this.lblObs.Location = new System.Drawing.Point(15, 124);
            this.lblObs.Name = "lblObs";
            this.lblObs.Size = new System.Drawing.Size(83, 15);
            this.lblObs.TabIndex = 7;
            this.lblObs.Text = "Observaciones";
            //
            // txtObs
            //
            this.txtObs.Location = new System.Drawing.Point(120, 121);
            this.txtObs.Name = "txtObs";
            this.txtObs.Size = new System.Drawing.Size(420, 23);
            this.txtObs.TabIndex = 8;
            //
            // btnRegistrarMov
            //
            this.btnRegistrarMov.Location = new System.Drawing.Point(120, 155);
            this.btnRegistrarMov.Name = "btnRegistrarMov";
            this.btnRegistrarMov.Size = new System.Drawing.Size(200, 30);
            this.btnRegistrarMov.TabIndex = 9;
            this.btnRegistrarMov.Text = "Registrar movimiento";
            this.btnRegistrarMov.UseVisualStyleBackColor = true;
            this.btnRegistrarMov.Click += new System.EventHandler(this.btnRegistrarMov_Click);
            //
            // gbCosto
            //
            this.gbCosto.Controls.Add(this.lblRecetaCosto);
            this.gbCosto.Controls.Add(this.cboRecetaCosto);
            this.gbCosto.Controls.Add(this.lblPorciones);
            this.gbCosto.Controls.Add(this.txtPorciones);
            this.gbCosto.Controls.Add(this.btnCalcular);
            this.gbCosto.Controls.Add(this.btnGenerarCostoPDF);
            this.gbCosto.Controls.Add(this.lblResultado);
            this.gbCosto.Location = new System.Drawing.Point(600, 253);
            this.gbCosto.Name = "gbCosto";
            this.gbCosto.Size = new System.Drawing.Size(560, 200);
            this.gbCosto.TabIndex = 4;
            this.gbCosto.TabStop = false;
            this.gbCosto.Text = "Calcular costo de receta";
            //
            // lblRecetaCosto
            //
            this.lblRecetaCosto.AutoSize = true;
            this.lblRecetaCosto.Location = new System.Drawing.Point(15, 33);
            this.lblRecetaCosto.Name = "lblRecetaCosto";
            this.lblRecetaCosto.Size = new System.Drawing.Size(43, 15);
            this.lblRecetaCosto.TabIndex = 0;
            this.lblRecetaCosto.Text = "Receta";
            //
            // cboRecetaCosto
            //
            this.cboRecetaCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecetaCosto.Location = new System.Drawing.Point(120, 30);
            this.cboRecetaCosto.Name = "cboRecetaCosto";
            this.cboRecetaCosto.Size = new System.Drawing.Size(420, 23);
            this.cboRecetaCosto.TabIndex = 1;
            //
            // lblPorciones
            //
            this.lblPorciones.AutoSize = true;
            this.lblPorciones.Location = new System.Drawing.Point(15, 67);
            this.lblPorciones.Name = "lblPorciones";
            this.lblPorciones.Size = new System.Drawing.Size(61, 15);
            this.lblPorciones.TabIndex = 2;
            this.lblPorciones.Text = "Porciones";
            //
            // txtPorciones
            //
            this.txtPorciones.Location = new System.Drawing.Point(120, 64);
            this.txtPorciones.Name = "txtPorciones";
            this.txtPorciones.Size = new System.Drawing.Size(120, 23);
            this.txtPorciones.TabIndex = 3;
            //
            // btnCalcular
            //
            this.btnCalcular.Location = new System.Drawing.Point(120, 102);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(150, 30);
            this.btnCalcular.TabIndex = 4;
            this.btnCalcular.Text = "Calcular";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            //
            // btnGenerarCostoPDF
            //
            this.btnGenerarCostoPDF.Location = new System.Drawing.Point(280, 102);
            this.btnGenerarCostoPDF.Name = "btnGenerarCostoPDF";
            this.btnGenerarCostoPDF.Size = new System.Drawing.Size(150, 30);
            this.btnGenerarCostoPDF.TabIndex = 5;
            this.btnGenerarCostoPDF.Text = "Generar PDF";
            this.btnGenerarCostoPDF.UseVisualStyleBackColor = true;
            this.btnGenerarCostoPDF.Click += new System.EventHandler(this.btnGenerarCostoPDF_Click);
            //
            // lblResultado
            //
            this.lblResultado.AutoSize = true;
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblResultado.Location = new System.Drawing.Point(15, 150);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(0, 17);
            this.lblResultado.TabIndex = 6;
            //
            // gbModificaciones
            //
            this.gbModificaciones.Controls.Add(this.dgvModificaciones);
            this.gbModificaciones.Location = new System.Drawing.Point(20, 459);
            this.gbModificaciones.Name = "gbModificaciones";
            this.gbModificaciones.Padding = new System.Windows.Forms.Padding(8);
            this.gbModificaciones.Size = new System.Drawing.Size(1140, 130);
            this.gbModificaciones.TabIndex = 5;
            this.gbModificaciones.TabStop = false;
            this.gbModificaciones.Text = "Historial de modificaciones";
            //
            // dgvModificaciones
            //
            this.dgvModificaciones.AllowUserToAddRows = false;
            this.dgvModificaciones.AllowUserToDeleteRows = false;
            this.dgvModificaciones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvModificaciones.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colMFecha,
                this.colMReceta,
                this.colMTipo,
                this.colMOriginal,
                this.colMReemplazo,
                this.colMCantidad,
                this.colMUnidad});
            this.dgvModificaciones.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvModificaciones.Location = new System.Drawing.Point(8, 24);
            this.dgvModificaciones.Name = "dgvModificaciones";
            this.dgvModificaciones.ReadOnly = true;
            this.dgvModificaciones.RowHeadersVisible = false;
            this.dgvModificaciones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvModificaciones.Size = new System.Drawing.Size(1124, 98);
            this.dgvModificaciones.TabIndex = 0;
            //
            // colMFecha
            //
            this.colMFecha.DataPropertyName = "Fecha";
            this.colMFecha.DefaultCellStyle.Format = "dd/MM/yyyy HH:mm";
            this.colMFecha.HeaderText = "Fecha";
            this.colMFecha.Name = "colMFecha";
            this.colMFecha.ReadOnly = true;
            this.colMFecha.Width = 125;
            //
            // colMReceta
            //
            this.colMReceta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colMReceta.DataPropertyName = "NombreReceta";
            this.colMReceta.HeaderText = "Receta";
            this.colMReceta.Name = "colMReceta";
            this.colMReceta.ReadOnly = true;
            //
            // colMTipo
            //
            this.colMTipo.DataPropertyName = "Tipo";
            this.colMTipo.HeaderText = "Tipo";
            this.colMTipo.Name = "colMTipo";
            this.colMTipo.ReadOnly = true;
            this.colMTipo.Width = 90;
            //
            // colMOriginal
            //
            this.colMOriginal.DataPropertyName = "IngredienteOriginal";
            this.colMOriginal.HeaderText = "Original";
            this.colMOriginal.Name = "colMOriginal";
            this.colMOriginal.ReadOnly = true;
            this.colMOriginal.Width = 160;
            //
            // colMReemplazo
            //
            this.colMReemplazo.DataPropertyName = "IngredienteReemplazo";
            this.colMReemplazo.HeaderText = "Reemplazo";
            this.colMReemplazo.Name = "colMReemplazo";
            this.colMReemplazo.ReadOnly = true;
            this.colMReemplazo.Width = 160;
            //
            // colMCantidad
            //
            this.colMCantidad.DataPropertyName = "Cantidad";
            this.colMCantidad.DefaultCellStyle.Format = "0.####";
            this.colMCantidad.HeaderText = "Cant.";
            this.colMCantidad.Name = "colMCantidad";
            this.colMCantidad.ReadOnly = true;
            this.colMCantidad.Width = 70;
            //
            // colMUnidad
            //
            this.colMUnidad.DataPropertyName = "Unidad";
            this.colMUnidad.HeaderText = "Un.";
            this.colMUnidad.Name = "colMUnidad";
            this.colMUnidad.ReadOnly = true;
            this.colMUnidad.Width = 50;
            //
            // ucDashboardAdmin
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.gbModificaciones);
            this.Controls.Add(this.gbCosto);
            this.Controls.Add(this.gbMovimiento);
            this.Controls.Add(this.gbHistorial);
            this.Controls.Add(this.gbStockCritico);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ucDashboardAdmin";
            this.Size = new System.Drawing.Size(1180, 592);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockCritico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvModificaciones)).EndInit();
            this.gbStockCritico.ResumeLayout(false);
            this.gbHistorial.ResumeLayout(false);
            this.gbMovimiento.ResumeLayout(false);
            this.gbMovimiento.PerformLayout();
            this.gbCosto.ResumeLayout(false);
            this.gbCosto.PerformLayout();
            this.gbModificaciones.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbStockCritico;
        private System.Windows.Forms.DataGridView dgvStockCritico;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCMinimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCUnidad;
        private System.Windows.Forms.GroupBox gbHistorial;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHIngrediente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHUnidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHObs;
        private System.Windows.Forms.GroupBox gbMovimiento;
        private System.Windows.Forms.Label lblTipoMov;
        private System.Windows.Forms.ComboBox cboTipoMov;
        private System.Windows.Forms.Label lblIngredienteMov;
        private System.Windows.Forms.ComboBox cboIngredienteMov;
        private System.Windows.Forms.Label lblCantMov;
        private System.Windows.Forms.TextBox txtCantMov;
        private System.Windows.Forms.Label lblUnidadMov;
        private System.Windows.Forms.Label lblObs;
        private System.Windows.Forms.TextBox txtObs;
        private System.Windows.Forms.Button btnRegistrarMov;
        private System.Windows.Forms.GroupBox gbCosto;
        private System.Windows.Forms.Label lblRecetaCosto;
        private System.Windows.Forms.ComboBox cboRecetaCosto;
        private System.Windows.Forms.Label lblPorciones;
        private System.Windows.Forms.TextBox txtPorciones;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.Button btnGenerarCostoPDF;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.GroupBox gbModificaciones;
        private System.Windows.Forms.DataGridView dgvModificaciones;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMReceta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMReemplazo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMUnidad;
    }
}
