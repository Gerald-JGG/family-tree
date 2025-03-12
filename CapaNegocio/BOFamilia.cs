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
            // Verificar si ya existe una familia con el mismo nombre
            if (daoFamilia.LeerFamiliaPorNombre(familia.Nombre, ruta) != null)
            {
                throw new Exception("La familia con ese nombre ya existe.");
            }
            daoFamilia.CrearFamilia(familia, ruta);
        }

        // Leer todas las familias
        public List<ObjFamilia> LeerFamilias(string ruta)
        {
            return daoFamilia.LeerFamilias(ruta);
        }

        // Modificar una familia existente
        public void ModificarFamilia(ObjFamilia familia, string ruta)
        {
            ObjFamilia familiaExistente = daoFamilia.LeerFamiliaPorNombre(familia.Nombre, ruta);
            if (familiaExistente == null)
            {
                throw new Exception("La familia no existe.");
            }
            daoFamilia.ModificarFamilia(familia, ruta);
        }

        // Eliminar una familia por nombre
        public void EliminarFamilia(string nombre, string ruta)
        {
            ObjFamilia familiaExistente = daoFamilia.LeerFamiliaPorNombre(nombre, ruta);
            if (familiaExistente == null)
            {
                throw new Exception("La familia no existe.");
            }
            daoFamilia.EliminarFamilia(nombre, ruta);
        }
    }
}
