using System;
using System.Collections.Generic;
using System.Xml;
using CapaObjetos;

namespace CapaDatos
{
    public class DAOFamilia
    {
        private XmlDocument doc;

        // Crear una nueva familia y agregarla al XML
        public void CrearFamilia(ObjFamilia familia, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode familiaNode = doc.CreateElement("familia");

            XmlElement id = doc.CreateElement("id");
            id.InnerText = familia.Id.ToString();
            familiaNode.AppendChild(id);

            XmlElement nombre = doc.CreateElement("nombre");
            nombre.InnerText = familia.Nombre;
            familiaNode.AppendChild(nombre);

            // Crear los miembros de la familia
            foreach (var miembro in familia.Miembros)
            {
                XmlNode miembroNode = doc.CreateElement("miembro");

                XmlElement cedula = doc.CreateElement("cedula");
                cedula.InnerText = miembro.Cedula;
                miembroNode.AppendChild(cedula);

                XmlElement nombreMiembro = doc.CreateElement("nombre");
                nombreMiembro.InnerText = miembro.Nombre;
                miembroNode.AppendChild(nombreMiembro);

                // Añadir más propiedades de ObjPersona si es necesario...

                familiaNode.AppendChild(miembroNode);
            }

            XmlNode rootFamilias = doc.SelectSingleNode("/datos/familias");
            rootFamilias.AppendChild(familiaNode);

            new DAOXml().guardarDatosXML(doc, ruta);
        }

        // Leer las familias desde el XML
        public List<ObjFamilia> LeerFamilias(string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            List<ObjFamilia> familias = new List<ObjFamilia>();
            XmlNodeList nodeList = doc.SelectNodes("/datos/familias/familia");

            foreach (XmlNode node in nodeList)
            {
                ObjFamilia familia = new ObjFamilia(
                    Convert.ToInt32(node["id"].InnerText),
                    node["nombre"].InnerText
                );

                // Leer los miembros de la familia
                XmlNodeList miembrosNodeList = node.SelectNodes("miembro");
                foreach (XmlNode miembroNode in miembrosNodeList)
                {
                    ObjPersona miembro = new ObjPersona()
                    {
                        Cedula = miembroNode["cedula"].InnerText,
                        Nombre = miembroNode["nombre"].InnerText
                        // Agregar más propiedades de ObjPersona si es necesario...
                    };
                    familia.Miembros.Add(miembro);
                }

                familias.Add(familia);
            }
            return familias;
        }

        // Modificar los detalles de una familia
        public void ModificarFamilia(ObjFamilia familia, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode familiaNode = doc.SelectSingleNode($"/datos/familias/familia[id='{familia.Id}']");

            if (familiaNode != null)
            {
                familiaNode["nombre"].InnerText = familia.Nombre;

                // Modificar los miembros de la familia
                foreach (var miembro in familia.Miembros)
                {
                    XmlNode miembroNode = doc.SelectSingleNode($"/datos/familias/familia[id='{familia.Id}']/miembro[cedula='{miembro.Cedula}']");
                    if (miembroNode != null)
                    {
                        miembroNode["nombre"].InnerText = miembro.Nombre;
                    }
                }

                new DAOXml().guardarDatosXML(doc, ruta);
            }
            else
            {
                throw new Exception("Familia no encontrada.");
            }
        }

        // Eliminar una familia del XML
        public void EliminarFamilia(int idFamilia, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode familiaNode = doc.SelectSingleNode($"/datos/familias/familia[id='{idFamilia}']");

            if (familiaNode != null)
            {
                familiaNode.ParentNode.RemoveChild(familiaNode);
                new DAOXml().guardarDatosXML(doc, ruta);
            }
            else
            {
                throw new Exception("Familia no encontrada.");
            }
        }

        // Buscar una familia por su nombre
        public ObjFamilia BuscarFamiliaPorNombre(string nombreFamilia, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode familiaNode = doc.SelectSingleNode($"/datos/familias/familia[nombre='{nombreFamilia}']");

            if (familiaNode != null)
            {
                ObjFamilia familia = new ObjFamilia(
                    Convert.ToInt32(familiaNode["id"].InnerText),
                    familiaNode["nombre"].InnerText
                );

                // Leer los miembros de la familia
                XmlNodeList miembrosNodeList = familiaNode.SelectNodes("miembro");
                foreach (XmlNode miembroNode in miembrosNodeList)
                {
                    ObjPersona miembro = new ObjPersona()
                    {
                        Cedula = miembroNode["cedula"].InnerText,
                        Nombre = miembroNode["nombre"].InnerText
                        // Agregar más propiedades de ObjPersona si es necesario...
                    };
                    familia.Miembros.Add(miembro);
                }

                return familia;
            }
            else
            {
                return null;
            }
        }
    }
}
