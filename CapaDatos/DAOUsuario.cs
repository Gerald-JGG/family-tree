using System;
using System.Collections.Generic;
using System.Xml;
using CapaObjetos;

namespace CapaDatos
{
    public class DAOUsuario
    {
        private XmlDocument doc;

        public void CrearUsuario(ObjUsuario usuario, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode usuarioNode = doc.CreateElement("usuario");

            XmlElement nombre = doc.CreateElement("nombre");
            nombre.InnerText = usuario.nombre;
            usuarioNode.AppendChild(nombre);

            XmlElement contraseña = doc.CreateElement("contraseña");
            contraseña.InnerText = usuario.contraseña.ToString();
            usuarioNode.AppendChild(contraseña);

            XmlElement genero = doc.CreateElement("genero");
            genero.InnerText = usuario.genero;
            usuarioNode.AppendChild(genero);

            XmlElement activo = doc.CreateElement("activo");
            activo.InnerText = usuario.activo.ToString();
            usuarioNode.AppendChild(activo);

            XmlElement rol = doc.CreateElement("rol");
            rol.InnerText = usuario.rol;
            usuarioNode.AppendChild(rol);

            XmlElement fechaRegistro = doc.CreateElement("fechaRegistro");
            fechaRegistro.InnerText = usuario.fechaRegistro.ToString("yyyy-MM-dd HH:mm:ss");
            usuarioNode.AppendChild(fechaRegistro);

            XmlNode rootUsuarios = doc.SelectSingleNode("/datos/usuarios");
            rootUsuarios.AppendChild(usuarioNode);

            new DAOXml().guardarDatosXML(doc, ruta);
        }

        public List<ObjUsuario> LeerUsuario(string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            List<ObjUsuario> usuarios = new List<ObjUsuario>();
            XmlNodeList nodeList = doc.SelectNodes("/datos/usuarios/usuario");

            foreach (XmlNode node in nodeList)
            {
                ObjUsuario usuario = new ObjUsuario()
                {
                    nombre = node["nombre"].InnerText,
                    contraseña = node["contraseña"].InnerText,
                    genero = node["genero"].InnerText,
                    activo = Convert.ToBoolean(node["activo"].InnerText),
                    rol = node["rol"].InnerText,
                    fechaRegistro = Convert.ToDateTime(node["fechaRegistro"].InnerText)
                };
                usuarios.Add(usuario);
            }
            return usuarios;
        }

        public void ModificarUsuario(ObjUsuario usuario, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode usuarioNode = doc.SelectSingleNode($"/datos/usuarios/usuario[nombre='{usuario.nombre}']");

            if (usuarioNode != null)
            {
                usuarioNode["nombre"].InnerText = usuario.nombre;
                usuarioNode["genero"].InnerText = usuario.genero;
                usuarioNode["activo"].InnerText = usuario.activo.ToString();

                new DAOXml().guardarDatosXML(doc, ruta);
            }
            else
            {
                throw new Exception("Usuario no encontrado.");
            }
        }

        public void EliminarUsuario(string nombre, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode usuarioNode = doc.SelectSingleNode($"/datos/usuarios/usuario[nombre='{nombre}']");

            if (usuarioNode != null)
            {
                usuarioNode.ParentNode.RemoveChild(usuarioNode);
                new DAOXml().guardarDatosXML(doc, ruta);
            }
            else
            {
                throw new Exception("Usuario no encontrado.");
            }
        }

        // EXTRA
        public ObjUsuario BuscarUsuarioPorNombre(string nombre, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode usuarioNode = doc.SelectSingleNode($"/datos/usuarios/usuario[nombre='{nombre}']");

            if (usuarioNode != null)
            {
                return new ObjUsuario()
                {
                    contraseña = usuarioNode["cedula"].InnerText,
                    nombre = usuarioNode["nombre"].InnerText,
                    genero = usuarioNode["genero"].InnerText,
                    activo = Convert.ToBoolean(usuarioNode["activo"].InnerText)
                };
            }
            else
            {
                return null;
            }
        }
    }
}
