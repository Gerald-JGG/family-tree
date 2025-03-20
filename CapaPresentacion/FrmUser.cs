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
        private FrmUserArbolFamiliar uc1 = new FrmUserArbolFamiliar();

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
