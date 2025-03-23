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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace CapaPresentacion
{
    public partial class FrmUserArbolFamiliar : UserControl
    {
        private ObjFamilia familiaSeleccionada;
        private BOFamilia boFamilia;
        private BOPersona boPersona;
        private string rutaArchivo = "datos.xml";
        public FrmUserArbolFamiliar()
        {
            InitializeComponent();
            boFamilia = new BOFamilia();
            boPersona = new BOPersona();
            CargarFamilias();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            FrmUserAgregarFamilia frmLogin = new FrmUserAgregarFamilia();
            frmLogin.Show();
        }

        private void CargarFamilias()
        {
            try
            {
                List<ObjFamilia> familias = boFamilia.LeerFamilias(rutaArchivo);
                comboBox1.Items.Clear();
                foreach (var familia in familias)
                {
                    comboBox1.Items.Add(new KeyValuePair<int, string>(familia.Id, familia.Nombre));
                }
                comboBox1.DisplayMember = "Value";
                comboBox1.ValueMember = "Key";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar las familias: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                int familyId = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;
                CargarPersonasEnTreeView(familyId);
            }
        }

        private void CargarPersonasEnTreeView(int familyId)
        {
            treeViewFamilia.Nodes.Clear();
            List<ObjPersona> personas = boPersona.LeerPersonas(rutaArchivo)
                .Where(p => p.Familia == familyId)
                .ToList();
            Dictionary<int, TreeNode> nodosCreados = new Dictionary<int, TreeNode>();

            foreach (var persona in personas)
            {
                if (int.TryParse(persona.Cedula, out int cedulaInt) && !nodosCreados.ContainsKey(cedulaInt))
                {
                    TreeNode nodoPersona = new TreeNode(persona.Nombre);
                    nodosCreados[cedulaInt] = nodoPersona;

                    if (int.TryParse(persona.Conyuge.ToString(), out int conyugeInt) && conyugeInt != 0)
                    {
                        ObjPersona conyuge = personas.FirstOrDefault(p => int.TryParse(p.Cedula, out int c) && c == conyugeInt);
                        if (conyuge != null && !nodosCreados.ContainsKey(conyugeInt))
                        {
                            nodoPersona.Text += " ♥ " + conyuge.Nombre;
                            nodosCreados[conyugeInt] = nodoPersona;
                        }
                    }

                    // Si tiene padres, agrégalo como hijo
                    if (persona.Padre != 0 && nodosCreados.ContainsKey(persona.Padre))
                    {
                        nodosCreados[persona.Padre].Nodes.Add(nodoPersona);
                    }
                    else if (persona.Madre != 0 && nodosCreados.ContainsKey(persona.Madre))
                    {
                        nodosCreados[persona.Madre].Nodes.Add(nodoPersona);
                    }
                    else
                    {
                        treeViewFamilia.Nodes.Add(nodoPersona);
                    }
                }
            }
            treeViewFamilia.ExpandAll();
        }

        private void btnAgregarHijo_Click(object sender, EventArgs e)
        {
            if (treeViewFamilia.SelectedNode != null)
            {
                List<ObjPersona> personas = boPersona.LeerPersonas(rutaArchivo);
                string nuevaCedula = ObtenerCedulaMaxima(personas);

                string[] nombresPareja = treeViewFamilia.SelectedNode.Text.Split('♥'); // Ver si hay pareja
                if (nombresPareja.Length == 2)
                {
                    ObjPersona persona1 = boPersona.BuscarPersonaPorNombre(nombresPareja[0].Trim(), rutaArchivo);
                    ObjPersona persona2 = boPersona.BuscarPersonaPorNombre(nombresPareja[1].Trim(), rutaArchivo);

                    if (persona1 != null && persona2 != null)
                    {
                        if (persona1.Edad >= 21 && persona1.Edad <= 40 && persona2.Edad >= 21 && persona2.Edad <= 40)
                        {
                            ObjPersona hijo = new ObjPersona
                            {
                                Cedula = nuevaCedula,
                                Nombre = "Nuevo Hijo",
                                Genero = "Desconocido",
                                FechaNacimiento = DateTime.Today,
                                LugarResidencia = persona1.LugarResidencia,
                                EstadoCivil = "Soltero",
                                Conyuge = 0,
                                Fallecido = false,
                                RelacionFamiliar = "Hijo",
                                Padre = int.Parse(persona1.Cedula),
                                Madre = int.Parse(persona2.Cedula),
                                Familia = persona1.Familia
                            };
                            boPersona.CrearPersona(hijo, rutaArchivo);
                            CargarPersonasEnTreeView(persona1.Familia);
                        }
                        else
                        {
                            MessageBox.Show("La pareja no cumple con la edad requerida (21-40 años).", "Restricción", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        public string ObtenerCedulaMaxima(List<ObjPersona> personas)
        {
            int cedulaMaxima = 0;
            foreach (var persona in personas)
            {
                if (int.TryParse(persona.Cedula, out int cedula))
                {
                    if (cedula > cedulaMaxima)
                    {
                        cedulaMaxima = cedula;
                    }
                }
            }
            return (cedulaMaxima + 1).ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is KeyValuePair<int, string> selectedFamilia)
            {
                ObjFamilia familiaSeleccionada = boFamilia.LeerFamiliaPorId(selectedFamilia.Key, rutaArchivo);
                FrmUserAgregarPersona frmAgregarPersona = new FrmUserAgregarPersona(familiaSeleccionada);
                frmAgregarPersona.Show();
            }
            else
            {
                MessageBox.Show("Seleccione una familia antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnHacerUnion_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is KeyValuePair<int, string> selectedFamilia)
            {
                ObjFamilia familiaSeleccionada = boFamilia.LeerFamiliaPorId(selectedFamilia.Key, rutaArchivo);
                FrmUserAgregarPersona frmAgregarPersona = new FrmUserAgregarPersona(familiaSeleccionada);
                frmAgregarPersona.Show();
            }
            else
            {
                MessageBox.Show("Seleccione una familia antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnAñadirHijo_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem is KeyValuePair<int, string> selectedFamilia)
            {
                ObjFamilia familiaSeleccionada = boFamilia.LeerFamiliaPorId(selectedFamilia.Key, rutaArchivo);
                FrmUserAgregarPersona frmAgregarPersona = new FrmUserAgregarPersona(familiaSeleccionada);
                frmAgregarPersona.Show();
            }
            else
            {
                MessageBox.Show("Seleccione una familia antes de continuar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CargarFamilias();
        }
    }
}
