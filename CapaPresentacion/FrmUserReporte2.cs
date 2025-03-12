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
            List<ObjVentaDetalle> detalles = new BOVentaDetalle().LeerVentaDetalles(rutaVentas);
            List<ObjVenta> ventas = new BOVenta().LeerVentas(rutaVentas);
            List<ObjProducto> productos = new BOProducto().LeerProductos(rutaProductos);
            Dictionary<int, (string Nombre, int CantidadTotal)> reporte = new Dictionary<int, (string, int)>();

            // Calcular la cantidad total de ventas de cada producto
            foreach (var detalle in detalles)
            {
                if (productos.Any(p => p.codigoProducto == detalle.codigoProducto))
                {
                    var producto = productos.First(p => p.codigoProducto == detalle.codigoProducto);

                    if (reporte.ContainsKey(detalle.codigoProducto))
                    {
                        reporte[detalle.codigoProducto] = (producto.nombreProducto, reporte[detalle.codigoProducto].CantidadTotal + detalle.cantidad);
                    }
                    else
                    {
                        reporte[detalle.codigoProducto] = (producto.nombreProducto, detalle.cantidad);
                    }
                }
            }
            var top3Productos = reporte.Values.OrderByDescending(r => r.CantidadTotal).Take(3).ToList();

            var listaReporte = top3Productos.Select(r => new
            {
                NombreProducto = r.Nombre,
                CantidadTotal = r.CantidadTotal
            }).ToList();

            // Asignar el reporte al DataGridView
            dgvUsuarios.DataSource = listaReporte;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MostrarTop3ProductosMasVendidosEnDGV(rutaXml, rutaXml, rutaXml);
        }
    }
}
