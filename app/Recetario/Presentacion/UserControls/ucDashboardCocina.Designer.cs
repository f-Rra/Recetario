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
            this.txtComensales = new System.Windows.Forms.TextBox();
            this.gbRecetas = new System.Windows.Forms.GroupBox();
            this.lblCatalogo = new System.Windows.Forms.Label();
            this.dgvRecetas = new System.Windows.Forms.DataGridView();
            this.colSeleccion = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colClasificacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblIngredientesReceta = new System.Windows.Forms.Label();
            this.dgvIngredientes = new System.Windows.Forms.DataGridView();
            this.colIngNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIngCant = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIngUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.gbComanda = new System.Windows.Forms.GroupBox();
            this.dgvComanda = new System.Windows.Forms.DataGridView();
            this.colItemReceta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemClasificacion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colItemMods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnModificar = new System.Windows.Forms.Button();
            this.btnQuitar = new System.Windows.Forms.Button();
            this.btnGenerar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIngredientes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComanda)).BeginInit();
            this.gbRecetas.SuspendLayout();
            this.gbComanda.SuspendLayout();
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
            // txtComensales
            //
            this.txtComensales.Location = new System.Drawing.Point(550, 50);
            this.txtComensales.Name = "txtComensales";
            this.txtComensales.Size = new System.Drawing.Size(80, 23);
            this.txtComensales.TabIndex = 4;
            this.txtComensales.Text = "4";
            //
            // gbRecetas
            //
            this.gbRecetas.Controls.Add(this.lblCatalogo);
            this.gbRecetas.Controls.Add(this.dgvRecetas);
            this.gbRecetas.Controls.Add(this.lblIngredientesReceta);
            this.gbRecetas.Controls.Add(this.dgvIngredientes);
            this.gbRecetas.Controls.Add(this.btnAgregar);
            this.gbRecetas.Location = new System.Drawing.Point(20, 85);
            this.gbRecetas.Name = "gbRecetas";
            this.gbRecetas.Size = new System.Drawing.Size(565, 455);
            this.gbRecetas.TabIndex = 5;
            this.gbRecetas.TabStop = false;
            this.gbRecetas.Text = "Recetas e ingredientes";
            //
            // lblCatalogo
            //
            this.lblCatalogo.AutoSize = true;
            this.lblCatalogo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblCatalogo.Location = new System.Drawing.Point(10, 25);
            this.lblCatalogo.Name = "lblCatalogo";
            this.lblCatalogo.Size = new System.Drawing.Size(127, 17);
            this.lblCatalogo.TabIndex = 0;
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
            this.dgvRecetas.Location = new System.Drawing.Point(10, 45);
            this.dgvRecetas.MultiSelect = false;
            this.dgvRecetas.Name = "dgvRecetas";
            this.dgvRecetas.RowHeadersVisible = false;
            this.dgvRecetas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecetas.Size = new System.Drawing.Size(545, 195);
            this.dgvRecetas.TabIndex = 1;
            this.dgvRecetas.SelectionChanged += new System.EventHandler(this.dgvRecetas_SelectionChanged);
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
            this.colClasificacion.DataPropertyName = "NombreClasificacion";
            this.colClasificacion.HeaderText = "Clasificación";
            this.colClasificacion.Name = "colClasificacion";
            this.colClasificacion.ReadOnly = true;
            this.colClasificacion.Width = 110;
            //
            // lblIngredientesReceta
            //
            this.lblIngredientesReceta.AutoSize = true;
            this.lblIngredientesReceta.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold);
            this.lblIngredientesReceta.Location = new System.Drawing.Point(10, 248);
            this.lblIngredientesReceta.Name = "lblIngredientesReceta";
            this.lblIngredientesReceta.Size = new System.Drawing.Size(150, 17);
            this.lblIngredientesReceta.TabIndex = 2;
            this.lblIngredientesReceta.Text = "Ingredientes de la receta";
            //
            // dgvIngredientes
            //
            this.dgvIngredientes.AllowUserToAddRows = false;
            this.dgvIngredientes.AllowUserToDeleteRows = false;
            this.dgvIngredientes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIngredientes.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colIngNombre,
                this.colIngCant,
                this.colIngUnidad});
            this.dgvIngredientes.Location = new System.Drawing.Point(10, 268);
            this.dgvIngredientes.MultiSelect = false;
            this.dgvIngredientes.Name = "dgvIngredientes";
            this.dgvIngredientes.ReadOnly = true;
            this.dgvIngredientes.RowHeadersVisible = false;
            this.dgvIngredientes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIngredientes.Size = new System.Drawing.Size(545, 135);
            this.dgvIngredientes.TabIndex = 3;
            //
            // colIngNombre
            //
            this.colIngNombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIngNombre.DataPropertyName = "NombreIngrediente";
            this.colIngNombre.HeaderText = "Ingrediente";
            this.colIngNombre.Name = "colIngNombre";
            this.colIngNombre.ReadOnly = true;
            //
            // colIngCant
            //
            this.colIngCant.DataPropertyName = "CantNeta";
            this.colIngCant.DefaultCellStyle.Format = "0.####";
            this.colIngCant.HeaderText = "Cant.";
            this.colIngCant.Name = "colIngCant";
            this.colIngCant.ReadOnly = true;
            this.colIngCant.Width = 70;
            //
            // colIngUnidad
            //
            this.colIngUnidad.DataPropertyName = "Abreviatura";
            this.colIngUnidad.HeaderText = "Un.";
            this.colIngUnidad.Name = "colIngUnidad";
            this.colIngUnidad.ReadOnly = true;
            this.colIngUnidad.Width = 40;
            //
            // btnAgregar
            //
            this.btnAgregar.Location = new System.Drawing.Point(10, 412);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(545, 30);
            this.btnAgregar.TabIndex = 4;
            this.btnAgregar.Text = "Agregar a la comanda →";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            //
            // gbComanda
            //
            this.gbComanda.Controls.Add(this.dgvComanda);
            this.gbComanda.Controls.Add(this.btnModificar);
            this.gbComanda.Controls.Add(this.btnQuitar);
            this.gbComanda.Controls.Add(this.btnGenerar);
            this.gbComanda.Location = new System.Drawing.Point(600, 85);
            this.gbComanda.Name = "gbComanda";
            this.gbComanda.Size = new System.Drawing.Size(560, 455);
            this.gbComanda.TabIndex = 6;
            this.gbComanda.TabStop = false;
            this.gbComanda.Text = "Comanda";
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
            this.dgvComanda.Location = new System.Drawing.Point(10, 25);
            this.dgvComanda.MultiSelect = false;
            this.dgvComanda.Name = "dgvComanda";
            this.dgvComanda.ReadOnly = true;
            this.dgvComanda.RowHeadersVisible = false;
            this.dgvComanda.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvComanda.Size = new System.Drawing.Size(540, 345);
            this.dgvComanda.TabIndex = 0;
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
            this.colItemClasificacion.Width = 110;
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
            this.btnModificar.Location = new System.Drawing.Point(10, 378);
            this.btnModificar.Name = "btnModificar";
            this.btnModificar.Size = new System.Drawing.Size(265, 30);
            this.btnModificar.TabIndex = 1;
            this.btnModificar.Text = "Modificar";
            this.btnModificar.UseVisualStyleBackColor = true;
            this.btnModificar.Click += new System.EventHandler(this.btnModificar_Click);
            //
            // btnQuitar
            //
            this.btnQuitar.Location = new System.Drawing.Point(285, 378);
            this.btnQuitar.Name = "btnQuitar";
            this.btnQuitar.Size = new System.Drawing.Size(265, 30);
            this.btnQuitar.TabIndex = 2;
            this.btnQuitar.Text = "Quitar de la comanda";
            this.btnQuitar.UseVisualStyleBackColor = true;
            this.btnQuitar.Click += new System.EventHandler(this.btnQuitar_Click);
            //
            // btnGenerar
            //
            this.btnGenerar.Location = new System.Drawing.Point(10, 412);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(540, 30);
            this.btnGenerar.TabIndex = 3;
            this.btnGenerar.Text = "Generar comanda";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            //
            // ucDashboardCocina
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbComanda);
            this.Controls.Add(this.gbRecetas);
            this.Controls.Add(this.txtComensales);
            this.Controls.Add(this.lblComensales);
            this.Controls.Add(this.cboClasificacion);
            this.Controls.Add(this.lblClasificacion);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ucDashboardCocina";
            this.Size = new System.Drawing.Size(1180, 556);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIngredientes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvComanda)).EndInit();
            this.gbRecetas.ResumeLayout(false);
            this.gbRecetas.PerformLayout();
            this.gbComanda.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.Label lblClasificacion;
        private System.Windows.Forms.ComboBox cboClasificacion;
        private System.Windows.Forms.Label lblComensales;
        private System.Windows.Forms.TextBox txtComensales;
        private System.Windows.Forms.GroupBox gbRecetas;
        private System.Windows.Forms.Label lblCatalogo;
        private System.Windows.Forms.DataGridView dgvRecetas;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSeleccion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colClasificacion;
        private System.Windows.Forms.Label lblIngredientesReceta;
        private System.Windows.Forms.DataGridView dgvIngredientes;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIngNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIngCant;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIngUnidad;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.GroupBox gbComanda;
        private System.Windows.Forms.DataGridView dgvComanda;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemReceta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemClasificacion;
        private System.Windows.Forms.DataGridViewTextBoxColumn colItemMods;
        private System.Windows.Forms.Button btnModificar;
        private System.Windows.Forms.Button btnQuitar;
        private System.Windows.Forms.Button btnGenerar;
    }
}
