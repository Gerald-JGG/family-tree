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
    public partial class FrmUserProducto : UserControl
    {
        private string rutaXml = "datos.xml";

        public FrmUserProducto()
        {
            InitializeComponent();
        }

        private void FrmUserProducto_Load(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void CargarProductos()
        {
            var listaProductos = new BOProducto().LeerProductos(rutaXml).Where(p => p.cantidad > 0).ToList();
            dataGridView2.DataSource = listaProductos;
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
                ObjProducto producto = new ObjProducto
                {
                    codigoProducto = Convert.ToInt32(row.Cells["codigoProducto"].Value),
                    nombreProducto = row.Cells["nombreProducto"].Value.ToString(),
                    precio = Convert.ToInt32(row.Cells["precio"].Value),
                    descripcion = row.Cells["descripcion"].Value.ToString(),
                    calorias = Convert.ToInt32(row.Cells["calorias"].Value),
                    cantidad = Convert.ToInt32(row.Cells["cantidad"].Value),
                    categoria = row.Cells["categoria"].Value.ToString()
                };
                FrmUserProductoDetalle detalleProducto = new FrmUserProductoDetalle(producto);
                detalleProducto.ShowDialog();
            }
            CargarProductos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }
    }
}
