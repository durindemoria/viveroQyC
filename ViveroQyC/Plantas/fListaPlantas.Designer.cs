namespace ViveroQyC.Plantas
{
    partial class fListaPlantas
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvPlantas = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlantas)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvPlantas
            // 
            this.dgvPlantas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPlantas.Location = new System.Drawing.Point(12, 12);
            this.dgvPlantas.Name = "dgvPlantas";
            this.dgvPlantas.Size = new System.Drawing.Size(713, 618);
            this.dgvPlantas.TabIndex = 0;
            this.dgvPlantas.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPlantas_CellContentDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvPlantas);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(746, 642);
            this.panel1.TabIndex = 1;
            // 
            // fListaPlantas
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(746, 642);
            this.Controls.Add(this.panel1);
            this.Name = "fListaPlantas";
            this.Text = "Lista de plantas";
            this.Load += new System.EventHandler(this.fListaPlantas_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlantas)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPlantas;
        private System.Windows.Forms.Panel panel1;
    }
}