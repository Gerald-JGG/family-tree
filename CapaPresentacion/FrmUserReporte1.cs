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
    public partial class FrmUserReporte1 : UserControl
    {
        private string rutaXml = "datos.xml";
        public FrmUserReporte1()
        {
            InitializeComponent();
            MostrarReporteProductosMujeresEnDGV(rutaXml, rutaXml, rutaXml);
        }

        public void MostrarReporteProductosMujeresEnDGV(string rutaVentas, string rutaUsuarios, string rutaProductos)
        {
            List<ObjVentaDetalle> detalles = new BOVentaDetalle().LeerVentaDetalles(rutaVentas);
            List<ObjVenta> ventas = new BOVenta().LeerVentas(rutaVentas);
            List<ObjUsuario> usuarios = new BOUsuario().LeerUsuarios(rutaUsuarios);
            List<ObjProducto> productos = new BOProducto().LeerProductos(rutaProductos);
            Dictionary<int, (string Nombre, int Calorias, int CantidadTotal)> reporte = new Dictionary<int, (string, int, int)>();

            // Filtrar usuarios femeninos
            HashSet<string> mujeres = new HashSet<string>();
            foreach (var usuario in usuarios)
            {
                if (usuario.genero.Trim().ToLower() == "femenino")
                {
                    mujeres.Add(usuario.nombre.Trim().ToLower());
                }
            }

            // Filtrar ventas realizadas por mujeres
            HashSet<int> ventasMujeres = new HashSet<int>();
            foreach (var venta in ventas)
            {
                if (mujeres.Contains(venta.nombreUsuario))
                {
                    ventasMujeres.Add(venta.codigoVenta);
                }
            }

            // Filtrar productos con calorías entre 450 y 700
            Dictionary<int, (string Nombre, int Calorias)> productosFiltrados = new Dictionary<int, (string, int)>();
            foreach (var producto in productos)
            {
                if (producto.calorias >= 450 && producto.calorias <= 700)
                {
                    productosFiltrados[producto.codigoProducto] = (producto.nombreProducto, producto.calorias);
                }
            }

            // Calcular la cantidad total de cada producto vendido por mujeres
            foreach (var detalle in detalles)
            {
                if (ventasMujeres.Contains(detalle.codigoVenta) && productosFiltrados.ContainsKey(detalle.codigoProducto))
                {
                    var producto = productosFiltrados[detalle.codigoProducto];
                    if (reporte.ContainsKey(detalle.codigoProducto))
                    {
                        reporte[detalle.codigoProducto] = (producto.Nombre, producto.Calorias, reporte[detalle.codigoProducto].CantidadTotal + detalle.cantidad);
                    }
                    else
                    {
                        reporte[detalle.codigoProducto] = (producto.Nombre, producto.Calorias, detalle.cantidad);
                    }
                }
            }
            var listaReporte = reporte.Values.Select(r => new
            {
                NombreProducto = r.Nombre,
                Calorias = r.Calorias,
                CantidadTotal = r.CantidadTotal
            }).ToList();

            dgvUsuarios.DataSource = listaReporte;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            MostrarReporteProductosMujeresEnDGV(rutaXml, rutaXml, rutaXml);
        }
    }
}
