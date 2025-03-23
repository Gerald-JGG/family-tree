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
            CargarParejas(familia.Id);
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
                if (generoSeleccionado == "Masculino")
                {
                    rutaImagen = "Imagenes/hombre.png";  // Ruta relativa
                }
                else
                {
                    rutaImagen = "Imagenes/mujer.png";  // Ruta relativa
                }
            }
            int? padreId = null;
            int? madreId = null;

            if (cmbPadres.SelectedItem != null)
            {
                var selectedItem = (KeyValuePair<string, Tuple<string, string>>)cmbPadres.SelectedItem;
                padreId = int.Parse(selectedItem.Value.Item1);
                madreId = int.Parse(selectedItem.Value.Item2);
            }
            ObjPersona persona = new ObjPersona
            {
                Cedula = txtCedula.Text.Trim(),
                Familia = familia.Id,
                Nombre = txtNombre.Text.Trim(),
                Genero = cmbGenero.SelectedItem.ToString(),
                FechaNacimiento = dtpFecha.Value,
                LugarResidencia = cmbResidencia.Text.Trim(),
                EstadoCivil = cmbEstadoCivil.SelectedItem.ToString(),
                Fallecido = chkFallecido.Checked,
                RelacionFamiliar = cmbRelacionFamiliar.Text.Trim(),
                Foto = rutaImagen,
                Padre = padreId.HasValue ? padreId.Value : 0,
                Madre = madreId.HasValue ? madreId.Value : 0,
                Conyuge = 0
            };
            boPersona.CrearPersona(persona, rutaArchivo);

            if (txtConyuge.Tag != null)
            {
                ObjPersona conyuge = (ObjPersona)txtConyuge.Tag;
                if (conyuge != null)
                {
                    persona.Conyuge = int.Parse(conyuge.Cedula);
                    conyuge.Conyuge = int.Parse(persona.Cedula);
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
                    boPersona.ModificarPersona(persona, rutaArchivo);
                    boPersona.ModificarPersona(conyuge, rutaArchivo);
                }
            }
            else
            {
                if (cmbEstadoCivil.SelectedItem.ToString() == "Casado")
                {
                    MessageBox.Show("Debe seleccionar un cónyuge.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
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
            string cedulaPersona = txtCedula.Text.Trim();
            ObjPersona persona = boPersona.BuscarPersonaPorCedula(cedulaPersona, rutaArchivo);

            if (persona == null)
            {
                MessageBox.Show("No se encontró a la persona con la cédula proporcionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                if (generoSeleccionado == "Masculino")
                {
                    rutaImagen = "Imagenes/hombre.png";  // Ruta relativa
                }
                else
                {
                    rutaImagen = "Imagenes/mujer.png";  // Ruta relativa
                }
            }
            int? padreId = null;
            int? madreId = null;

            if (cmbPadres.SelectedItem != null)
            {
                var selectedItem = (KeyValuePair<string, Tuple<string, string>>)cmbPadres.SelectedItem;
                padreId = int.Parse(selectedItem.Value.Item1);
                madreId = int.Parse(selectedItem.Value.Item2);
            }
            persona.Nombre = txtNombre.Text.Trim();
            persona.Genero = cmbGenero.SelectedItem.ToString();
            persona.FechaNacimiento = dtpFecha.Value;
            persona.LugarResidencia = cmbResidencia.Text.Trim();
            persona.EstadoCivil = cmbEstadoCivil.SelectedItem.ToString();
            persona.Fallecido = chkFallecido.Checked;
            persona.RelacionFamiliar = cmbRelacionFamiliar.Text.Trim();
            persona.Foto = rutaImagen;
            persona.Padre = padreId.HasValue ? padreId.Value : 0;
            persona.Madre = madreId.HasValue ? madreId.Value : 0; 
            boPersona.ModificarPersona(persona, rutaArchivo);

            if (txtConyuge.Tag != null)
            {
                ObjPersona conyuge = (ObjPersona)txtConyuge.Tag;
                if (conyuge != null)
                {
                    persona.Conyuge = int.Parse(conyuge.Cedula);
                    conyuge.Conyuge = int.Parse(persona.Cedula);
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
                    boPersona.ModificarPersona(persona, rutaArchivo);
                    boPersona.ModificarPersona(conyuge, rutaArchivo);
                }
            }
            else
            {
                if (cmbEstadoCivil.SelectedItem.ToString() == "Casado")
                {
                    MessageBox.Show("Debe seleccionar un cónyuge.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            MessageBox.Show("Persona modificada exitosamente.");
            LimpiarCampos();
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
            if (cmbEstadoCivil.SelectedItem != null && cmbEstadoCivil.SelectedItem.ToString() == "Casado")
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
            cmbPersonas.SelectedIndex = -1;
            chkFallecido.Checked = false;
            cmbRelacionFamiliar.SelectedIndex = -1;
            pictureFoto.Image = null;
            cmbPadres.SelectedIndex = -1;
            CargarPersonas(familia.Id);
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

        private List<ObjPersona> CargarConyuges()
        {
            string generoSeleccionado = cmbGenero.SelectedItem.ToString();
            string generoContrario = (generoSeleccionado == "Masculino") ? "Femenino" : "Masculino";

            List<ObjPersona> personas = boPersona.LeerPersonas(rutaArchivo);
            List<ObjPersona> posiblesConyuges = personas
                .Where(p => p.Genero == generoContrario
                         && p.Conyuge == 0
                         && p.FechaNacimiento != null)
                .ToList();
            return posiblesConyuges;
        }


        private void btnUnionPareja_Click(object sender, EventArgs e)
        {
            if (!VerificarCampos())
            {
                return;
            }
            List<ObjPersona> conyugesDisponibles = CargarConyuges();
            if (conyugesDisponibles.Any())
            {
                FrmUserAgregarPareja frmPareja = new FrmUserAgregarPareja(conyugesDisponibles, this);
                frmPareja.Show();
            }
            else
            {
                MessageBox.Show("No hay cónyuges disponibles.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        public void SetConyuge(ObjPersona conyuge)
        {
            if (conyuge != null)
            {
                txtConyuge.Text = conyuge.Nombre;
                txtConyuge.Tag = conyuge;
                lblProducto.Text = conyuge.Nombre;
                lblProducto.Tag = conyuge;

                if (cmbRelacionFamiliar.SelectedItem?.ToString() == "Hermanos")
                {
                    conyuge.RelacionFamiliar = "Cuñados";
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado un cónyuge.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    string rutaFoto = personaSeleccionada.Foto;
                    if (!string.IsNullOrEmpty(rutaFoto) && File.Exists(rutaFoto))
                    {
                        pictureFoto.ImageLocation = rutaFoto;
                        pictureFoto.SizeMode = PictureBoxSizeMode.Zoom;
                    }

                    cmbPadres.SelectedValue = personaSeleccionada.Padre;
                    cmbPadres.SelectedValue = personaSeleccionada.Madre;
                }
            }
        }

        private void CargarParejas(int idFamilia)
        {
            cmbPadres.Items.Clear();
            List<ObjPersona> personasFamiliaConConyuge = boPersona.LeerPersonas(rutaArchivo)
                .Where(p => p.Conyuge != 0).ToList();

            foreach (var persona in personasFamiliaConConyuge)
            {
                ObjPersona conyuge = boPersona.BuscarPersonaPorCedula(persona.Conyuge.ToString(), rutaArchivo);
                if (conyuge != null && conyuge.Conyuge == int.Parse(persona.Cedula))
                {
                    string parejaNombre = $"{persona.Nombre} & {conyuge.Nombre}";
                    cmbPadres.Items.Add(new KeyValuePair<string, Tuple<string, string>>(
                        parejaNombre, new Tuple<string, string>(persona.Cedula, conyuge.Cedula)
                    ));
                }
            }
            cmbPadres.DisplayMember = "Key";
            cmbPadres.ValueMember = "Value";
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
            cmbPersonas.DisplayMember = "Value";
            cmbPersonas.ValueMember = "Key";
        }


        private void button3_Click(object sender, EventArgs e)
        {
            CargarPersonas(familia.Id);
            CargarParejas(familia.Id);
        }

        private void cmbPadres_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPadres.SelectedItem != null)
            {
                // Deshabilitar el cmbRelacion
                cmbRelacionFamiliar.Enabled = false;

                // Obtener la relación seleccionada
                var selectedPair = (KeyValuePair<string, Tuple<string, string>>)cmbPadres.SelectedItem;
                string padreId = selectedPair.Value.Item1;
                string madreId = selectedPair.Value.Item2;
                string relacion = cmbRelacionFamiliar.Text;

                switch (relacion)
                {
                    case "Abuelos":
                        cmbRelacionFamiliar.Items.Clear();
                        cmbRelacionFamiliar.Items.Add("Tios");
                        cmbRelacionFamiliar.Items.Add("Padres");
                        break;

                    case "Padres":
                        cmbRelacionFamiliar.Items.Clear();
                        cmbRelacionFamiliar.Items.Add("Hermanos");
                        break;

                    case "Hermanos":
                        cmbRelacionFamiliar.Items.Clear();
                        cmbRelacionFamiliar.Items.Add("Sobrinos");
                        break;

                    case "Ego":
                        cmbRelacionFamiliar.Items.Clear();
                        cmbRelacionFamiliar.Items.Add("Hijos");
                        break;

                    case "Hijos":
                        cmbRelacionFamiliar.Items.Clear();
                        cmbRelacionFamiliar.Items.Add("Nietos");
                        break;

                    case "Nietos":
                        cmbRelacionFamiliar.Items.Clear();
                        cmbRelacionFamiliar.Items.Add("Bisnietos");
                        break;

                    default:
                        cmbRelacionFamiliar.Items.Clear();
                        break;
                }
            }
        }

    }
}
