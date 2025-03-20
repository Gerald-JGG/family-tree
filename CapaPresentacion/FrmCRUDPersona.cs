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
using System.Xml;
using CapaNegocio;
using CapaObjetos;

namespace CapaPresentacion
{
    public partial class FrmCRUDPersona : UserControl
    {
        private string rutaXml = "datos.xml";

        public FrmCRUDPersona()
        {
            InitializeComponent();
            CrearXML();
            CargarListaProductos();
            LimpiarCampos();
            CargarCategorias();
        }

        public void CrearXML()
        {
            BOXml boXml = new BOXml();
            XmlDocument doc = new XmlDocument();
            if (!File.Exists(rutaXml))
            {
                boXml.CrearXML(rutaXml, "datos");
            }
            doc.Load(rutaXml);

            XmlNode root = doc.SelectSingleNode("/datos");
            if (root == null)
            {
                root = doc.CreateElement("datos");
                doc.AppendChild(root);
            }

            XmlNode usuariosNode = root.SelectSingleNode("productos");
            if (usuariosNode == null)
            {
                usuariosNode = doc.CreateElement("productos");
                root.AppendChild(usuariosNode);
                boXml.guardarDatosXML(doc, rutaXml);
            }
            boXml.guardarDatosXML(doc, rutaXml);
        }

        private void CargarCategorias()
        {
            
        }

        private void LimpiarCampos()
        {
            numId.Value = 0;
            txtNombre.Text = "";
            txtPrecio.Text = "";
            txtDescripcion.Text = "";
            txtCalorias.Text = "";
            numCantidad.Value = 0;
            cmbCategoria.Text = "";
            numId.Enabled = true;
        }

        private void CargarListaProductos()
        {
            
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {

        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {

        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {

        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            CargarListaProductos();
            LimpiarCampos();
        }

        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvUsuarios.Rows[e.RowIndex];

                numId.Value = Convert.ToInt32(row.Cells["codigoProducto"].Value);
                txtNombre.Text = row.Cells["nombreProducto"].Value.ToString();
                txtPrecio.Text = row.Cells["precio"].Value.ToString();
                txtDescripcion.Text = row.Cells["descripcion"].Value.ToString();
                txtCalorias.Text = row.Cells["calorias"].Value.ToString();
                numCantidad.Value = Convert.ToInt32(row.Cells["cantidad"].Value);
                cmbCategoria.Text = row.Cells["categoria"].Value.ToString();

                numId.Enabled = false;
            }
        }
    }
}
