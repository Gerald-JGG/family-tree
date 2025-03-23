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
    public partial class FrmUserAgregarPareja : Form
    {
        private List<ObjPersona> posiblesConyuges;
        private FrmUserAgregarPersona ventanaPrincipal;

        public FrmUserAgregarPareja(List<ObjPersona> conyuges, FrmUserAgregarPersona ventanaPrincipal)
        {
            InitializeComponent();
            this.ventanaPrincipal = ventanaPrincipal;
            posiblesConyuges = conyuges;
            CargarConyuges();
        }

        private void CargarConyuges()
        {
            cmbConyuge.DataSource = posiblesConyuges;
            cmbConyuge.DisplayMember = "Nombre";
            cmbConyuge.ValueMember = "Cedula";
            cmbConyuge.SelectedIndex = -1;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnUnir_Click(object sender, EventArgs e)
        {
            if (cmbConyuge.SelectedItem == null)
            {
                MessageBox.Show("Debe seleccionar una persona.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string cedulaSeleccionada = cmbConyuge.SelectedValue.ToString();
            ObjPersona seleccionado = posiblesConyuges.FirstOrDefault(p => p.Cedula == cedulaSeleccionada);

            if (seleccionado != null)
            {
                if (ventanaPrincipal is FrmUserAgregarPersona frm)
                {
                    frm.SetConyuge(seleccionado);
                }
                MessageBox.Show($"Se ha seleccionado con {seleccionado.Nombre} correctamente.", "Unión Seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se encontró el conyuge con la cédula seleccionada.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            this.Close();
        }

    }
}
