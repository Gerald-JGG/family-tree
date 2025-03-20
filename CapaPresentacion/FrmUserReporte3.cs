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
    public partial class FrmUserReporte3 : UserControl
    {
        private string rutaXml = "datos.xml";
        public FrmUserReporte3()
        {
            InitializeComponent();
            CargarCedulasEnComboBox(rutaXml, rutaXml, rutaXml);
            MostrarComprasPorCedula(rutaXml, rutaXml, rutaXml, rutaXml);
        }

        public void CargarCedulasEnComboBox(string rutaVentas, string rutaUsuarios, string rutaProductos)
        {

        }

        private void comboBoxCedulas_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string cedulaSeleccionada = comboBoxCedulas.SelectedItem.ToString();
            MostrarComprasPorCedula(cedulaSeleccionada, rutaXml, rutaXml, rutaXml);
        }

        public void MostrarComprasPorCedula(string cedula, string rutaVentas, string rutaUsuarios, string rutaProductos)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CargarCedulasEnComboBox(rutaXml, rutaXml, rutaXml);
            MostrarComprasPorCedula(rutaXml, rutaXml, rutaXml, rutaXml);
        }
    }
}
