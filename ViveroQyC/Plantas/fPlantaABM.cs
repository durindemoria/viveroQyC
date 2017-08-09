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
using System.Drawing.Drawing2D;

namespace ViveroQyC.Plantas
{
    public partial class fPlantaABM : Form
    {
        private string tabla = "Planta";
        public fPlantaABM()
        {
            InitializeComponent();
        }

      

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void fPlantaABM_Load(object sender, EventArgs e)
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

            // OpenFileDialog
            ofdImg.Filter = "Imagenes |*.jpg;*.jpeg;*.png";
            ofdImg.InitialDirectory = @"C:\";
            ofdImg.Title = "Por favor seleccione una imagen para su producto";
        }

        private void btnOpenImg_Click(object sender, EventArgs e)
        {
            
            if(ofdImg.ShowDialog() == DialogResult.OK)
            {
                txtFoto.Text = ofdImg.FileName;
                picboxPlanta.Image = new System.Drawing.Bitmap(ofdImg.FileName);
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImg = new Bitmap(width, height);

            destImg.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImg))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new System.Drawing.Imaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImg;
        }

        private void GuardarImagen(Image imagen)
        {
            string path = System.IO.Directory.GetCurrentDirectory();
            imagen.Save(path+"..\\Imagenes\\Plantas");
            
        }

       

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Le doy formato al precio
            txtPrecio.Text.Replace('.', ',');

            int resultado = HandleForm.SanearValidar(tabla, txtNombre, cmbCat, txtPrecio, cmbTam, txtFoto);
            if (resultado == 1)
            {
                resultado = HandleBD.InsertSinCheckear("Planta", HandleForm.parsearDatos("Planta", txtNombre, cmbCat, txtPrecio, cmbTam, txtFoto));
                if (resultado == 1)
                {
                    MessageBox.Show("Hecho!", "Hecho");
                    this.Close();
                    fPlantaABM fNuevoPlantaAbm = new fPlantaABM();
                    fNuevoPlantaAbm.Parent = this.Parent;
                    fNuevoPlantaAbm.Dock = DockStyle.Fill;
                    fNuevoPlantaAbm.Show();
                }
                else
                    MessageBox.Show(ErroresForm.dErrores[resultado], "Error");
            }
                
            else
                MessageBox.Show(ErroresForm.dErrores[resultado], "Error");

        }

        // Cambia el texto del button. 
        // Para llamar desde otra clase.
        public void SetButtonText(string text)
        {
            btnAgregar.Text = text;
        }

        public void SetTxtNombre(string text)
        {
            txtNombre.Text = text;
        }

        public void SetTxtPrecio(string text)
        {
            txtPrecio.Text = text;
        }

        public void SetTxtFoto(string text)
        {
            txtFoto.Text = text;
        }

        public void SetCmbCat(string txt)
        {
            ComboBox.ObjectCollection x = cmbCat.Items;
            x
            cmbCat.SelectedValue = txt;
        }
    }
}
