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
    public partial class FrmUserProductoDetalle : Form
    {
        private ObjProducto producto;
        public FrmUserProductoDetalle(ObjProducto productoSeleccionado)
        {
            InitializeComponent();
            producto = productoSeleccionado;
            numCantidad.ReadOnly = true;
            numCantidad.Controls[1].Enabled = false;
            lblProducto.Text = "Comprar " + productoSeleccionado.nombreProducto;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnComprar_Click(object sender, EventArgs e)
        {
            int cantidadAComprar = (int)numCantidad.Value;

            if (cantidadAComprar <= 0)
            {
                MessageBox.Show("Seleccione una cantidad válida.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!FrmUserMiCarrito.PuedeAgregarAlCarrito(producto, cantidadAComprar))
            {
                MessageBox.Show("No puedes agregar más de lo que hay en stock.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ObjProducto productoComprado = new ObjProducto
            {
                codigoProducto = producto.codigoProducto,
                nombreProducto = producto.nombreProducto,
                precio = producto.precio,
                descripcion = producto.descripcion,
                calorias = producto.calorias,
                cantidad = cantidadAComprar,
                categoria = producto.categoria
            };

            FrmUserMiCarrito.AgregarProductoAlCarrito(productoComprado, cantidadAComprar);
            MessageBox.Show("Producto agregado al carrito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.Close();
        }
    }
}
