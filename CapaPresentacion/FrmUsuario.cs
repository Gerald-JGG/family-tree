using System.Windows.Forms;
using System;
using System.IO;
using CapaObjetos;
using CapaNegocio;
using System.Collections.Generic;

namespace CapaPresentacion
{
    public partial class FrmUsuario : Form
    {
        public BOUsuario boUsr;
        List<ObjUsuario> lista;
        string ruta = "Datos.xml";

        public FrmUsuario()
        {
            InitializeComponent();
            boUsr = new BOUsuario();
            CrearXML();
        }

        public void CrearXML() {
            if (!File.Exists(ruta))
            {
                boUsr.CrearXML(ruta, "Estudiantes");
            }
            else {
                boUsr.LeerXML(ruta);
            }
        }

        public void LimpiarCampos()
        {
            txtCedula.Clear();
            txtNombre.Clear();
            dtpFechaNac.Value = DateTime.Now;
            txtResidencia.Clear();
            txtCedulaC.Clear();
            txtNombreC.Clear();
            dtpFechaNacC.Value = DateTime.Now;
            txtResidenciaC.Clear();
        }

        private void btnInsertar_Click(object sender, EventArgs e)
        {
            ObjUsuario usuario = new ObjUsuario
            {
                cedula = Convert.ToInt32(txtCedula.Text),
                nombre = txtNombre.Text,
                fechaNac = Convert.ToDateTime(dtpFechaNac.Text),
                residencia = txtResidencia.Text
            };
            boUsr.Registrar(usuario, ruta);
            LimpiarCampos();
        }

        private void tcEstudiantes_SelectedIndexChanged(object sender, EventArgs e)
        {
            lista = boUsr.CargarCombo();
            cmbEstudiante.ValueMember = "cedula";
            cmbEstudiante.DisplayMember = "cedula";
            cmbEstudiante.DataSource = lista;
        }

        private void cmbEstudiante_SelectedIndexChanged(object sender, EventArgs e)
        {
            int cedula = Convert.ToInt32(cmbEstudiante.SelectedValue.ToString());
            List<ObjUsuario> lista = boUsr.CargarDatos(cedula);
            txtCedulaC.Text = Convert.ToString(lista[0].cedula);
            txtNombreC.Text = lista[0].nombre;
            dtpFechaNacC.Text = Convert.ToString(lista[0].fechaNac);
            txtResidenciaC.Text = lista[0].residencia;
        }

        public ObjUsuario obtenerDatosInterfaz()
        {
            ObjUsuario estudiante = new ObjUsuario
            {
                cedula = Convert.ToInt32(txtCedulaC.Text),
                nombre = txtNombreC.Text,
                fechaNac = Convert.ToDateTime(dtpFechaNacC.Text),
                residencia = txtResidenciaC.Text
            };
            return estudiante;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            ObjUsuario estudiante = obtenerDatosInterfaz();
            boUsr.ModificarUsuario(estudiante, ruta);
            tcEstudiantes.SelectedTab = tcEstudiantes.TabPages["tabPage1"];
            LimpiarCampos();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ObjUsuario usuario = obtenerDatosInterfaz();
            boUsr.EliminarUsuario(usuario, ruta);
            tcEstudiantes.SelectedTab = tcEstudiantes.TabPages["tabPage1"];
            LimpiarCampos();
        }
    }
}
