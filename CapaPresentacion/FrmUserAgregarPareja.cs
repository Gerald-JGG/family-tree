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

        public FrmUserAgregarPareja(List<ObjPersona> conyuges)
        {
            InitializeComponent();
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
            ObjPersona seleccionado = (ObjPersona)cmbConyuge.SelectedItem;
            if (ventanaPrincipal is FrmUserAgregarPersona frm)
            {
                frm.SetConyuge(seleccionado);
            }
            MessageBox.Show($"Se ha unido con {seleccionado.Nombre} correctamente.", "Unión Confirmada", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }

    }
}
