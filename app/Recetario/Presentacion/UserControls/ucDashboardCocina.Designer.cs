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
            this.lblCatalogo = new System.Windows.Forms.Label();
            this.dgvRecetas = new System.Windows.Forms.DataGridView();
            this.colSeleccion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClasificacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.lblComanda = new System.Windows.Forms.Label();
            this.dgvComanda = new System.Windows.Forms.DataGridView();
            this.colItemReceta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemClasificacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemMods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnGenerar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudComensales)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComanda)).BeginInit();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(178, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Comanda de cocina";
            //
            // lblClasificacion
            //
            this.lblClasificacion.AutoSize = true;
            this.lblClasificacion.Location = new System.Drawing.Point(20, 53);
            this.lblClasificacion.Name = "lblClasificacion";
            this.lblClasificacion.Size = new System.Drawing.Size(78, 15);
            this.lblClasificacion.TabIndex = 1;
            this.lblClasificacion.Text = "Clasificación";
            //
            // cboClasificacion
            //
            this.cboClasificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboClasificacion.Location = new System.Drawing.Point(110, 50);
            this.cboClasificacion.Name = "cboClasificacion";
            this.cboClasificacion.Size = new System.Drawing.Size(200, 23);
            this.cboClasificacion.TabIndex = 2;
            this.cboClasificacion.SelectedIndexChanged += new System.EventHandler(this.cboClasificacion_SelectedIndexChanged);
            //
            // lblComensales
            //
            this.lblComensales.AutoSize = true;
            this.lblComensales.Location = new System.Drawing.Point(470, 53);
            this.lblComensales.Name = "lblComensales";
            this.lblComensales.Size = new System.Drawing.Size(72, 15);
            this.lblComensales.TabIndex = 3;
            this.lblComensales.Text = "Comensales";
            //
            // nudComensales
            //
            this.nudComensales.Location = new System.Drawing.Point(550, 50);
            this.nudComensales.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            this.nudComensales.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            this.nudComensales.Name = "nudComensales";
            this.nudComensales.Size = new System.Drawing.Size(80, 23);
            this.nudComensales.TabIndex = 4;
            this.nudComensales.Value = new decimal(new int[] { 4, 0, 0, 0 });
            //
            // lblCatalogo
            //
            this.lblCatalogo.AutoSize = true;
            this.lblCatalogo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCatalogo.Location = new System.Drawing.Point(20, 85);
            this.lblCatalogo.Name = "lblCatalogo";
            this.lblCatalogo.Size = new System.Drawing.Size(127, 17);
            this.lblCatalogo.TabIndex = 5;
            this.lblCatalogo.Text = "Recetas disponibles";
            //
            // dgvRecetas
            //
            this.dgvRecetas.AllowUserToAddRows = false;
            this.dgvRecetas.AllowUserToDeleteRows = false;
            this.dgvRecetas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecetas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colSeleccion,
                this.colCodigo,
                this.colNombre,
                this.colClasificacion});
            this.dgvRecetas.Location = new System.Drawing.Point(20, 105);
            this.dgvRecetas.MultiSelect = false;
            this.dgvRecetas.Name = "dgvRecetas";
            this.dgvRecetas.RowHeadersVisible = false;
            this.dgvRecetas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecetas.Size = new System.Drawing.Size(420, 360);
            this.dgvRecetas.TabIndex = 6;
            //
            // colSeleccion
            //
            this.colSeleccion.HeaderText = "";
            this.colSeleccion.Name = "colSeleccion";
            this.colSeleccion.Width = 30;
            //
            // colCodigo
            //
            this.colCodigo.DataPropertyName = "Codigo";
            this.colCodigo.HeaderText = "Código";
            this.colCodigo.Name = "colCodigo";
            this.colCodigo.ReadOnly = true;
            this.colCodigo.Width = 90;
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
            this.colClasificacion.DataPropertyName = "Clasificacion";
            this.colClasificacion.HeaderText = "Clasificación";
            this.colClasificacion.Name = "colClasificacion";
            this.colClasificacion.ReadOnly = true;
            this.colClasificacion.Width = 120;
            //
            // btnAgregar
            //
            this.btnAgregar.Location = new System.Drawing.Point(20, 472);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(420, 30);
            this.btnAgregar.TabIndex = 7;
            this.btnAgregar.Text = "Agregar a la comanda →";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            //
            // lblComanda
            //
            this.lblComanda.AutoSize = true;
            this.lblComanda.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblComanda.Location = new System.Drawing.Point(460, 85);
            this.lblComanda.Name = "lblComanda";
            this.lblComanda.Size = new System.Drawing.Size(67, 17);
            this.lblComanda.TabIndex = 8;
            this.lblComanda.Text = "Comanda";
            //
            // dgvComanda
            //
            this.dgvComanda.AllowUserToAddRows = false;
            this.dgvComanda.AllowUserToDeleteRows = false;
            this.dgvComanda.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvComanda.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colItemReceta,
                this.colItemClasificacion,
                this.colItemMods});
            this.dgvComanda.Location = new System.Drawing.Point(460, 105);
            this.dgvComanda.MultiSelect = false;
            this.dgvComanda.Name = "dgvComanda";
            this.dgvComanda.ReadOnly = true;
            this.dgvComanda.RowHeadersVisible = false;
            this.dgvComanda.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComanda.Size = new System.Drawing.Size(420, 320);
            this.dgvComanda.TabIndex = 9;
            //
            // colItemReceta
            //
            this.colItemReceta.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colItemReceta.DataPropertyName = "NombreReceta";
            this.colItemReceta.HeaderText = "Receta";
            this.colItemReceta.Name = "colItemReceta";
            this.colItemReceta.ReadOnly = true;
            //
            // colItemClasificacion
            //
            this.colItemClasificacion.DataPropertyName = "Clasificacion";
            this.colItemClasificacion.HeaderText = "Clasificación";
            this.colItemClasificacion.Name = "colItemClasificacion";
            this.colItemClasificacion.ReadOnly = true;
            this.colItemClasificacion.Width = 120;
            //
            // colItemMods
            //
            this.colItemMods.DataPropertyName = "CantidadModificaciones";
            this.colItemMods.HeaderText = "Modif.";
            this.colItemMods.Name = "colItemMods";
            this.colItemMods.ReadOnly = true;
            this.colItemMods.Width = 55;
            //
            // btnModificar
            //
            this.btnModificar.Location = new System.Drawing.Point(460, 432);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(205, 30);
            this.btnModificar.TabIndex = 10;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            //
            // btnQuitar
            //
            this.btnQuitar.Location = new System.Drawing.Point(675, 432);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(205, 30);
            this.btnQuitar.TabIndex = 11;
            this.btnQuitar.Text = "Quitar de la comanda";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            //
            // btnGenerar
            //
            this.btnGenerar.Location = new System.Drawing.Point(460, 472);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(420, 30);
            this.btnGenerar.TabIndex = 12;
            this.btnGenerar.Text = "Generar comanda";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            //
            // ucDashboardCocina
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.btnQuitar);
            this.Controls.Add(this.btnModificar);
            this.Controls.Add(this.dgvComanda);
            this.Controls.Add(this.lblComanda);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.dgvRecetas);
            this.Controls.Add(this.lblCatalogo);
            this.Controls.Add(this.nudComensales);
            this.Controls.Add(this.lblComensales);
            this.Controls.Add(this.cboClasificacion);
            this.Controls.Add(this.lblClasificacion);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ucDashboardCocina";
            this.Size = new System.Drawing.Size(900, 556);
            ((System.ComponentModel.ISupportInitialize)(this.nudComensales)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComanda)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblClasificacion;
        private System.Windows.Forms.ComboBox cboClasificacion;
        private System.Windows.Forms.Label lblComensales;
        private System.Windows.Forms.NumericUpDown nudComensales;
        private System.Windows.Forms.Label lblCatalogo;
        private System.Windows.Forms.DataGridView dgvRecetas;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSeleccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClasificacion;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Label lblComanda;
        private System.Windows.Forms.DataGridView dgvComanda;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemReceta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemClasificacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemMods;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnGenerar;
    }
}
