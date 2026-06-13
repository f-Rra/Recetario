namespace Presentacion
{
    partial class frmModificacion
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
            this.lblReceta = new System.Windows.Forms.Label();
            this.lblTipo = new System.Windows.Forms.Label();
            this.cboTipo = new System.Windows.Forms.ComboBox();
            this.lblOriginal = new System.Windows.Forms.Label();
            this.cboOriginal = new System.Windows.Forms.ComboBox();
            this.lblReemplazo = new System.Windows.Forms.Label();
            this.cboReemplazo = new System.Windows.Forms.ComboBox();
            this.lblCantidad = new System.Windows.Forms.Label();
            this.txtCantidad = new System.Windows.Forms.TextBox();
            this.lblUnidad = new System.Windows.Forms.Label();
            this.cboUnidad = new System.Windows.Forms.ComboBox();
            this.btnAgregarMod = new System.Windows.Forms.Button();
            this.dgvMods = new System.Windows.Forms.DataGridView();
            this.colModTipo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModOriginal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModReemplazo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModCantidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnQuitarMod = new System.Windows.Forms.Button();
            this.btnAceptar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMods)).BeginInit();
            this.SuspendLayout();
            //
            // lblReceta
            //
            this.lblReceta.AutoSize = true;
            this.lblReceta.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblReceta.Location = new System.Drawing.Point(12, 12);
            this.lblReceta.Name = "lblReceta";
            this.lblReceta.Size = new System.Drawing.Size(57, 20);
            this.lblReceta.TabIndex = 0;
            this.lblReceta.Text = "Receta";
            //
            // lblTipo
            //
            this.lblTipo.AutoSize = true;
            this.lblTipo.Location = new System.Drawing.Point(12, 52);
            this.lblTipo.Name = "lblTipo";
            this.lblTipo.Size = new System.Drawing.Size(30, 15);
            this.lblTipo.TabIndex = 1;
            this.lblTipo.Text = "Tipo";
            //
            // cboTipo
            //
            this.cboTipo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTipo.Location = new System.Drawing.Point(130, 49);
            this.cboTipo.Name = "cboTipo";
            this.cboTipo.Size = new System.Drawing.Size(180, 23);
            this.cboTipo.TabIndex = 2;
            this.cboTipo.SelectedIndexChanged += new System.EventHandler(this.cboTipo_SelectedIndexChanged);
            //
            // lblOriginal
            //
            this.lblOriginal.AutoSize = true;
            this.lblOriginal.Location = new System.Drawing.Point(12, 90);
            this.lblOriginal.Name = "lblOriginal";
            this.lblOriginal.Size = new System.Drawing.Size(99, 15);
            this.lblOriginal.TabIndex = 3;
            this.lblOriginal.Text = "Ingr. original";
            //
            // cboOriginal
            //
            this.cboOriginal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboOriginal.Location = new System.Drawing.Point(130, 87);
            this.cboOriginal.Name = "cboOriginal";
            this.cboOriginal.Size = new System.Drawing.Size(300, 23);
            this.cboOriginal.TabIndex = 4;
            this.cboOriginal.SelectedIndexChanged += new System.EventHandler(this.cboOriginal_SelectedIndexChanged);
            //
            // lblReemplazo
            //
            this.lblReemplazo.AutoSize = true;
            this.lblReemplazo.Location = new System.Drawing.Point(12, 125);
            this.lblReemplazo.Name = "lblReemplazo";
            this.lblReemplazo.Size = new System.Drawing.Size(110, 15);
            this.lblReemplazo.TabIndex = 5;
            this.lblReemplazo.Text = "Ingr. reemplazo";
            //
            // cboReemplazo
            //
            this.cboReemplazo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboReemplazo.Location = new System.Drawing.Point(130, 122);
            this.cboReemplazo.Name = "cboReemplazo";
            this.cboReemplazo.Size = new System.Drawing.Size(300, 23);
            this.cboReemplazo.TabIndex = 6;
            this.cboReemplazo.SelectedIndexChanged += new System.EventHandler(this.cboReemplazo_SelectedIndexChanged);
            //
            // lblCantidad
            //
            this.lblCantidad.AutoSize = true;
            this.lblCantidad.Location = new System.Drawing.Point(12, 160);
            this.lblCantidad.Name = "lblCantidad";
            this.lblCantidad.Size = new System.Drawing.Size(58, 15);
            this.lblCantidad.TabIndex = 7;
            this.lblCantidad.Text = "Cantidad";
            //
            // txtCantidad
            //
            this.txtCantidad.Location = new System.Drawing.Point(130, 157);
            this.txtCantidad.Name = "txtCantidad";
            this.txtCantidad.Size = new System.Drawing.Size(90, 23);
            this.txtCantidad.TabIndex = 8;
            //
            // lblUnidad
            //
            this.lblUnidad.AutoSize = true;
            this.lblUnidad.Location = new System.Drawing.Point(240, 160);
            this.lblUnidad.Name = "lblUnidad";
            this.lblUnidad.Size = new System.Drawing.Size(45, 15);
            this.lblUnidad.TabIndex = 9;
            this.lblUnidad.Text = "Unidad";
            //
            // cboUnidad
            //
            this.cboUnidad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnidad.Location = new System.Drawing.Point(310, 157);
            this.cboUnidad.Name = "cboUnidad";
            this.cboUnidad.Size = new System.Drawing.Size(120, 23);
            this.cboUnidad.TabIndex = 10;
            //
            // btnAgregarMod
            //
            this.btnAgregarMod.Location = new System.Drawing.Point(130, 192);
            this.btnAgregarMod.Name = "btnAgregarMod";
            this.btnAgregarMod.Size = new System.Drawing.Size(180, 28);
            this.btnAgregarMod.TabIndex = 11;
            this.btnAgregarMod.Text = "Agregar modificación";
            this.btnAgregarMod.UseVisualStyleBackColor = true;
            this.btnAgregarMod.Click += new System.EventHandler(this.btnAgregarMod_Click);
            //
            // dgvMods
            //
            this.dgvMods.AllowUserToAddRows = false;
            this.dgvMods.AllowUserToDeleteRows = false;
            this.dgvMods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colModTipo,
                this.colModOriginal,
                this.colModReemplazo,
                this.colModCantidad,
                this.colModUnidad});
            this.dgvMods.Location = new System.Drawing.Point(12, 232);
            this.dgvMods.MultiSelect = false;
            this.dgvMods.Name = "dgvMods";
            this.dgvMods.ReadOnly = true;
            this.dgvMods.RowHeadersVisible = false;
            this.dgvMods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMods.Size = new System.Drawing.Size(530, 160);
            this.dgvMods.TabIndex = 12;
            //
            // colModTipo
            //
            this.colModTipo.DataPropertyName = "Tipo";
            this.colModTipo.HeaderText = "Tipo";
            this.colModTipo.Name = "colModTipo";
            this.colModTipo.ReadOnly = true;
            this.colModTipo.Width = 90;
            //
            // colModOriginal
            //
            this.colModOriginal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colModOriginal.DataPropertyName = "IngredienteOriginal";
            this.colModOriginal.HeaderText = "Original";
            this.colModOriginal.Name = "colModOriginal";
            this.colModOriginal.ReadOnly = true;
            //
            // colModReemplazo
            //
            this.colModReemplazo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colModReemplazo.DataPropertyName = "IngredienteReemplazo";
            this.colModReemplazo.HeaderText = "Reemplazo";
            this.colModReemplazo.Name = "colModReemplazo";
            this.colModReemplazo.ReadOnly = true;
            //
            // colModCantidad
            //
            this.colModCantidad.DataPropertyName = "Cantidad";
            this.colModCantidad.HeaderText = "Cant.";
            this.colModCantidad.Name = "colModCantidad";
            this.colModCantidad.ReadOnly = true;
            this.colModCantidad.Width = 70;
            //
            // colModUnidad
            //
            this.colModUnidad.DataPropertyName = "Abreviatura";
            this.colModUnidad.HeaderText = "Unidad";
            this.colModUnidad.Name = "colModUnidad";
            this.colModUnidad.ReadOnly = true;
            this.colModUnidad.Width = 60;
            //
            // btnQuitarMod
            //
            this.btnQuitarMod.Location = new System.Drawing.Point(12, 400);
            this.btnQuitarMod.Name = "btnQuitarMod";
            this.btnQuitarMod.Size = new System.Drawing.Size(120, 28);
            this.btnQuitarMod.TabIndex = 13;
            this.btnQuitarMod.Text = "Quitar";
            this.btnQuitarMod.UseVisualStyleBackColor = true;
            this.btnQuitarMod.Click += new System.EventHandler(this.btnQuitarMod_Click);
            //
            // btnAceptar
            //
            this.btnAceptar.Location = new System.Drawing.Point(381, 400);
            this.btnAceptar.Name = "btnAceptar";
            this.btnAceptar.Size = new System.Drawing.Size(75, 28);
            this.btnAceptar.TabIndex = 14;
            this.btnAceptar.Text = "Aceptar";
            this.btnAceptar.UseVisualStyleBackColor = true;
            this.btnAceptar.Click += new System.EventHandler(this.btnAceptar_Click);
            //
            // btnCancelar
            //
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelar.Location = new System.Drawing.Point(467, 400);
            this.btnCancelar.Name = "btnCancelar";
            this.btnCancelar.Size = new System.Drawing.Size(75, 28);
            this.btnCancelar.TabIndex = 15;
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.UseVisualStyleBackColor = true;
            //
            // frmModificacion
            //
            this.AcceptButton = this.btnAceptar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancelar;
            this.ClientSize = new System.Drawing.Size(554, 444);
            this.Controls.Add(this.btnCancelar);
            this.Controls.Add(this.btnAceptar);
            this.Controls.Add(this.btnQuitarMod);
            this.Controls.Add(this.dgvMods);
            this.Controls.Add(this.btnAgregarMod);
            this.Controls.Add(this.cboUnidad);
            this.Controls.Add(this.lblUnidad);
            this.Controls.Add(this.txtCantidad);
            this.Controls.Add(this.lblCantidad);
            this.Controls.Add(this.cboReemplazo);
            this.Controls.Add(this.lblReemplazo);
            this.Controls.Add(this.cboOriginal);
            this.Controls.Add(this.lblOriginal);
            this.Controls.Add(this.cboTipo);
            this.Controls.Add(this.lblTipo);
            this.Controls.Add(this.lblReceta);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmModificacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Modificaciones de la receta";
            ((System.ComponentModel.ISupportInitialize)(this.dgvMods)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblReceta;
        private System.Windows.Forms.Label lblTipo;
        private System.Windows.Forms.ComboBox cboTipo;
        private System.Windows.Forms.Label lblOriginal;
        private System.Windows.Forms.ComboBox cboOriginal;
        private System.Windows.Forms.Label lblReemplazo;
        private System.Windows.Forms.ComboBox cboReemplazo;
        private System.Windows.Forms.Label lblCantidad;
        private System.Windows.Forms.TextBox txtCantidad;
        private System.Windows.Forms.Label lblUnidad;
        private System.Windows.Forms.ComboBox cboUnidad;
        private System.Windows.Forms.Button btnAgregarMod;
        private System.Windows.Forms.DataGridView dgvMods;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModTipo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModOriginal;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModReemplazo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModCantidad;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModUnidad;
        private System.Windows.Forms.Button btnQuitarMod;
        private System.Windows.Forms.Button btnAceptar;
        private System.Windows.Forms.Button btnCancelar;
    }
}
