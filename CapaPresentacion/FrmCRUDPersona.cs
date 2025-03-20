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
        private BOProducto boProducto;
        private string rutaXml = "datos.xml";

        public FrmCRUDPersona()
        {
            InitializeComponent();
            boProducto = new BOProducto();
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
            List<ObjCategoria> categorias = new BOCategoria().LeerCategorias(rutaXml);
            var categoriasUnique = new HashSet<string>();
            foreach (var categoria in categorias)
            {
                categoriasUnique.Add(categoria.nombreCategoria);
            }

            cmbCategoria.Items.Clear();
            foreach (var categoria in categoriasUnique)
            {
                cmbCategoria.Items.Add(categoria);
            }
        }

        private void LimpiarCampos()
        {
            List<ObjProducto> productos = boProducto.LeerProductos(rutaXml);
            if (productos.Count > 0)
            {
                int ultimoId = productos.Max(c => c.codigoProducto);
                numId.Value = ultimoId + 1;
            }
            else
            {
                numId.Value = 1;
            }
            txtNombre.Clear();
            txtPrecio.Clear();
            txtDescripcion.Clear();
            txtCalorias.Clear();
            numCantidad.Value = 0;
        }

        private void CargarListaProductos()
        {
            List<ObjProducto> productos = boProducto.LeerProductos(rutaXml);
            dgvUsuarios.DataSource = productos;
        }

        private void btnAñadir_Click(object sender, EventArgs e)
        {
            try
            {
                ObjProducto producto = new ObjProducto()
                {
                    codigoProducto = (int)numId.Value,
                    nombreProducto = txtNombre.Text,
                    precio = Convert.ToInt32(txtPrecio.Text),
                    descripcion = txtDescripcion.Text,
                    calorias = Convert.ToInt32(txtCalorias.Text),
                    cantidad = (int)numCantidad.Value,
                    categoria = cmbCategoria.Text
                };

                boProducto.CrearProducto(producto, rutaXml);
                MessageBox.Show("Producto creado exitosamente.");
                LimpiarCampos();
                CargarListaProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                ObjProducto producto = new ObjProducto()
                {
                    codigoProducto = (int)numId.Value,
                    nombreProducto = txtNombre.Text,
                    precio = Convert.ToInt32(txtPrecio.Text),
                    descripcion = txtDescripcion.Text,
                    calorias = Convert.ToInt32(txtCalorias.Text),
                    cantidad = (int)numCantidad.Value,
                    categoria = cmbCategoria.Text
                };

                boProducto.ModificarProducto(producto, rutaXml);
                MessageBox.Show("Producto modificado exitosamente.");
                LimpiarCampos();
                CargarListaProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEliminar_Click_1(object sender, EventArgs e)
        {
            try
            {
                int codigoProducto = (int)numId.Value;
                boProducto.EliminarProducto(codigoProducto, rutaXml);
                MessageBox.Show("Producto eliminado exitosamente.");
                LimpiarCampos();
                CargarListaProductos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
