using System;
using System.Collections.Generic;
using CapaDatos;
using CapaObjetos;

namespace CapaNegocio
{
    public class BOFamilia
    {
        private DAOFamilia daoFamilia;

        public BOFamilia()
        {
            daoFamilia = new DAOFamilia();
        }

        // Crear una nueva familia
        public void CrearFamilia(ObjFamilia familia, string ruta)
        {
            if (daoFamilia.LeerFamiliaPorId(familia.Id, ruta) != null)
            {
                throw new Exception("La familia con ese nombre ya existe.");
            }
            daoFamilia.CrearFamilia(familia, ruta);
        }

        public List<ObjFamilia> LeerFamilias(string ruta)
        {
            return daoFamilia.LeerFamilias(ruta);
        }

        // Modificar una familia existente
        public void ModificarFamilia(ObjFamilia familia, string ruta)
        {
            ObjFamilia familiaExistente = daoFamilia.LeerFamiliaPorId(familia.Id, ruta);
            if (familiaExistente == null)
            {
                throw new Exception("La familia no existe.");
            }
            daoFamilia.ModificarFamilia(familia, ruta);
        }

        // Eliminar una familia por nombre
        public void EliminarFamilia(int id, string ruta)
        {
            ObjFamilia familiaExistente = daoFamilia.LeerFamiliaPorId(id, ruta);
            if (familiaExistente == null)
            {
                throw new Exception("La familia no existe.");
            }
            daoFamilia.EliminarFamilia(id, ruta);
        }
    }
}
