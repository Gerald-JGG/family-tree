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
    public partial class FrmUser : Form
    {
        private string rutaArchivo = "datos.xml";

        public FrmUser()
        {
            InitializeComponent();
            CrearXML();
            MostrarControl(uc1);
        }
        private FrmUserArbolFamiliar uc1 = new FrmUserArbolFamiliar();

        public void CrearXML()
        {
            BOXml boXml = new BOXml();
            XmlDocument doc = new XmlDocument();

            if (!File.Exists(rutaArchivo))
            {
                boXml.CrearXML(rutaArchivo, "datos");
            }

            doc.Load(rutaArchivo);

            XmlNode root = doc.SelectSingleNode("/datos");
            if (root == null)
            {
                root = doc.CreateElement("datos");
                doc.AppendChild(root);
            }

            XmlNode familiasNode = root.SelectSingleNode("familias");
            if (familiasNode == null)
            {
                familiasNode = doc.CreateElement("familias");
                root.AppendChild(familiasNode);
            }

            XmlNode personasNode = root.SelectSingleNode("personas");
            if (personasNode == null)
            {
                personasNode = doc.CreateElement("personas");
                root.AppendChild(personasNode);
            }
            boXml.guardarDatosXML(doc, rutaArchivo);
        }


        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnArbolFamiliar_Click(object sender, EventArgs e)
        {
            MostrarControl(uc1);
        }

        private void MostrarControl(UserControl control)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(control);
            control.BringToFront();
        }
    }
}
