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
        public string Nombre { get; set; }
        public string Genero { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public int Edad { get; set; } 
        public string LugarResidencia { get; set; } 
        public string EstadoCivil { get; set; } 
        public bool Fallecido { get; set; }  
        public bool Casado { get; set; }
        public string RelacionFamiliar { get; set; }
        public string Foto { get; set; }
        public ObjPersona Conyuge { get; set; } 
        public List<ObjPersona> Hijos { get; set; } 

        public ObjPersona(string cedula, string nombre, string genero, DateTime fechaNacimiento, string lugarResidencia, string estadoCivil, string relacionFamiliar)
        {
            Cedula = cedula;
            Nombre = nombre;
            Genero = genero;
            FechaNacimiento = fechaNacimiento;
            Edad = CalcularEdad();
            LugarResidencia = lugarResidencia;
            EstadoCivil = estadoCivil;
            Fallecido = false;
            Casado = false;
            RelacionFamiliar = relacionFamiliar;
            Foto = genero == "Masculino" ? "avatar_masculino.jpg" : "avatar_femenino.jpg";
            Hijos = new List<ObjPersona>();
        }

        public int CalcularEdad()
        {
            var edad = DateTime.Now.Year - FechaNacimiento.Year;
            if (FechaNacimiento > DateTime.Now.AddYears(-edad)) edad--;
            return edad;
        }
    }
}
