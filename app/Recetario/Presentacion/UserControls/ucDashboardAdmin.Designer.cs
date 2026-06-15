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
            this.lblStockCritico = new System.Windows.Forms.Label();
            this.dgvStockCritico = new System.Windows.Forms.DataGridView();
            this.colSCCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCDescripcion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCActual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCMinimo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSCUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblMovimiento = new System.Windows.Forms.Label();
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
            this.lblCosto = new System.Windows.Forms.Label();
            this.lblRecetaCosto = new System.Windows.Forms.Label();
            this.cboRecetaCosto = new System.Windows.Forms.ComboBox();
            this.lblPorciones = new System.Windows.Forms.Label();
            this.txtPorciones = new System.Windows.Forms.TextBox();
            this.btnCalcular = new System.Windows.Forms.Button();
            this.lblResultado = new System.Windows.Forms.Label();
            this.lblHistorial = new System.Windows.Forms.Label();
            this.dgvHistorial = new System.Windows.Forms.DataGridView();
            this.colHFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHIngrediente = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHObs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockCritico)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(232, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Panel de administración";
            //
            // lblStockCritico
            //
            this.lblStockCritico.AutoSize = true;
            this.lblStockCritico.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblStockCritico.Location = new System.Drawing.Point(20, 48);
            this.lblStockCritico.Name = "lblStockCritico";
            this.lblStockCritico.Size = new System.Drawing.Size(82, 17);
            this.lblStockCritico.TabIndex = 1;
            this.lblStockCritico.Text = "Stock crítico";
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
            this.dgvStockCritico.Location = new System.Drawing.Point(20, 70);
            this.dgvStockCritico.Name = "dgvStockCritico";
            this.dgvStockCritico.ReadOnly = true;
            this.dgvStockCritico.RowHeadersVisible = false;
            this.dgvStockCritico.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockCritico.Size = new System.Drawing.Size(860, 130);
            this.dgvStockCritico.TabIndex = 2;
            //
            // colSCCodigo
            //
            this.colSCCodigo.DataPropertyName = "Codigo";
            this.colSCCodigo.HeaderText = "Código";
            this.colSCCodigo.Name = "colSCCodigo";
            this.colSCCodigo.ReadOnly = true;
            this.colSCCodigo.Width = 100;
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
            this.colSCActual.HeaderText = "Stock actual";
            this.colSCActual.Name = "colSCActual";
            this.colSCActual.ReadOnly = true;
            this.colSCActual.Width = 110;
            //
            // colSCMinimo
            //
            this.colSCMinimo.DataPropertyName = "StockMinimo";
            this.colSCMinimo.HeaderText = "Stock mínimo";
            this.colSCMinimo.Name = "colSCMinimo";
            this.colSCMinimo.ReadOnly = true;
            this.colSCMinimo.Width = 110;
            //
            // colSCUnidad
            //
            this.colSCUnidad.DataPropertyName = "Abreviatura";
            this.colSCUnidad.HeaderText = "Unidad";
            this.colSCUnidad.Name = "colSCUnidad";
            this.colSCUnidad.ReadOnly = true;
            this.colSCUnidad.Width = 70;
            //
            // lblMovimiento
            //
            this.lblMovimiento.AutoSize = true;
            this.lblMovimiento.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblMovimiento.Location = new System.Drawing.Point(20, 215);
            this.lblMovimiento.Name = "lblMovimiento";
            this.lblMovimiento.Size = new System.Drawing.Size(199, 17);
            this.lblMovimiento.TabIndex = 3;
            this.lblMovimiento.Text = "Registrar movimiento de stock";
            //
            // lblTipoMov
            //
            this.lblTipoMov.AutoSize = true;
            this.lblTipoMov.Location = new System.Drawing.Point(20, 248);
            this.lblTipoMov.Name = "lblTipoMov";
            this.lblTipoMov.Size = new System.Drawing.Size(30, 15);
            this.lblTipoMov.TabIndex = 4;
            this.lblTipoMov.Text = "Tipo";
            //
            // cboTipoMov
            //
            this.cboTipoMov.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipoMov.Location = new System.Drawing.Point(95, 245);
            this.cboTipoMov.Name = "cboTipoMov";
            this.cboTipoMov.Size = new System.Drawing.Size(120, 23);
            this.cboTipoMov.TabIndex = 5;
            //
            // lblIngredienteMov
            //
            this.lblIngredienteMov.AutoSize = true;
            this.lblIngredienteMov.Location = new System.Drawing.Point(235, 248);
            this.lblIngredienteMov.Name = "lblIngredienteMov";
            this.lblIngredienteMov.Size = new System.Drawing.Size(67, 15);
            this.lblIngredienteMov.TabIndex = 6;
            this.lblIngredienteMov.Text = "Ingrediente";
            //
            // cboIngredienteMov
            //
            this.cboIngredienteMov.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIngredienteMov.Location = new System.Drawing.Point(320, 245);
            this.cboIngredienteMov.Name = "cboIngredienteMov";
            this.cboIngredienteMov.Size = new System.Drawing.Size(200, 23);
            this.cboIngredienteMov.TabIndex = 7;
            this.cboIngredienteMov.SelectedIndexChanged += new System.EventHandler(this.cboIngredienteMov_SelectedIndexChanged);
            //
            // lblCantMov
            //
            this.lblCantMov.AutoSize = true;
            this.lblCantMov.Location = new System.Drawing.Point(540, 248);
            this.lblCantMov.Name = "lblCantMov";
            this.lblCantMov.Size = new System.Drawing.Size(58, 15);
            this.lblCantMov.TabIndex = 8;
            this.lblCantMov.Text = "Cantidad";
            //
            // txtCantMov
            //
            this.txtCantMov.Location = new System.Drawing.Point(610, 245);
            this.txtCantMov.Name = "txtCantMov";
            this.txtCantMov.Size = new System.Drawing.Size(70, 23);
            this.txtCantMov.TabIndex = 9;
            //
            // lblUnidadMov
            //
            this.lblUnidadMov.AutoSize = true;
            this.lblUnidadMov.Location = new System.Drawing.Point(688, 248);
            this.lblUnidadMov.Name = "lblUnidadMov";
            this.lblUnidadMov.Size = new System.Drawing.Size(12, 15);
            this.lblUnidadMov.TabIndex = 10;
            this.lblUnidadMov.Text = "-";
            //
            // lblObs
            //
            this.lblObs.AutoSize = true;
            this.lblObs.Location = new System.Drawing.Point(20, 281);
            this.lblObs.Name = "lblObs";
            this.lblObs.Size = new System.Drawing.Size(83, 15);
            this.lblObs.TabIndex = 11;
            this.lblObs.Text = "Observaciones";
            //
            // txtObs
            //
            this.txtObs.Location = new System.Drawing.Point(110, 278);
            this.txtObs.Name = "txtObs";
            this.txtObs.Size = new System.Drawing.Size(410, 23);
            this.txtObs.TabIndex = 12;
            //
            // btnRegistrarMov
            //
            this.btnRegistrarMov.Location = new System.Drawing.Point(540, 277);
            this.btnRegistrarMov.Name = "btnRegistrarMov";
            this.btnRegistrarMov.Size = new System.Drawing.Size(180, 27);
            this.btnRegistrarMov.TabIndex = 13;
            this.btnRegistrarMov.Text = "Registrar movimiento";
            this.btnRegistrarMov.UseVisualStyleBackColor = true;
            this.btnRegistrarMov.Click += new System.EventHandler(this.btnRegistrarMov_Click);
            //
            // lblCosto
            //
            this.lblCosto.AutoSize = true;
            this.lblCosto.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCosto.Location = new System.Drawing.Point(20, 320);
            this.lblCosto.Name = "lblCosto";
            this.lblCosto.Size = new System.Drawing.Size(165, 17);
            this.lblCosto.TabIndex = 14;
            this.lblCosto.Text = "Calcular costo de receta";
            //
            // lblRecetaCosto
            //
            this.lblRecetaCosto.AutoSize = true;
            this.lblRecetaCosto.Location = new System.Drawing.Point(20, 353);
            this.lblRecetaCosto.Name = "lblRecetaCosto";
            this.lblRecetaCosto.Size = new System.Drawing.Size(43, 15);
            this.lblRecetaCosto.TabIndex = 15;
            this.lblRecetaCosto.Text = "Receta";
            //
            // cboRecetaCosto
            //
            this.cboRecetaCosto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRecetaCosto.Location = new System.Drawing.Point(95, 350);
            this.cboRecetaCosto.Name = "cboRecetaCosto";
            this.cboRecetaCosto.Size = new System.Drawing.Size(220, 23);
            this.cboRecetaCosto.TabIndex = 16;
            //
            // lblPorciones
            //
            this.lblPorciones.AutoSize = true;
            this.lblPorciones.Location = new System.Drawing.Point(335, 353);
            this.lblPorciones.Name = "lblPorciones";
            this.lblPorciones.Size = new System.Drawing.Size(61, 15);
            this.lblPorciones.TabIndex = 17;
            this.lblPorciones.Text = "Porciones";
            //
            // txtPorciones
            //
            this.txtPorciones.Location = new System.Drawing.Point(410, 350);
            this.txtPorciones.Name = "txtPorciones";
            this.txtPorciones.Size = new System.Drawing.Size(60, 23);
            this.txtPorciones.TabIndex = 18;
            //
            // btnCalcular
            //
            this.btnCalcular.Location = new System.Drawing.Point(490, 349);
            this.btnCalcular.Name = "btnCalcular";
            this.btnCalcular.Size = new System.Drawing.Size(110, 27);
            this.btnCalcular.TabIndex = 19;
            this.btnCalcular.Text = "Calcular";
            this.btnCalcular.UseVisualStyleBackColor = true;
            this.btnCalcular.Click += new System.EventHandler(this.btnCalcular_Click);
            //
            // lblResultado
            //
            this.lblResultado.AutoSize = true;
            this.lblResultado.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblResultado.Location = new System.Drawing.Point(620, 354);
            this.lblResultado.Name = "lblResultado";
            this.lblResultado.Size = new System.Drawing.Size(0, 17);
            this.lblResultado.TabIndex = 20;
            //
            // lblHistorial
            //
            this.lblHistorial.AutoSize = true;
            this.lblHistorial.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblHistorial.Location = new System.Drawing.Point(20, 390);
            this.lblHistorial.Name = "lblHistorial";
            this.lblHistorial.Size = new System.Drawing.Size(167, 17);
            this.lblHistorial.TabIndex = 21;
            this.lblHistorial.Text = "Historial de movimientos";
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
            this.dgvHistorial.Location = new System.Drawing.Point(20, 412);
            this.dgvHistorial.Name = "dgvHistorial";
            this.dgvHistorial.ReadOnly = true;
            this.dgvHistorial.RowHeadersVisible = false;
            this.dgvHistorial.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHistorial.Size = new System.Drawing.Size(860, 150);
            this.dgvHistorial.TabIndex = 22;
            //
            // colHFecha
            //
            this.colHFecha.DataPropertyName = "Fecha";
            this.colHFecha.HeaderText = "Fecha";
            this.colHFecha.Name = "colHFecha";
            this.colHFecha.ReadOnly = true;
            this.colHFecha.Width = 140;
            //
            // colHIngrediente
            //
            this.colHIngrediente.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colHIngrediente.DataPropertyName = "NombreIngrediente";
            this.colHIngrediente.HeaderText = "Ingrediente";
            this.colHIngrediente.Name = "colHIngrediente";
            this.colHIngrediente.ReadOnly = true;
            //
            // colHTipo
            //
            this.colHTipo.DataPropertyName = "TipoMovimiento";
            this.colHTipo.HeaderText = "Tipo";
            this.colHTipo.Name = "colHTipo";
            this.colHTipo.ReadOnly = true;
            this.colHTipo.Width = 90;
            //
            // colHCantidad
            //
            this.colHCantidad.DataPropertyName = "Cantidad";
            this.colHCantidad.HeaderText = "Cantidad";
            this.colHCantidad.Name = "colHCantidad";
            this.colHCantidad.ReadOnly = true;
            this.colHCantidad.Width = 90;
            //
            // colHUnidad
            //
            this.colHUnidad.DataPropertyName = "Abreviatura";
            this.colHUnidad.HeaderText = "Unidad";
            this.colHUnidad.Name = "colHUnidad";
            this.colHUnidad.ReadOnly = true;
            this.colHUnidad.Width = 70;
            //
            // colHObs
            //
            this.colHObs.DataPropertyName = "Observaciones";
            this.colHObs.HeaderText = "Observaciones";
            this.colHObs.Name = "colHObs";
            this.colHObs.ReadOnly = true;
            this.colHObs.Width = 200;
            //
            // ucDashboardAdmin
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.dgvHistorial);
            this.Controls.Add(this.lblHistorial);
            this.Controls.Add(this.lblResultado);
            this.Controls.Add(this.btnCalcular);
            this.Controls.Add(this.txtPorciones);
            this.Controls.Add(this.lblPorciones);
            this.Controls.Add(this.cboRecetaCosto);
            this.Controls.Add(this.lblRecetaCosto);
            this.Controls.Add(this.lblCosto);
            this.Controls.Add(this.btnRegistrarMov);
            this.Controls.Add(this.txtObs);
            this.Controls.Add(this.lblObs);
            this.Controls.Add(this.lblUnidadMov);
            this.Controls.Add(this.txtCantMov);
            this.Controls.Add(this.lblCantMov);
            this.Controls.Add(this.cboIngredienteMov);
            this.Controls.Add(this.lblIngredienteMov);
            this.Controls.Add(this.cboTipoMov);
            this.Controls.Add(this.lblTipoMov);
            this.Controls.Add(this.lblMovimiento);
            this.Controls.Add(this.dgvStockCritico);
            this.Controls.Add(this.lblStockCritico);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ucDashboardAdmin";
            this.Size = new System.Drawing.Size(900, 556);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockCritico)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHistorial)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblStockCritico;
        private System.Windows.Forms.DataGridView dgvStockCritico;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCDescripcion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCActual;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCMinimo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSCUnidad;
        private System.Windows.Forms.Label lblMovimiento;
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
        private System.Windows.Forms.Label lblCosto;
        private System.Windows.Forms.Label lblRecetaCosto;
        private System.Windows.Forms.ComboBox cboRecetaCosto;
        private System.Windows.Forms.Label lblPorciones;
        private System.Windows.Forms.TextBox txtPorciones;
        private System.Windows.Forms.Button btnCalcular;
        private System.Windows.Forms.Label lblResultado;
        private System.Windows.Forms.Label lblHistorial;
        private System.Windows.Forms.DataGridView dgvHistorial;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHFecha;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHIngrediente;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHUnidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHObs;
    }
}
