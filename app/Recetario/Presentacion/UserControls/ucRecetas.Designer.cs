namespace Presentacion.UserControls
{
    partial class ucRecetas
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
            this.gbRecetas = new System.Windows.Forms.GroupBox();
            this.dgvRecetas = new System.Windows.Forms.DataGridView();
            this.colRCodigo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRClasif = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRPorciones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.gbDatos = new System.Windows.Forms.GroupBox();
            this.lblRCodigo = new System.Windows.Forms.Label();
            this.txtRCodigo = new System.Windows.Forms.TextBox();
            this.lblRNombre = new System.Windows.Forms.Label();
            this.txtRNombre = new System.Windows.Forms.TextBox();
            this.lblRClasif = new System.Windows.Forms.Label();
            this.cboRClasificacion = new System.Windows.Forms.ComboBox();
            this.lblRPorciones = new System.Windows.Forms.Label();
            this.txtRPorciones = new System.Windows.Forms.TextBox();
            this.btnNuevo = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.gbIngredientes = new System.Windows.Forms.GroupBox();
            this.dgvIngRec = new System.Windows.Forms.DataGridView();
            this.colIRNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIRNeta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIRRend = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIRBruta = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIRUnidad = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblIng = new System.Windows.Forms.Label();
            this.cboIngrediente = new System.Windows.Forms.ComboBox();
            this.lblNeta = new System.Windows.Forms.Label();
            this.txtNeta = new System.Windows.Forms.TextBox();
            this.lblRend = new System.Windows.Forms.Label();
            this.txtRend = new System.Windows.Forms.TextBox();
            this.lblUniIng = new System.Windows.Forms.Label();
            this.btnAgregarIng = new System.Windows.Forms.Button();
            this.btnQuitarIng = new System.Windows.Forms.Button();
            this.gbProcedimiento = new System.Windows.Forms.GroupBox();
            this.dgvProc = new System.Windows.Forms.DataGridView();
            this.colPNro = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPDesc = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lblPaso = new System.Windows.Forms.Label();
            this.txtPaso = new System.Windows.Forms.TextBox();
            this.btnAgregarPaso = new System.Windows.Forms.Button();
            this.btnQuitarPaso = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIngRec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProc)).BeginInit();
            this.gbRecetas.SuspendLayout();
            this.gbDatos.SuspendLayout();
            this.gbIngredientes.SuspendLayout();
            this.gbProcedimiento.SuspendLayout();
            this.SuspendLayout();
            //
            // lblTitulo
            //
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 12);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(78, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Recetas";
            //
            // gbRecetas
            //
            this.gbRecetas.Controls.Add(this.dgvRecetas);
            this.gbRecetas.Location = new System.Drawing.Point(20, 45);
            this.gbRecetas.Name = "gbRecetas";
            this.gbRecetas.Padding = new System.Windows.Forms.Padding(8);
            this.gbRecetas.Size = new System.Drawing.Size(430, 240);
            this.gbRecetas.TabIndex = 1;
            this.gbRecetas.TabStop = false;
            this.gbRecetas.Text = "Recetas disponibles";
            //
            // dgvRecetas
            //
            this.dgvRecetas.AllowUserToAddRows = false;
            this.dgvRecetas.AllowUserToDeleteRows = false;
            this.dgvRecetas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecetas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colRCodigo,
                this.colRNombre,
                this.colRClasif,
                this.colRPorciones});
            this.dgvRecetas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecetas.Location = new System.Drawing.Point(8, 24);
            this.dgvRecetas.MultiSelect = false;
            this.dgvRecetas.Name = "dgvRecetas";
            this.dgvRecetas.ReadOnly = true;
            this.dgvRecetas.RowHeadersVisible = false;
            this.dgvRecetas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRecetas.Size = new System.Drawing.Size(414, 208);
            this.dgvRecetas.TabIndex = 0;
            this.dgvRecetas.SelectionChanged += new System.EventHandler(this.dgvRecetas_SelectionChanged);
            //
            // colRCodigo
            //
            this.colRCodigo.DataPropertyName = "Codigo";
            this.colRCodigo.HeaderText = "Código";
            this.colRCodigo.Name = "colRCodigo";
            this.colRCodigo.ReadOnly = true;
            this.colRCodigo.Width = 90;
            //
            // colRNombre
            //
            this.colRNombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colRNombre.DataPropertyName = "Nombre";
            this.colRNombre.HeaderText = "Nombre";
            this.colRNombre.Name = "colRNombre";
            this.colRNombre.ReadOnly = true;
            //
            // colRClasif
            //
            this.colRClasif.DataPropertyName = "NombreClasificacion";
            this.colRClasif.HeaderText = "Clasificación";
            this.colRClasif.Name = "colRClasif";
            this.colRClasif.ReadOnly = true;
            this.colRClasif.Width = 110;
            //
            // colRPorciones
            //
            this.colRPorciones.DataPropertyName = "PorcionesBase";
            this.colRPorciones.HeaderText = "Porc.";
            this.colRPorciones.Name = "colRPorciones";
            this.colRPorciones.ReadOnly = true;
            this.colRPorciones.Width = 55;
            //
            // gbDatos
            //
            this.gbDatos.Controls.Add(this.lblRCodigo);
            this.gbDatos.Controls.Add(this.txtRCodigo);
            this.gbDatos.Controls.Add(this.lblRNombre);
            this.gbDatos.Controls.Add(this.txtRNombre);
            this.gbDatos.Controls.Add(this.lblRClasif);
            this.gbDatos.Controls.Add(this.cboRClasificacion);
            this.gbDatos.Controls.Add(this.lblRPorciones);
            this.gbDatos.Controls.Add(this.txtRPorciones);
            this.gbDatos.Controls.Add(this.btnNuevo);
            this.gbDatos.Controls.Add(this.btnGuardar);
            this.gbDatos.Controls.Add(this.btnEliminar);
            this.gbDatos.Location = new System.Drawing.Point(458, 45);
            this.gbDatos.Name = "gbDatos";
            this.gbDatos.Size = new System.Drawing.Size(422, 240);
            this.gbDatos.TabIndex = 2;
            this.gbDatos.TabStop = false;
            this.gbDatos.Text = "Datos de la receta";
            //
            // lblRCodigo
            //
            this.lblRCodigo.AutoSize = true;
            this.lblRCodigo.Location = new System.Drawing.Point(15, 33);
            this.lblRCodigo.Name = "lblRCodigo";
            this.lblRCodigo.Size = new System.Drawing.Size(46, 15);
            this.lblRCodigo.TabIndex = 0;
            this.lblRCodigo.Text = "Código";
            //
            // txtRCodigo
            //
            this.txtRCodigo.Location = new System.Drawing.Point(120, 30);
            this.txtRCodigo.Name = "txtRCodigo";
            this.txtRCodigo.Size = new System.Drawing.Size(150, 23);
            this.txtRCodigo.TabIndex = 1;
            //
            // lblRNombre
            //
            this.lblRNombre.AutoSize = true;
            this.lblRNombre.Location = new System.Drawing.Point(15, 65);
            this.lblRNombre.Name = "lblRNombre";
            this.lblRNombre.Size = new System.Drawing.Size(51, 15);
            this.lblRNombre.TabIndex = 2;
            this.lblRNombre.Text = "Nombre";
            //
            // txtRNombre
            //
            this.txtRNombre.Location = new System.Drawing.Point(120, 62);
            this.txtRNombre.Name = "txtRNombre";
            this.txtRNombre.Size = new System.Drawing.Size(280, 23);
            this.txtRNombre.TabIndex = 3;
            //
            // lblRClasif
            //
            this.lblRClasif.AutoSize = true;
            this.lblRClasif.Location = new System.Drawing.Point(15, 97);
            this.lblRClasif.Name = "lblRClasif";
            this.lblRClasif.Size = new System.Drawing.Size(78, 15);
            this.lblRClasif.TabIndex = 4;
            this.lblRClasif.Text = "Clasificación";
            //
            // cboRClasificacion
            //
            this.cboRClasificacion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboRClasificacion.Location = new System.Drawing.Point(120, 94);
            this.cboRClasificacion.Name = "cboRClasificacion";
            this.cboRClasificacion.Size = new System.Drawing.Size(200, 23);
            this.cboRClasificacion.TabIndex = 5;
            //
            // lblRPorciones
            //
            this.lblRPorciones.AutoSize = true;
            this.lblRPorciones.Location = new System.Drawing.Point(15, 129);
            this.lblRPorciones.Name = "lblRPorciones";
            this.lblRPorciones.Size = new System.Drawing.Size(91, 15);
            this.lblRPorciones.TabIndex = 6;
            this.lblRPorciones.Text = "Porciones base";
            //
            // txtRPorciones
            //
            this.txtRPorciones.Location = new System.Drawing.Point(120, 126);
            this.txtRPorciones.Name = "txtRPorciones";
            this.txtRPorciones.Size = new System.Drawing.Size(80, 23);
            this.txtRPorciones.TabIndex = 7;
            //
            // btnNuevo
            //
            this.btnNuevo.Location = new System.Drawing.Point(15, 175);
            this.btnNuevo.Name = "btnNuevo";
            this.btnNuevo.Size = new System.Drawing.Size(100, 30);
            this.btnNuevo.TabIndex = 8;
            this.btnNuevo.Text = "Nueva";
            this.btnNuevo.UseVisualStyleBackColor = true;
            this.btnNuevo.Click += new System.EventHandler(this.btnNuevo_Click);
            //
            // btnGuardar
            //
            this.btnGuardar.Location = new System.Drawing.Point(125, 175);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(100, 30);
            this.btnGuardar.TabIndex = 9;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            //
            // btnEliminar
            //
            this.btnEliminar.Location = new System.Drawing.Point(235, 175);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 30);
            this.btnEliminar.TabIndex = 10;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            //
            // gbIngredientes
            //
            this.gbIngredientes.Controls.Add(this.dgvIngRec);
            this.gbIngredientes.Controls.Add(this.lblIng);
            this.gbIngredientes.Controls.Add(this.cboIngrediente);
            this.gbIngredientes.Controls.Add(this.lblNeta);
            this.gbIngredientes.Controls.Add(this.txtNeta);
            this.gbIngredientes.Controls.Add(this.lblRend);
            this.gbIngredientes.Controls.Add(this.txtRend);
            this.gbIngredientes.Controls.Add(this.lblUniIng);
            this.gbIngredientes.Controls.Add(this.btnAgregarIng);
            this.gbIngredientes.Controls.Add(this.btnQuitarIng);
            this.gbIngredientes.Location = new System.Drawing.Point(20, 295);
            this.gbIngredientes.Name = "gbIngredientes";
            this.gbIngredientes.Size = new System.Drawing.Size(430, 255);
            this.gbIngredientes.TabIndex = 3;
            this.gbIngredientes.TabStop = false;
            this.gbIngredientes.Text = "Ingredientes de la receta";
            //
            // dgvIngRec
            //
            this.dgvIngRec.AllowUserToAddRows = false;
            this.dgvIngRec.AllowUserToDeleteRows = false;
            this.dgvIngRec.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvIngRec.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colIRNombre,
                this.colIRNeta,
                this.colIRRend,
                this.colIRBruta,
                this.colIRUnidad});
            this.dgvIngRec.Location = new System.Drawing.Point(8, 22);
            this.dgvIngRec.MultiSelect = false;
            this.dgvIngRec.Name = "dgvIngRec";
            this.dgvIngRec.ReadOnly = true;
            this.dgvIngRec.RowHeadersVisible = false;
            this.dgvIngRec.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvIngRec.Size = new System.Drawing.Size(414, 103);
            this.dgvIngRec.TabIndex = 0;
            //
            // colIRNombre
            //
            this.colIRNombre.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIRNombre.DataPropertyName = "NombreIngrediente";
            this.colIRNombre.HeaderText = "Ingrediente";
            this.colIRNombre.Name = "colIRNombre";
            this.colIRNombre.ReadOnly = true;
            //
            // colIRNeta
            //
            this.colIRNeta.DataPropertyName = "CantNeta";
            this.colIRNeta.HeaderText = "Neta";
            this.colIRNeta.Name = "colIRNeta";
            this.colIRNeta.ReadOnly = true;
            this.colIRNeta.Width = 60;
            //
            // colIRRend
            //
            this.colIRRend.DataPropertyName = "Rendimiento";
            this.colIRRend.HeaderText = "Rend.%";
            this.colIRRend.Name = "colIRRend";
            this.colIRRend.ReadOnly = true;
            this.colIRRend.Width = 60;
            //
            // colIRBruta
            //
            this.colIRBruta.DataPropertyName = "CantBruta";
            this.colIRBruta.HeaderText = "Bruta";
            this.colIRBruta.Name = "colIRBruta";
            this.colIRBruta.ReadOnly = true;
            this.colIRBruta.Width = 60;
            //
            // colIRUnidad
            //
            this.colIRUnidad.DataPropertyName = "Abreviatura";
            this.colIRUnidad.HeaderText = "Un.";
            this.colIRUnidad.Name = "colIRUnidad";
            this.colIRUnidad.ReadOnly = true;
            this.colIRUnidad.Width = 45;
            //
            // lblIng
            //
            this.lblIng.AutoSize = true;
            this.lblIng.Location = new System.Drawing.Point(8, 138);
            this.lblIng.Name = "lblIng";
            this.lblIng.Size = new System.Drawing.Size(34, 15);
            this.lblIng.TabIndex = 1;
            this.lblIng.Text = "Ingr.";
            //
            // cboIngrediente
            //
            this.cboIngrediente.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIngrediente.Location = new System.Drawing.Point(55, 135);
            this.cboIngrediente.Name = "cboIngrediente";
            this.cboIngrediente.Size = new System.Drawing.Size(200, 23);
            this.cboIngrediente.TabIndex = 2;
            this.cboIngrediente.SelectedIndexChanged += new System.EventHandler(this.cboIngrediente_SelectedIndexChanged);
            //
            // lblNeta
            //
            this.lblNeta.AutoSize = true;
            this.lblNeta.Location = new System.Drawing.Point(8, 170);
            this.lblNeta.Name = "lblNeta";
            this.lblNeta.Size = new System.Drawing.Size(31, 15);
            this.lblNeta.TabIndex = 3;
            this.lblNeta.Text = "Neta";
            //
            // txtNeta
            //
            this.txtNeta.Location = new System.Drawing.Point(55, 167);
            this.txtNeta.Name = "txtNeta";
            this.txtNeta.Size = new System.Drawing.Size(55, 23);
            this.txtNeta.TabIndex = 4;
            //
            // lblRend
            //
            this.lblRend.AutoSize = true;
            this.lblRend.Location = new System.Drawing.Point(125, 170);
            this.lblRend.Name = "lblRend";
            this.lblRend.Size = new System.Drawing.Size(48, 15);
            this.lblRend.TabIndex = 5;
            this.lblRend.Text = "Rend.%";
            //
            // txtRend
            //
            this.txtRend.Location = new System.Drawing.Point(180, 167);
            this.txtRend.Name = "txtRend";
            this.txtRend.Size = new System.Drawing.Size(45, 23);
            this.txtRend.TabIndex = 6;
            this.txtRend.Text = "100";
            //
            // lblUniIng
            //
            this.lblUniIng.AutoSize = true;
            this.lblUniIng.Location = new System.Drawing.Point(235, 170);
            this.lblUniIng.Name = "lblUniIng";
            this.lblUniIng.Size = new System.Drawing.Size(12, 15);
            this.lblUniIng.TabIndex = 7;
            this.lblUniIng.Text = "-";
            //
            // btnAgregarIng
            //
            this.btnAgregarIng.Location = new System.Drawing.Point(8, 200);
            this.btnAgregarIng.Name = "btnAgregarIng";
            this.btnAgregarIng.Size = new System.Drawing.Size(150, 28);
            this.btnAgregarIng.TabIndex = 8;
            this.btnAgregarIng.Text = "Agregar ingrediente";
            this.btnAgregarIng.UseVisualStyleBackColor = true;
            this.btnAgregarIng.Click += new System.EventHandler(this.btnAgregarIng_Click);
            //
            // btnQuitarIng
            //
            this.btnQuitarIng.Location = new System.Drawing.Point(168, 200);
            this.btnQuitarIng.Name = "btnQuitarIng";
            this.btnQuitarIng.Size = new System.Drawing.Size(100, 28);
            this.btnQuitarIng.TabIndex = 9;
            this.btnQuitarIng.Text = "Quitar";
            this.btnQuitarIng.UseVisualStyleBackColor = true;
            this.btnQuitarIng.Click += new System.EventHandler(this.btnQuitarIng_Click);
            //
            // gbProcedimiento
            //
            this.gbProcedimiento.Controls.Add(this.dgvProc);
            this.gbProcedimiento.Controls.Add(this.lblPaso);
            this.gbProcedimiento.Controls.Add(this.txtPaso);
            this.gbProcedimiento.Controls.Add(this.btnAgregarPaso);
            this.gbProcedimiento.Controls.Add(this.btnQuitarPaso);
            this.gbProcedimiento.Location = new System.Drawing.Point(458, 295);
            this.gbProcedimiento.Name = "gbProcedimiento";
            this.gbProcedimiento.Size = new System.Drawing.Size(422, 255);
            this.gbProcedimiento.TabIndex = 4;
            this.gbProcedimiento.TabStop = false;
            this.gbProcedimiento.Text = "Procedimiento";
            //
            // dgvProc
            //
            this.dgvProc.AllowUserToAddRows = false;
            this.dgvProc.AllowUserToDeleteRows = false;
            this.dgvProc.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProc.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colPNro,
                this.colPDesc});
            this.dgvProc.Location = new System.Drawing.Point(8, 22);
            this.dgvProc.MultiSelect = false;
            this.dgvProc.Name = "dgvProc";
            this.dgvProc.ReadOnly = true;
            this.dgvProc.RowHeadersVisible = false;
            this.dgvProc.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProc.Size = new System.Drawing.Size(406, 130);
            this.dgvProc.TabIndex = 0;
            //
            // colPNro
            //
            this.colPNro.DataPropertyName = "NroPaso";
            this.colPNro.HeaderText = "Paso";
            this.colPNro.Name = "colPNro";
            this.colPNro.ReadOnly = true;
            this.colPNro.Width = 50;
            //
            // colPDesc
            //
            this.colPDesc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPDesc.DataPropertyName = "Descripcion";
            this.colPDesc.HeaderText = "Descripción";
            this.colPDesc.Name = "colPDesc";
            this.colPDesc.ReadOnly = true;
            //
            // lblPaso
            //
            this.lblPaso.AutoSize = true;
            this.lblPaso.Location = new System.Drawing.Point(8, 168);
            this.lblPaso.Name = "lblPaso";
            this.lblPaso.Size = new System.Drawing.Size(33, 15);
            this.lblPaso.TabIndex = 1;
            this.lblPaso.Text = "Paso";
            //
            // txtPaso
            //
            this.txtPaso.Location = new System.Drawing.Point(50, 165);
            this.txtPaso.Name = "txtPaso";
            this.txtPaso.Size = new System.Drawing.Size(364, 23);
            this.txtPaso.TabIndex = 2;
            //
            // btnAgregarPaso
            //
            this.btnAgregarPaso.Location = new System.Drawing.Point(8, 200);
            this.btnAgregarPaso.Name = "btnAgregarPaso";
            this.btnAgregarPaso.Size = new System.Drawing.Size(150, 28);
            this.btnAgregarPaso.TabIndex = 3;
            this.btnAgregarPaso.Text = "Agregar paso";
            this.btnAgregarPaso.UseVisualStyleBackColor = true;
            this.btnAgregarPaso.Click += new System.EventHandler(this.btnAgregarPaso_Click);
            //
            // btnQuitarPaso
            //
            this.btnQuitarPaso.Location = new System.Drawing.Point(168, 200);
            this.btnQuitarPaso.Name = "btnQuitarPaso";
            this.btnQuitarPaso.Size = new System.Drawing.Size(100, 28);
            this.btnQuitarPaso.TabIndex = 4;
            this.btnQuitarPaso.Text = "Quitar";
            this.btnQuitarPaso.UseVisualStyleBackColor = true;
            this.btnQuitarPaso.Click += new System.EventHandler(this.btnQuitarPaso_Click);
            //
            // ucRecetas
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.gbProcedimiento);
            this.Controls.Add(this.gbIngredientes);
            this.Controls.Add(this.gbDatos);
            this.Controls.Add(this.gbRecetas);
            this.Controls.Add(this.lblTitulo);
            this.Name = "ucRecetas";
            this.Size = new System.Drawing.Size(900, 575);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecetas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvIngRec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProc)).EndInit();
            this.gbRecetas.ResumeLayout(false);
            this.gbDatos.ResumeLayout(false);
            this.gbDatos.PerformLayout();
            this.gbIngredientes.ResumeLayout(false);
            this.gbIngredientes.PerformLayout();
            this.gbProcedimiento.ResumeLayout(false);
            this.gbProcedimiento.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.GroupBox gbRecetas;
        private System.Windows.Forms.DataGridView dgvRecetas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRCodigo;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRClasif;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRPorciones;
        private System.Windows.Forms.GroupBox gbDatos;
        private System.Windows.Forms.Label lblRCodigo;
        private System.Windows.Forms.TextBox txtRCodigo;
        private System.Windows.Forms.Label lblRNombre;
        private System.Windows.Forms.TextBox txtRNombre;
        private System.Windows.Forms.Label lblRClasif;
        private System.Windows.Forms.ComboBox cboRClasificacion;
        private System.Windows.Forms.Label lblRPorciones;
        private System.Windows.Forms.TextBox txtRPorciones;
        private System.Windows.Forms.Button btnNuevo;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.GroupBox gbIngredientes;
        private System.Windows.Forms.DataGridView dgvIngRec;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIRNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIRNeta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIRRend;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIRBruta;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIRUnidad;
        private System.Windows.Forms.Label lblIng;
        private System.Windows.Forms.ComboBox cboIngrediente;
        private System.Windows.Forms.Label lblNeta;
        private System.Windows.Forms.TextBox txtNeta;
        private System.Windows.Forms.Label lblRend;
        private System.Windows.Forms.TextBox txtRend;
        private System.Windows.Forms.Label lblUniIng;
        private System.Windows.Forms.Button btnAgregarIng;
        private System.Windows.Forms.Button btnQuitarIng;
        private System.Windows.Forms.GroupBox gbProcedimiento;
        private System.Windows.Forms.DataGridView dgvProc;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPNro;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPDesc;
        private System.Windows.Forms.Label lblPaso;
        private System.Windows.Forms.TextBox txtPaso;
        private System.Windows.Forms.Button btnAgregarPaso;
        private System.Windows.Forms.Button btnQuitarPaso;
    }
}
