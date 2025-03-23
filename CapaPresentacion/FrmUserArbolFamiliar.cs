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
    public partial class FrmUserArbolFamiliar : UserControl
    {
        private ObjFamilia familiaSeleccionada;
        private BOFamilia boFamilia;
        private string rutaArchivo = "datos.xml";
        public FrmUserArbolFamiliar()
        {
            InitializeComponent();
            boFamilia = new BOFamilia();
            CargarFamilias();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmUserAgregarFamilia frmLogin = new FrmUserAgregarFamilia();
            frmLogin.Show();
        }

        private void CargarFamilias()
        {
            try
            {
                List<ObjFamilia> familias = boFamilia.LeerFamilias(rutaArchivo);
                comboBox1.Items.Clear();
                foreach (var familia in familias)
                {
                    comboBox1.Items.Add(new KeyValuePair<int, string>(familia.Id, familia.Nombre));
                }
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las familias: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is KeyValuePair<int, string> selectedFamilia)
            {
                ObjFamilia familiaSeleccionada = boFamilia.LeerFamiliaPorId(selectedFamilia.Key, rutaArchivo);
                FrmUserAgregarPersona frmAgregarPersona = new FrmUserAgregarPersona(familiaSeleccionada);
                frmAgregarPersona.Show();
            }
            else
            {
                MessageBox.Show("Seleccione una familia antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHacerUnion_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is KeyValuePair<int, string> selectedFamilia)
            {
                ObjFamilia familiaSeleccionada = boFamilia.LeerFamiliaPorId(selectedFamilia.Key, rutaArchivo);
                FrmUserAgregarPareja frmAgregarPersona = new FrmUserAgregarPareja(familiaSeleccionada);
                frmAgregarPersona.Show();
            }
            else
            {
                MessageBox.Show("Seleccione una familia antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAñadirHijo_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is KeyValuePair<int, string> selectedFamilia)
            {
                ObjFamilia familiaSeleccionada = boFamilia.LeerFamiliaPorId(selectedFamilia.Key, rutaArchivo);
                FrmUserAgregarHijoGenerar frmAgregarPersona = new FrmUserAgregarHijo(familiaSeleccionada);
                frmAgregarPersona.Show();
            }
            else
            {
                MessageBox.Show("Seleccione una familia antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
