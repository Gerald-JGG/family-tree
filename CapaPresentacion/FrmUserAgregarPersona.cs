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
    public partial class FrmUserAgregarPersona : Form
    {
        private BOPersona boPersona;
        private string rutaArchivo = "datos.xml";
        private ObjFamilia familia;
        public FrmUserAgregarPersona(ObjFamilia familia)
        {
            InitializeComponent();
            boPersona = new BOPersona();
            this.familia = familia;
            CargarPersonas(familia.Id);
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            if (!VerificarCampos())
            {
                return;
            }

            string rutaImagen = null;
            if (pictureFoto.Image != null)
            {
                rutaImagen = GuardarImagen(pictureFoto.ImageLocation);
            }
            else
            {
                string generoSeleccionado = cmbGenero.SelectedItem?.ToString();
                string carpetaImagenes = Path.Combine(Application.StartupPath, "Imagenes");

                if (generoSeleccionado == "Masculino")
                {
                    rutaImagen = Path.Combine(carpetaImagenes, "hombre.png");
                }
                else
                {
                    rutaImagen = Path.Combine(carpetaImagenes, "mujer.png");
                }
            }

            ObjPersona persona = new ObjPersona
            {
                Cedula = txtCedula.Text,
                Familia = familia.Id,
                Nombre = txtNombre.Text,
                Genero = cmbGenero.SelectedItem.ToString(),
                FechaNacimiento = dtpFecha.Value,
                LugarResidencia = cmbResidencia.Text,
                EstadoCivil = cmbEstadoCivil.SelectedItem.ToString(),
                Conyuge = Convert.ToInt32(txtConyuge),
                Fallecido = chkFallecido.Checked,
                RelacionFamiliar = cmbRelacionFamiliar.Text,
                Foto = rutaImagen,
                Padre = Convert.ToInt32(cmbPadre1.SelectedValue),
                Madre = Convert.ToInt32(cmbPadre2.SelectedValue)
            };

            boPersona.CrearPersona(persona, rutaArchivo);

            if (txtConyuge.Tag != null)
            {
                ObjPersona conyuge = (ObjPersona)txtConyuge.Tag;
                if (conyuge != null)
                {
                    if (persona.Fallecido || conyuge.Fallecido)
                    {
                        persona.EstadoCivil = "Viudo";
                        conyuge.EstadoCivil = "Viudo";
                    }
                    else
                    {
                        persona.EstadoCivil = "Casado";
                        conyuge.EstadoCivil = "Casado";
                    }

                    boPersona.ModificarPersona(conyuge, rutaArchivo);
                }
            }

            MessageBox.Show("Persona añadida exitosamente.");
            LimpiarCampos();
        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            if (!VerificarCampos())
            {
                return;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string cedula = txtCedula.Text;

            try
            {
                boPersona.EliminarPersona(cedula, rutaArchivo);
                MessageBox.Show("Persona eliminada exitosamente.");
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar persona: {ex.Message}");
            }
        }


        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void cmbEstadoCivil_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbEstadoCivil.SelectedItem.ToString() == "Casado")
            {
                btnUnionPareja.Enabled = true;
            }
            else
            {
                btnUnionPareja.Enabled = false;
            }
        }

        private void LimpiarCampos()
        {
            txtCedula.Clear();
            txtNombre.Clear();
            cmbGenero.SelectedIndex = -1;
            dtpFecha.Value = DateTime.Now;
            cmbResidencia.SelectedIndex = -1;
            cmbEstadoCivil.SelectedIndex = -1;
            chkFallecido.Checked = false;
            cmbRelacionFamiliar.SelectedIndex = -1;
            pictureFoto.Image = null;
            cmbPadre1.SelectedIndex = -1;
            cmbPadre2.SelectedIndex = -1;
        }

        // Verificar campos requeridos
        private bool VerificarCampos()
        {
            if (string.IsNullOrEmpty(txtCedula.Text))
            {
                MessageBox.Show("La cédula es obligatoria.");
                return false;
            }

            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("El nombre es obligatorio.");
                return false;
            }

            if (cmbGenero.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar un género.");
                return false;
            }

            if (cmbEstadoCivil.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar el estado civil.");
                return false;
            }

            if (cmbResidencia.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar el lugar de residencia.");
                return false;
            }
            return true;
        }

        private string GuardarImagen(string rutaOrigen)
        {
            string directorioDestino = Path.Combine(Application.StartupPath, "Imagenes");
            string nombreArchivo = Guid.NewGuid().ToString() + Path.GetExtension(rutaOrigen);
            string rutaDestino = Path.Combine(directorioDestino, nombreArchivo);

            File.Copy(rutaOrigen, rutaDestino);

            return rutaDestino;
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

        private List<ObjPersona> CargarConyuges()
        {
            string generoSeleccionado = cmbGenero.SelectedItem.ToString();
            string generoContrario = (generoSeleccionado == "Masculino") ? "Femenino" : "Masculino";

            List<ObjPersona> personas = boPersona.LeerPersonas(rutaArchivo);
            List<ObjPersona> posiblesConyuges = personas
                .Where(p => p.Familia != familia.Id
                         && p.Genero == generoContrario
                         && p.RelacionFamiliar == cmbRelacionFamiliar.SelectedItem.ToString()
                         && p.Conyuge == 0
                         && p.FechaNacimiento != null
                         && (DateTime.Now.Year - p.FechaNacimiento.Year) >= 18 
                         && (DateTime.Now.Year - p.FechaNacimiento.Year) <= 45)
                .ToList();
            return posiblesConyuges;
        }


        private void btnUnionPareja_Click(object sender, EventArgs e)
        {
            if (!VerificarCampos())
            {
                return;
            }
            DateTime fechaSeleccionada = dtpFecha.Value;
            int edadSeleccionada = DateTime.Now.Year - fechaSeleccionada.Year;

            // Verificar que la persona esté dentro del rango de edad (18 a 45 años)
            if (edadSeleccionada < 18 || edadSeleccionada > 45)
            {
                MessageBox.Show("La fecha seleccionada no está dentro del rango de edad permitido (18 a 45 años).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            List<ObjPersona> conyugesDisponibles = CargarConyuges();

            if (conyugesDisponibles.Any())
            {
                FrmUserAgregarPareja frmPareja = new FrmUserAgregarPareja(conyugesDisponibles);
                frmPareja.Show();
            }
            else
            {
                MessageBox.Show("No hay cónyuges disponibles que cumplan con los requisitos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        public void SetConyuge(ObjPersona conyuge)
        {
            if (conyuge != null)
            {
                txtConyuge.Text = conyuge.Nombre;
                txtConyuge.Tag = conyuge;
            }
        }

        private void cmbPersonas_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPersonas.SelectedIndex != -1)
            {
                string cedulaSeleccionada = ((KeyValuePair<string, string>)cmbPersonas.SelectedItem).Key;
                ObjPersona personaSeleccionada = boPersona.BuscarPersonaPorCedula(cedulaSeleccionada, rutaArchivo);

                if (personaSeleccionada != null)
                {
                    txtCedula.Text = personaSeleccionada.Cedula;
                    txtNombre.Text = personaSeleccionada.Nombre;
                    cmbGenero.SelectedItem = personaSeleccionada.Genero;
                    dtpFecha.Value = personaSeleccionada.FechaNacimiento;
                    cmbResidencia.SelectedItem = personaSeleccionada.LugarResidencia;
                    cmbEstadoCivil.SelectedItem = personaSeleccionada.EstadoCivil;
                    txtConyuge.Text = personaSeleccionada.Conyuge.ToString();
                    chkFallecido.Checked = personaSeleccionada.Fallecido;
                    cmbRelacionFamiliar.SelectedItem = personaSeleccionada.RelacionFamiliar;
                    pictureFoto.ImageLocation = personaSeleccionada.Foto;
                    cmbPadre1.SelectedValue = personaSeleccionada.Padre;
                    cmbPadre2.SelectedValue = personaSeleccionada.Madre;
                }
            }
        }

        private void CargarPersonas(int idFamilia)
        {
            cmbPersonas.Items.Clear();
            List<ObjPersona> personasFamilia = boPersona.LeerPersonas(rutaArchivo)
                .Where(p => p.Familia == idFamilia).ToList();

            foreach (var persona in personasFamilia)
            {
                cmbPersonas.Items.Add(new KeyValuePair<string, string>(persona.Cedula, persona.Nombre));
            }
        }

    }
}
