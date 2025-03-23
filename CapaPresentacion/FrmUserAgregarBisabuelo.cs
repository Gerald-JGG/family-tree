using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaObjetos;

namespace CapaPresentacion
{
    public partial class FrmUserAgregarBisabuelo : Form
    {
        private BOPersona boPersona;
        private string rutaArchivo = "datos.xml";

        public FrmUserAgregarBisabuelo()
        {
            InitializeComponent();
            boPersona = new BOPersona();
        }

        private void btnCerrar_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAñadirPareja_Click(object sender, EventArgs e)
        {
            // Validar que todos los campos estén completos
            if (string.IsNullOrWhiteSpace(txtCedula.Text) ||
                string.IsNullOrWhiteSpace(txtNombre.Text) ||
                cmbGenero.SelectedItem == null ||
                string.IsNullOrWhiteSpace(cmbResidencia.Text) ||
                string.IsNullOrWhiteSpace(txtCedula2.Text) ||
                string.IsNullOrWhiteSpace(txtNombre2.Text) ||
                cmbGenero2.SelectedItem == null ||
                string.IsNullOrWhiteSpace(cmbResidencia2.Text))
            {
                MessageBox.Show("Todos los campos son obligatorios, incluyendo las fotos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Guardar imagen de abuelo
            string rutaImagenAbuelo = null;
            if (pictureFoto.Image != null)
            {
                rutaImagenAbuelo = GuardarImagen(pictureFoto.ImageLocation);
            }
            else
            {
                string generoSeleccionado = cmbGenero.SelectedItem?.ToString();
                if (generoSeleccionado == "Masculino")
                {
                    rutaImagenAbuelo = "Imagenes/hombre.png";
                }
                else
                {
                    rutaImagenAbuelo = "Imagenes/mujer.png";
                }
            }

            // Guardar imagen de abuela
            string rutaImagenAbuela = null;
            if (pictureFoto2.Image != null)
            {
                rutaImagenAbuela = GuardarImagen(pictureFoto2.ImageLocation);
            }
            else
            {
                string generoSeleccionado2 = cmbGenero2.SelectedItem?.ToString();
                if (generoSeleccionado2 == "Masculino")
                {
                    rutaImagenAbuela = "Imagenes/hombre.png";  // Ruta relativa
                }
                else
                {
                    rutaImagenAbuela = "Imagenes/mujer.png";  // Ruta relativa
                }
            }

            // Crear objeto abuelo
            ObjPersona abuelo = new ObjPersona
            {
                Cedula = txtCedula.Text.Trim(),
                Nombre = txtNombre.Text.Trim(),
                Genero = cmbGenero.SelectedItem.ToString(),
                FechaNacimiento = dtpFecha.Value,
                LugarResidencia = cmbResidencia.Text.Trim(),
                EstadoCivil = "Casado",
                Conyuge = int.TryParse(txtCedula2.Text.Trim(), out int cedulaConyuge) ? cedulaConyuge : 0,
                Fallecido = chkFallecido.Checked,
                RelacionFamiliar = "Abuelo",
                Foto = rutaImagenAbuelo
            };

            // Crear objeto abuela
            ObjPersona abuela = new ObjPersona
            {
                Cedula = txtCedula2.Text.Trim(),
                Nombre = txtNombre2.Text.Trim(),
                Genero = cmbGenero2.SelectedItem.ToString(),
                FechaNacimiento = dtpFecha2.Value,
                LugarResidencia = cmbResidencia2.Text.Trim(),
                EstadoCivil = "Casado",
                Conyuge = int.TryParse(txtCedula.Text.Trim(), out int cedulaPersona) ? cedulaPersona : 0,
                Fallecido = chkFallecido2.Checked,
                RelacionFamiliar = "Abuela",
                Foto = rutaImagenAbuela
            };

            // Cambiar estado civil si alguno de los abuelos está fallecido
            if (abuelo.Fallecido || abuela.Fallecido)
            {
                abuelo.EstadoCivil = "Viudo";
                abuela.EstadoCivil = "Viuda";
            }

            // Guardar datos de abuelo y abuela en el archivo
            boPersona.CrearPersona(abuelo, rutaArchivo);
            boPersona.CrearPersona(abuela, rutaArchivo);

            MessageBox.Show("Pareja de abuelos añadida correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private string GuardarImagen(string rutaOrigen)
        {
            string directorioDestino = Path.Combine(Application.StartupPath, "Imagenes");
            if (!Directory.Exists(directorioDestino))
            {
                Directory.CreateDirectory(directorioDestino);
            }
            string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(rutaOrigen);
            string rutaDestino = Path.Combine(directorioDestino, nombreArchivo);

            File.Copy(rutaOrigen, rutaDestino);
            return Path.Combine("Imagenes", nombreArchivo);
        }

        private void btnSeleccionarFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureFoto.Image = Image.FromFile(openFileDialog.FileName);
                pictureFoto.SizeMode = PictureBoxSizeMode.Zoom;
                pictureFoto.ImageLocation = openFileDialog.FileName;
            }
        }

        private void btnSeleccionarFoto2_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de imagen (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg;*.jpeg;*.png;*.bmp";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                pictureFoto2.Image = Image.FromFile(openFileDialog.FileName);
                pictureFoto2.SizeMode = PictureBoxSizeMode.Zoom;
                pictureFoto2.ImageLocation = openFileDialog.FileName;
            }
        }
    }
}
