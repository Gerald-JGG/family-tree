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
            List<ObjVenta> ventas = new BOVenta().LeerVentas(rutaVentas);
            List<ObjUsuario> usuarios = new BOUsuario().LeerUsuarios(rutaUsuarios);

            // Crear un conjunto de cédulas de los usuarios que han realizado compras
            HashSet<string> cedulasUsuarios = new HashSet<string>();
            foreach (var venta in ventas)
            {
                var usuario = usuarios.FirstOrDefault(u => u.nombre.ToLower() == venta.nombreUsuario.ToLower());
                if (usuario != null)
                {
                    cedulasUsuarios.Add(usuario.nombre);
                }
            }
            comboBoxCedulas.Items.Clear();
            comboBoxCedulas.Items.AddRange(cedulasUsuarios.ToArray());
        }

        private void comboBoxCedulas_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string cedulaSeleccionada = comboBoxCedulas.SelectedItem.ToString();
            MostrarComprasPorCedula(cedulaSeleccionada, rutaXml, rutaXml, rutaXml);
        }

        public void MostrarComprasPorCedula(string cedula, string rutaVentas, string rutaUsuarios, string rutaProductos)
        {
            List<ObjVentaDetalle> detalles = new BOVentaDetalle().LeerVentaDetalles(rutaVentas);
            List<ObjVenta> ventas = new BOVenta().LeerVentas(rutaVentas);
            List<ObjUsuario> usuarios = new BOUsuario().LeerUsuarios(rutaUsuarios);
            List<ObjProducto> productos = new BOProducto().LeerProductos(rutaProductos);

            var ventasUsuario = ventas.Where(v => usuarios.FirstOrDefault(u => u.nombre == cedula)?.nombre == v.nombreUsuario).ToList();

            // Listar las compras con los productos asociados
            var listaCompras = from venta in ventasUsuario
                               join detalle in detalles on venta.codigoVenta equals detalle.codigoVenta
                               join producto in productos on detalle.codigoProducto equals producto.codigoProducto
                               select new
                               {
                                   venta.codigoVenta,
                                   producto.nombreProducto,
                                   detalle.cantidad,
                                   producto.calorias,
                                   venta.fecha
                               };
            dgvUsuarios.DataSource = listaCompras.ToList();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CargarCedulasEnComboBox(rutaXml, rutaXml, rutaXml);
            MostrarComprasPorCedula(rutaXml, rutaXml, rutaXml, rutaXml);
        }
    }
}
