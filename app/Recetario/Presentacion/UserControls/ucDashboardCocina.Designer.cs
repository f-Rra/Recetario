namespace Presentacion.UserControls
{
    partial class ucDashboardCocina
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
            this.lblClasificacion = new System.Windows.Forms.Label();
            this.cboClasificacion = new System.Windows.Forms.ComboBox();
            this.lblComensales = new System.Windows.Forms.Label();
            this.nudComensales = new System.Windows.Forms.NumericUpDown();
            this.dgvRecetas = new System.Windows.Forms.DataGridView();
            this.colSeleccion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClasificacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPorciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.nudComensales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(151, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Recetas del día";
            //
            // lblClasificacion
            //
            this.lblClasificacion.AutoSize = true;
            this.lblClasificacion.Location = new System.Drawing.Point(20, 62);
            this.lblClasificacion.Name = "lblClasificacion";
            this.lblClasificacion.Size = new System.Drawing.Size(78, 15);
            this.lblClasificacion.TabIndex = 1;
            this.lblClasificacion.Text = "Clasificación";
            //
            // cboClasificacion
            //
            this.cboClasificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClasificacion.Location = new System.Drawing.Point(110, 59);
            this.cboClasificacion.Name = "cboClasificacion";
            this.cboClasificacion.Size = new System.Drawing.Size(220, 23);
            this.cboClasificacion.TabIndex = 2;
            this.cboClasificacion.SelectedIndexChanged += new System.EventHandler(this.cboClasificacion_SelectedIndexChanged);
            //
            // lblComensales
            //
            this.lblComensales.AutoSize = true;
            this.lblComensales.Location = new System.Drawing.Point(370, 62);
            this.lblComensales.Name = "lblComensales";
            this.lblComensales.Size = new System.Drawing.Size(72, 15);
            this.lblComensales.TabIndex = 3;
            this.lblComensales.Text = "Comensales";
            //
            // nudComensales
            //
            this.nudComensales.Location = new System.Drawing.Point(450, 59);
            this.nudComensales.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudComensales.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudComensales.Name = "nudComensales";
            this.nudComensales.Size = new System.Drawing.Size(80, 23);
            this.nudComensales.TabIndex = 4;
            this.nudComensales.Value = new decimal(new int[] { 4, 0, 0, 0 });
            //
            // dgvRecetas
            //
            this.dgvRecetas.AllowUserToAddRows = false;
            this.dgvRecetas.AllowUserToDeleteRows = false;
            this.dgvRecetas.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                | System.Windows.Forms.AnchorStyles.Left)
                | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRecetas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecetas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colSeleccion,
                this.colCodigo,
                this.colNombre,
                this.colClasificacion,
                this.colPorciones});
            this.dgvRecetas.Location = new System.Drawing.Point(20, 100);
            this.dgvRecetas.MultiSelect = false;
            this.dgvRecetas.Name = "dgvRecetas";
            this.dgvRecetas.RowHeadersVisible = false;
            this.dgvRecetas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecetas.Size = new System.Drawing.Size(860, 430);
            this.dgvRecetas.TabIndex = 5;
            //
            // colSeleccion
            //
            this.colSeleccion.HeaderText = "";
            this.colSeleccion.Name = "colSeleccion";
            this.colSeleccion.Width = 40;
            //
            // colCodigo
            //
            this.colCodigo.DataPropertyName = "Codigo";
            this.colCodigo.HeaderText = "Código";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.ReadOnly = true;
            this.colCodigo.Width = 100;
            //
            // colNombre
            //
            this.colNombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNombre.DataPropertyName = "Nombre";
            this.colNombre.HeaderText = "Nombre";
            this.colNombre.Name = "colNombre";
            this.colNombre.ReadOnly = true;
            //
            // colClasificacion
            //
            this.colClasificacion.DataPropertyName = "NombreClasificacion";
            this.colClasificacion.HeaderText = "Clasificación";
            this.colClasificacion.Name = "colClasificacion";
            this.colClasificacion.ReadOnly = true;
            this.colClasificacion.Width = 160;
            //
            // colPorciones
            //
            this.colPorciones.DataPropertyName = "PorcionesBase";
            this.colPorciones.HeaderText = "Porciones base";
            this.colPorciones.Name = "colPorciones";
            this.colPorciones.ReadOnly = true;
            this.colPorciones.Width = 120;
            //
            // ucDashboardCocina
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvRecetas);
            this.Controls.Add(this.nudComensales);
            this.Controls.Add(this.lblComensales);
            this.Controls.Add(this.cboClasificacion);
            this.Controls.Add(this.lblClasificacion);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ucDashboardCocina";
            this.Size = new System.Drawing.Size(900, 556);
            ((System.ComponentModel.ISupportInitialize)(this.nudComensales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblClasificacion;
        private System.Windows.Forms.ComboBox cboClasificacion;
        private System.Windows.Forms.Label lblComensales;
        private System.Windows.Forms.NumericUpDown nudComensales;
        private System.Windows.Forms.DataGridView dgvRecetas;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSeleccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClasificacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPorciones;
    }
}
