using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CapaNegocio;
using CapaObjetos;

namespace CapaPresentacion
{
    public partial class FrmUserAgregarFamilia : Form
    {
        private BOFamilia boFamilia;
        private string rutaArchivo = "datos.xml";
        public FrmUserAgregarFamilia()
        {
            InitializeComponent();
            boFamilia = new BOFamilia();
            CargarFamilias();
        }

        private void CargarFamilias()
        {
            cboFamilias.Items.Clear();
            List<ObjFamilia> familias = boFamilia.LeerFamilias(rutaArchivo);

            foreach (var familia in familias)
            {
                cboFamilias.Items.Add(familia);
            }

            cboFamilias.DisplayMember = "Nombre";
            cboFamilias.ValueMember = "Id";
        }

        private void btnAñadir_Click_1(object sender, EventArgs e)
        {
            string nombreFamilia = txtNombre.Text.Trim();

            if (string.IsNullOrEmpty(nombreFamilia))
            {
                MessageBox.Show("Ingrese un nombre para la familia.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Asegurar que se ingresen 8 bisabuelos
            int contadorBisabuelos = 0;
            while (contadorBisabuelos < 8)
            {
                FrmUserAgregarBisabuelo formBisabuelos = new FrmUserAgregarBisabuelo();
                if (formBisabuelos.ShowDialog() == DialogResult.OK)
                {
                    contadorBisabuelos++;
                }
                else
                {
                    MessageBox.Show("Debe agregar los bisabuelos antes de continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Asegurar que se ingresen 4 abuelos
            int contadorAbuelos = 0;
            while (contadorAbuelos < 4)
            {
                FrmUserAgregarAbuelo formAbuelos = new FrmUserAgregarAbuelo();
                if (formAbuelos.ShowDialog() == DialogResult.OK)
                {
                    contadorAbuelos++;
                }
                else
                {
                    MessageBox.Show("Debe agregar los abuelos antes de continuar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            List<ObjFamilia> familias = boFamilia.LeerFamilias(rutaArchivo);
            int nuevoId = 1;
            if (familias.Count > 0)
            {
                nuevoId = familias.Max(f => f.Id) + 1;
            }

            ObjFamilia nuevaFamilia = new ObjFamilia(nuevoId, nombreFamilia);
            boFamilia.CrearFamilia(nuevaFamilia, rutaArchivo);

            MessageBox.Show("Familia añadida correctamente.");
            CargarFamilias();
            LimpiarCampos();
        }

        private void btnModificar_Click_1(object sender, EventArgs e)
        {
            if (cboFamilias.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una familia para modificar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string nuevoNombre = txtNombre.Text.Trim();
            if (string.IsNullOrEmpty(nuevoNombre))
            {
                MessageBox.Show("Ingrese el nuevo nombre.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ObjFamilia familiaSeleccionada = (ObjFamilia)cboFamilias.SelectedItem;
                familiaSeleccionada.Nombre = nuevoNombre;
                boFamilia.ModificarFamilia(familiaSeleccionada, rutaArchivo);
                MessageBox.Show("Familia modificada correctamente.");
                CargarFamilias();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            if (cboFamilias.SelectedItem == null)
            {
                MessageBox.Show("Seleccione una familia para eliminar.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                ObjFamilia familiaSeleccionada = (ObjFamilia)cboFamilias.SelectedItem;
                boFamilia.EliminarFamilia(familiaSeleccionada.Id, rutaArchivo);
                MessageBox.Show("Familia eliminada correctamente.");
                CargarFamilias();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLimpiar_Click_1(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void LimpiarCampos()
        {
            txtNombre.Clear();
            cboFamilias.SelectedIndex = -1;
        }

        private void cboFamilias_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cboFamilias.SelectedItem != null)
            {
                ObjFamilia familiaSeleccionada = (ObjFamilia)cboFamilias.SelectedItem;
                txtNombre.Text = familiaSeleccionada.Nombre;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CargarFamilias();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
