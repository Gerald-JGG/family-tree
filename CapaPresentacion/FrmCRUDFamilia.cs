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
    public partial class FrmCRUDFamilia : UserControl
    {
        private string rutaXml = "datos.xml";

        public FrmCRUDFamilia()
        {
            InitializeComponent();
            CrearXML();
            CargarListaProductos();
            LimpiarCampos();
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

        private void LimpiarCampos()
        {

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

        }
    }
}
