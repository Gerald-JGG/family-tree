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
    public partial class FrmUserReporte2 : UserControl
    {
        private string rutaXml = "datos.xml";
        public FrmUserReporte2()
        {
            InitializeComponent();
            MostrarTop3ProductosMasVendidosEnDGV(rutaXml, rutaXml, rutaXml);
        }

        public void MostrarTop3ProductosMasVendidosEnDGV(string rutaVentas, string rutaUsuarios, string rutaProductos)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            MostrarTop3ProductosMasVendidosEnDGV(rutaXml, rutaXml, rutaXml);
        }
    }
}
