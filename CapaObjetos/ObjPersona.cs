using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaObjetos
{
    public class ObjPersona
    {
        public string Cedula { get; set; }
        public int Familia { get; set; }
        public string Nombre { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get; set; } 
        public string LugarResidencia { get; set; } 
        public string EstadoCivil { get; set; }
        public int Conyuge { get; set; }
        public bool Fallecido { get; set; }  
        public string RelacionFamiliar { get; set; }
        public string Foto { get; set; }
        public int Padre { get; set; }
        public int Madre { get; set; }

        public ObjPersona() { }
    }
}
