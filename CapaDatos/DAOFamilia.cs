using System;
using System.Collections.Generic;
using System.Xml;
using CapaObjetos;

namespace CapaDatos
{
    public class DAOFamilia
    {
        private XmlDocument doc;

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

            XmlNode rootFamilias = doc.SelectSingleNode("/datos/familias");
            rootFamilias.AppendChild(familiaNode);

            new DAOXml().guardarDatosXML(doc, ruta);
        }

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
                familias.Add(familia);
            }
            return familias;
        }

        public ObjFamilia LeerFamiliaPorId(int id, string ruta)
        {
            List<ObjFamilia> familias = LeerFamilias(ruta);
            ObjFamilia familia = familias.Find(f => f.Id == id);
            if (familia == null)
            {
                throw new Exception("Familia no encontrada.");
            }
            return familia;
        }

        public void ModificarFamilia(ObjFamilia familia, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode familiaNode = doc.SelectSingleNode($"/datos/familias/familia[id='" + familia.Id + "']");
            if (familiaNode == null)
            {
                throw new Exception("La familia no existe.");
            }

            familiaNode["id"].InnerText = familia.Id.ToString();
            familiaNode["nombre"].InnerText = familia.Nombre;

            new DAOXml().guardarDatosXML(doc, ruta);
        }

        public void EliminarFamilia(int id, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode familiaNode = doc.SelectSingleNode("/datos/familias/familia[id='" + id + "']");
            if (familiaNode == null)
            {
                throw new Exception("La familia no existe.");
            }
            familiaNode.ParentNode.RemoveChild(familiaNode);
            new DAOXml().guardarDatosXML(doc, ruta);
        }
    }
}
