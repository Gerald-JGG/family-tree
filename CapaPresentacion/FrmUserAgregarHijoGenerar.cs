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
    public partial class FrmUserAgregarHijoGenerar : Form
    {
        private BOPersona boPersona;
        private string rutaArchivo = "datos.xml";
        public FrmUserAgregarHijoGenerar()
        {
            InitializeComponent();
            boPersona = new BOPersona();
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //private List<ObjPersona> CargarConyuges()
        //{
        //    string generoSeleccionado = cmbGenero.SelectedItem.ToString();
        //    string generoContrario = (generoSeleccionado == "Masculino") ? "Femenino" : "Masculino";

        //    List<ObjPersona> personas = boPersona.LeerPersonas(rutaArchivo);
        //    List<ObjPersona> posiblesConyuges = personas
        //        .Where(p => p.Genero == generoContrario
        //                 && p.Conyuge == 0
        //                 && p.FechaNacimiento != null
        //                 && (DateTime.Now.Year - p.FechaNacimiento.Year) >= 18
        //                 && (DateTime.Now.Year - p.FechaNacimiento.Year) <= 45)
        //        .ToList();
        //    return posiblesConyuges;
        //}

        //private void btnUnionPareja_Click(object sender, EventArgs e)
        //{
        //    if (!VerificarCampos())
        //    {
        //        return;
        //    }
        //    DateTime fechaSeleccionada = dtpFecha.Value;
        //    int edadSeleccionada = DateTime.Now.Year - fechaSeleccionada.Year;

        //    // Verificar que la persona esté dentro del rango de edad (18 a 45 años)
        //    if (edadSeleccionada < 18 || edadSeleccionada > 45)
        //    {
        //        MessageBox.Show("La fecha seleccionada no está dentro del rango de edad permitido (18 a 45 años).", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    List<ObjPersona> conyugesDisponibles = CargarConyuges();

        //    if (conyugesDisponibles.Any())
        //    {
        //        FrmUserAgregarPareja frmPareja = new FrmUserAgregarPareja(conyugesDisponibles, this);
        //        frmPareja.Show();
        //    }
        //    else
        //    {
        //        MessageBox.Show("No hay cónyuges disponibles que cumplan con los requisitos.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        //private void btnAñadir_Click(object sender, EventArgs e)
        //{
        //    if (string.IsNullOrEmpty(txtPadres.Text))
        //    {
        //        MessageBox.Show("Debe seleccionar una pareja.");
        //        return;
        //    }
        //    var selectedPair = (KeyValuePair<string, Tuple<string, string>>)txtPadres.Text();
        //    int padreId = int.Parse(selectedPair.Value.Item1);
        //    int madreId = int.Parse(selectedPair.Value.Item2);

        //    int maxCedula = ObtenerMaximaCedula();
        //    string cedulaHijo = (maxCedula + 1).ToString();
        //    string nombreHijo = $"Hijo de {selectedPair.Value.Item1} y {selectedPair.Value.Item2}";
        //    Random rand = new Random();
        //    string generoHijo = rand.Next(2) == 0 ? "Masculino" : "Femenino";
        //    DateTime fechaNacimientoHijo = DateTime.Now;
        //    int edadHijo = (int)((DateTime.Now - fechaNacimientoHijo).TotalDays / 365.25);
        //    string lugarResidenciaHijo = rand.Next(2) == 0 ? "Residencia del Padre" : "Residencia de la Madre";
        //    string relacionFamiliarHijo = "Hijo";
        //    string fotoHijo = generoHijo == "Masculino" ? "Imagenes/hombre.png" : "Imagenes/mujer.png";

        //    ObjPersona hijo = new ObjPersona
        //    {
        //        Cedula = cedulaHijo,
        //        Familia = 0,
        //        Nombre = nombreHijo,
        //        Genero = generoHijo,
        //        FechaNacimiento = fechaNacimientoHijo,
        //        Edad = edadHijo,
        //        LugarResidencia = lugarResidenciaHijo,
        //        EstadoCivil = "Soltero",
        //        Conyuge = 0,
        //        Fallecido = false,
        //        RelacionFamiliar = relacionFamiliarHijo,
        //        Foto = fotoHijo,
        //        Padre = padreId,
        //        Madre = madreId,
        //    };
        //    boPersona.CrearPersona(hijo, rutaArchivo);

        //    MessageBox.Show("Hijo añadido exitosamente.");
        //    this.Close();
        //}

        private int ObtenerMaximaCedula()
        {
            List<ObjPersona> personas = boPersona.LeerPersonas(rutaArchivo);
            int maxCedula = personas.Max(p => int.Parse(p.Cedula));
            return maxCedula;
        }
    }
}
