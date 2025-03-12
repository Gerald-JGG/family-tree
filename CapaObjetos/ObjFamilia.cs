using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaObjetos
{
    public class ObjFamilia
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public List<ObjPersona> Miembros { get; set; }

        public ObjFamilia(int id, string nombre)
        {
            Id = id;
            Nombre = nombre;
            Miembros = new List<ObjPersona>();
        }
    }
}