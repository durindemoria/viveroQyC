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
    public partial class fListaPlantas : Form
    {
        public fListaPlantas()
        {
            InitializeComponent();
        }

        private void fListaPlantas_Load(object sender, EventArgs e)
        {
            dgvPlantas.DataSource = HandleBD.SelectAll("Planta");
            dgvPlantas.Refresh();
        }

        private void dgvPlantas_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            fPlantaABM x = null;
            foreach(Form f in Application.OpenForms)
            {

                if (f.GetType().Equals(typeof(fPlantaABM)))
                {
                    x = f as fPlantaABM;
                    break;
                }
            }

            x.SetTxtNombre(dgvPlantas.Rows[e.RowIndex].Cells[0].Value.ToString());
            x.SetCmbCat(dgvPlantas.Rows[e.RowIndex].Cells[1].Value.ToString());
        }
    }
}
