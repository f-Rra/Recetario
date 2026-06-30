namespace Presentacion
{
    partial class frmPrincipal
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
            this.menuPrincipal = new System.Windows.Forms.MenuStrip();
            this.menuInicio = new System.Windows.Forms.ToolStripMenuItem();
            this.menuRecetas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuIngredientes = new System.Windows.Forms.ToolStripMenuItem();
            this.menuProveedores = new System.Windows.Forms.ToolStripMenuItem();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.pnlContenido = new System.Windows.Forms.Panel();
            this.menuPrincipal.SuspendLayout();
            this.SuspendLayout();
            //
            // menuPrincipal
            //
            this.menuPrincipal.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.menuInicio,
                this.menuRecetas,
                this.menuIngredientes,
                this.menuProveedores});
            this.menuPrincipal.Location = new System.Drawing.Point(0, 0);
            this.menuPrincipal.Name = "menuPrincipal";
            this.menuPrincipal.Size = new System.Drawing.Size(900, 24);
            this.menuPrincipal.TabIndex = 2;
            //
            // menuInicio
            //
            this.menuInicio.Name = "menuInicio";
            this.menuInicio.Size = new System.Drawing.Size(54, 20);
            this.menuInicio.Text = "Inicio";
            this.menuInicio.Click += new System.EventHandler(this.menuInicio_Click);
            //
            // menuRecetas
            //
            this.menuRecetas.Name = "menuRecetas";
            this.menuRecetas.Size = new System.Drawing.Size(62, 20);
            this.menuRecetas.Text = "Recetas";
            this.menuRecetas.Click += new System.EventHandler(this.menuRecetas_Click);
            //
            // menuIngredientes
            //
            this.menuIngredientes.Name = "menuIngredientes";
            this.menuIngredientes.Size = new System.Drawing.Size(85, 20);
            this.menuIngredientes.Text = "Ingredientes";
            this.menuIngredientes.Click += new System.EventHandler(this.menuIngredientes_Click);
            //
            // menuProveedores
            //
            this.menuProveedores.Name = "menuProveedores";
            this.menuProveedores.Size = new System.Drawing.Size(85, 20);
            this.menuProveedores.Text = "Proveedores";
            this.menuProveedores.Click += new System.EventHandler(this.menuProveedores_Click);
            //
            // lblBienvenida
            //
            this.lblBienvenida.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.Location = new System.Drawing.Point(0, 24);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblBienvenida.Size = new System.Drawing.Size(900, 44);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.Text = "Bienvenido/a";
            this.lblBienvenida.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // pnlContenido
            //
            this.pnlContenido.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pnlContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlContenido.Location = new System.Drawing.Point(0, 68);
            this.pnlContenido.Name = "pnlContenido";
            this.pnlContenido.Size = new System.Drawing.Size(1200, 592);
            this.pnlContenido.TabIndex = 1;
            //
            // frmPrincipal
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 660);
            this.Controls.Add(this.pnlContenido);
            this.Controls.Add(this.lblBienvenida);
            this.Controls.Add(this.menuPrincipal);
            this.MainMenuStrip = this.menuPrincipal;
            this.Name = "frmPrincipal";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Recetario";
            this.menuPrincipal.ResumeLayout(false);
            this.menuPrincipal.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.MenuStrip menuPrincipal;
        private System.Windows.Forms.ToolStripMenuItem menuInicio;
        private System.Windows.Forms.ToolStripMenuItem menuRecetas;
        private System.Windows.Forms.ToolStripMenuItem menuIngredientes;
        private System.Windows.Forms.ToolStripMenuItem menuProveedores;
        private System.Windows.Forms.Label lblBienvenida;
        private System.Windows.Forms.Panel pnlContenido;
    }
}
