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
using static CapaPresentacion.FrmLogin;

namespace CapaPresentacion
{
    public partial class FrmUserMiCarrito : UserControl
    {
        private static string rutaXml = "datos.xml";
        private static List<ObjProducto> productosEnCarrito = new List<ObjProducto>();
        private static Dictionary<int, int> cantidadEnCarrito = new Dictionary<int, int>();
        private static ObjUsuario usuarioLogeado = SesionUsuario.UsuarioActual;
        public FrmUserMiCarrito()
        {
            InitializeComponent();
            CrearXML();
            CargarCarrito();
            usuarioLogeado = SesionUsuario.UsuarioActual;
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

            XmlNode ventasNode = root.SelectSingleNode("ventas");
            if (ventasNode == null)
            {
                ventasNode = doc.CreateElement("ventas");
                root.AppendChild(ventasNode);
                boXml.guardarDatosXML(doc, rutaXml);
            }

            XmlNode ventasDetalleNode = root.SelectSingleNode("ventaDetalles");
            if (ventasDetalleNode == null)
            {
                ventasDetalleNode = doc.CreateElement("ventaDetalles");
                root.AppendChild(ventasDetalleNode);
                boXml.guardarDatosXML(doc, rutaXml);
            }

            boXml.guardarDatosXML(doc, rutaXml);
        }

        public static bool PuedeAgregarAlCarrito(ObjProducto producto, int cantidadAComprar)
        {
            int cantidadActual = cantidadEnCarrito.ContainsKey(producto.codigoProducto) ? cantidadEnCarrito[producto.codigoProducto] : 0;
            return (cantidadActual + cantidadAComprar) <= producto.cantidad;
        }

        public static void AgregarProductoAlCarrito(ObjProducto producto, int cantidadAComprar)
        {
            if (!cantidadEnCarrito.ContainsKey(producto.codigoProducto))
            {
                cantidadEnCarrito[producto.codigoProducto] = 0;
                productosEnCarrito.Add(producto);
            }

            cantidadEnCarrito[producto.codigoProducto] += cantidadAComprar;
            productosEnCarrito.ForEach(p =>
            {
                if (p.codigoProducto == producto.codigoProducto)
                {
                    p.cantidad = cantidadEnCarrito[producto.codigoProducto];
                }
            });
            FrmUserMiCarrito instanciaCarrito = Application.OpenForms.OfType<FrmUserMiCarrito>().FirstOrDefault();
            instanciaCarrito?.CargarCarrito();
        }

        private void CargarCarrito()
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = productosEnCarrito;
            dataGridView2.DataSource = bs;
            dataGridView2.Refresh();
        }

        private void btnComprar_Click_1(object sender, EventArgs e)
        {
            if (productosEnCarrito.Count == 0)
            {
                MessageBox.Show("El carrito está vacío.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var venta = new ObjVenta
            {
                codigoVenta = GenerarCodigoVenta(),
                nombreUsuario = usuarioLogeado.nombre,
                fecha = DateTime.Now,
                total = CalcularTotalCompra()
            };

            new BOVenta().CrearVenta(venta, rutaXml);

            foreach (var producto in productosEnCarrito)
            {
                var ventaDetalle = new ObjVentaDetalle
                {
                    codigoVenta = venta.codigoVenta,
                    codigoProducto = producto.codigoProducto,
                    cantidad = cantidadEnCarrito[producto.codigoProducto]
                };

                new BOVentaDetalle().CrearVentaDetalle(ventaDetalle, rutaXml);

                var productoEnInventario = new BOProducto().LeerProductos(rutaXml).FirstOrDefault(p => p.codigoProducto == producto.codigoProducto);
                if (productoEnInventario != null)
                {
                    productoEnInventario.cantidad -= cantidadEnCarrito[producto.codigoProducto];
                    new BOProducto().ModificarProducto(productoEnInventario, rutaXml);
                }
            }

            productosEnCarrito.Clear();
            cantidadEnCarrito.Clear();
            CargarCarrito();

            MessageBox.Show("Compra confirmada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private int GenerarCodigoVenta()
        {
            List<ObjVenta> ventas = new BOVenta().LeerVentas(rutaXml);
            if (ventas.Count == 0)
            {
                return 1;
            }
            else
            {
                int maxCodigoVenta = ventas.Max(v => v.codigoVenta);
                return maxCodigoVenta + 1;
            }
        }

        private int CalcularTotalCompra()
        {
            int total = 0;
            foreach (var producto in productosEnCarrito)
            {
                var productoEnInventario = new BOProducto().LeerProductos(rutaXml).FirstOrDefault(p => p.codigoProducto == producto.codigoProducto);
                if (productoEnInventario != null)
                {
                    total += productoEnInventario.precio * cantidadEnCarrito[producto.codigoProducto];
                }
            }
            return total;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            CargarCarrito();
        }

        private void FrmUserMiCarrito_Load(object sender, EventArgs e)
        {
            CargarCarrito();
        }
    }
}
