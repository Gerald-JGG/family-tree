using System;
using System.Collections.Generic;
using CapaDatos;
using CapaObjetos;

namespace CapaNegocio
{
    public class BOUsuario
    {
        private DAOUsuario daoUsuario;

        public BOUsuario()
        {
            daoUsuario = new DAOUsuario();
        }

        public void CrearUsuario(ObjUsuario usuario, string ruta)
        {
            if (daoUsuario.BuscarUsuarioPorNombre(usuario.nombre, ruta) != null)
            {
                throw new Exception("El usuario con esa cédula ya existe.");
            }
            daoUsuario.CrearUsuario(usuario, ruta);
        }

        public List<ObjUsuario> LeerUsuarios(string ruta)
        {
            return daoUsuario.LeerUsuario(ruta);
        }

        public void ModificarUsuario(ObjUsuario usuario, string ruta)
        {
            ObjUsuario usuarioExistente = daoUsuario.BuscarUsuarioPorNombre(usuario.nombre, ruta);
            if (usuarioExistente == null)
            {
                throw new Exception("El usuario no existe.");
            }
            daoUsuario.ModificarUsuario(usuario, ruta);
        }

        public void EliminarUsuario(string nombre, string ruta)
        {
            ObjUsuario usuarioExistente = daoUsuario.BuscarUsuarioPorNombre(nombre, ruta);
            if (usuarioExistente == null)
            {
                throw new Exception("El usuario no existe.");
            }
            daoUsuario.EliminarUsuario(nombre, ruta);
        }
    }
}
