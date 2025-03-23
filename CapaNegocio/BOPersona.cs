using System;
using System.Collections.Generic;
using CapaDatos;
using CapaObjetos;

namespace CapaNegocio
{
    public class BOPersona
    {
        private DAOPersona daoPersona;

        public BOPersona()
        {
            daoPersona = new DAOPersona();
        }

        // Crear una nueva persona
        public void CrearPersona(ObjPersona persona, string ruta)
        {
            // Verificar si ya existe una persona con esa cédula
            if (daoPersona.BuscarPersonaPorCedula(persona.Cedula, ruta) != null)
            {
                throw new Exception("La persona con esa cédula ya existe.");
            }
            daoPersona.CrearPersona(persona, ruta);
        }

        // Leer todas las personas
        public List<ObjPersona> LeerPersonas(string ruta)
        {
            return daoPersona.LeerPersonas(ruta);
        }

        // Modificar una persona existente
        public void ModificarPersona(ObjPersona persona, string ruta)
        {
            ObjPersona personaExistente = daoPersona.BuscarPersonaPorCedula(persona.Cedula, ruta);
            if (personaExistente == null)
            {
                throw new Exception("La persona no existe.");
            }
            daoPersona.ModificarPersona(persona, ruta);
        }

        // Eliminar una persona por cédula
        public void EliminarPersona(string cedula, string ruta)
        {
            ObjPersona personaExistente = daoPersona.BuscarPersonaPorCedula(cedula, ruta);
            if (personaExistente == null)
            {
                throw new Exception("La persona no existe.");
            }
            daoPersona.EliminarPersona(cedula, ruta);
        }

        public ObjPersona BuscarPersonaPorCedula(string cedula, string ruta)
        {
            ObjPersona persona = daoPersona.BuscarPersonaPorCedula(cedula, ruta);
            return persona;
        }
    }
}
