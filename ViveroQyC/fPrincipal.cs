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


namespace ViveroQyC
{
    public partial class fPrincipal : Form
    {
       
        public fPrincipal()
        {
         //   HandleBD.Cantidad_tablas
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void agregarCategoriaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CerrarHijos();
            Plantas.fCategoriaPlanta fCatP = new Plantas.fCategoriaPlanta();
            fCatP.MdiParent = this;
            fCatP.Dock = DockStyle.Fill;
            fCatP.Show();
        }

        // Cerrar formularios hijos
        private void CerrarHijos()
        {
            if (ActiveMdiChild != null)
                ActiveMdiChild.Close();
        }

        private void fPrincipal_Load(object sender, EventArgs e)
        {
            // Cargar el controlador de la base de datos.
            HandleBD.CargarCantidadTablas();
            HandleBD.CargarNombreTablas();
        }

        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CerrarHijos();
            Plantas.fPlantaABM fPlantaAbm = new Plantas.fPlantaABM();
            fPlantaAbm.MdiParent = this;
            fPlantaAbm.Dock = DockStyle.Fill;
            fPlantaAbm.Show();
        }

        private void cambiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CerrarHijos();
            Plantas.fPlantaABM fPlantaABM = new Plantas.fPlantaABM();
            fPlantaABM.SetButtonText("Editar");
            fPlantaABM.MdiParent = this;
            fPlantaABM.Dock = DockStyle.Fill;
            fPlantaABM.Show();
            Plantas.fListaPlantas fListaPlantas = new Plantas.fListaPlantas();
            fListaPlantas.Show();
            fListaPlantas.BringToFront();
            
        }
    }
}
