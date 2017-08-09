using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ViveroQyC.Scripts;

namespace ViveroQyC.Plantas
{
    public partial class fCategoriaPlanta : Form
    {
        public fCategoriaPlanta()
        {
            InitializeComponent();
        }

        private void fCategoriaPlanta_Load(object sender, EventArgs e)
        {
            DataTable dt = Scripts.HandleBD.SelectAll("CategoriaPlantas");
            // Controlo de que haya alguna fila.
            if (dt.Rows.Count > 0)
            {
                cmbCat.DataSource = dt;
                cmbCat.ValueMember = "CatP_id";
                cmbCat.DisplayMember = "CatP_desc";
                cmbCat.Refresh();
            }
            cmbCat.SelectedIndex = -1;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            DialogResult dr = DialogResult.Yes;
            if (btnAgregar.Text == "Agregar")
            {
                if (string.IsNullOrWhiteSpace(txtNuevaCat.Text))
                    dr = MessageBox.Show("Aviso: El campo no tiene ninguna descripcion!. Reviste antes de seguir", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                dr = MessageBox.Show("Se registrara " + txtNuevaCat.Text + " como una nueva categoria. ¿Desea seguir?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int result = Scripts.HandleBD.Insert("CategoriaPlantas", txtNuevaCat.Text);
                    if (result != 1)
                    {
                        MessageBox.Show(ErroresBD.dErrores[result], "Error");
                    }
                    else
                    {
                        MessageBox.Show("Nueva categoria creada", "Hecho");
                        this.Close();
                        fCategoriaPlanta fNuevaCat = new fCategoriaPlanta();
                        fNuevaCat.MdiParent = this.MdiParent;
                        fNuevaCat.Dock = DockStyle.Fill;
                        fNuevaCat.Show();
                    }
                }
            } else if (btnAgregar.Text == "Editar")
            {
                if (string.IsNullOrWhiteSpace(txtNuevaCat.Text))
                    dr = MessageBox.Show("Aviso: El campo no tiene ninguna descripcion!. Reviste antes de seguir", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                dr = MessageBox.Show("¿Desea seguir?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    int result = HandleBD.Update("CategoriaPlantas", ushort.Parse(cmbCat.SelectedValue.ToString()), txtNuevaCat.Text);
                    if (result != 1)
                    {
                        MessageBox.Show(ErroresBD.dErrores[result], "Error");
                    }
                }
            }
        }

        private void cmbCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCat.SelectedIndex != -1)
            {
                txtNuevaCat.Text = cmbCat.Text;
                btnAgregar.Text = "Editar";
            } else
            {
                txtNuevaCat.Text = "";
                btnAgregar.Text = "Agregar";
            }
        }
    }
}
