using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaPresentacion
{
    public partial class FrmUser : Form
    {
        public FrmUser()
        {
            InitializeComponent();
            MostrarControl(uc1);
        }
        private FrmUserProducto uc1 = new FrmUserProducto();
        private FrmUserMiCarrito uc2 = new FrmUserMiCarrito();

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            var resultado = MessageBox.Show("¿Está seguro que quiere cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (resultado == DialogResult.Yes)
            {
                this.Close();
                FrmLogin frmLogin = new FrmLogin();
                frmLogin.Show();
            }
        }

        private void MostrarControl(UserControl control)
        {
            panel2.Controls.Clear();
            panel2.Controls.Add(control);
            control.BringToFront();
        }

        private void btnProductos_Click(object sender, EventArgs e)
        {
            MostrarControl(uc1);
        }

        private void btnMiCarrito_Click(object sender, EventArgs e)
        {
            MostrarControl(uc2);
        }
    }
}
