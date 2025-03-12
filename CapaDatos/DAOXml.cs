using System;
using System.Collections.Generic;
using System.Xml;
using CapaObjetos;

namespace CapaDatos
{
    public class DAOXml
    {
        private XmlDocument doc;

        public void guardarDatosXML(XmlDocument doc, string ruta)
        {
            doc.Save(ruta);
        }

        public void CrearXML(string ruta, string nodoRaiz)
        {
            doc = new XmlDocument();
            XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", "no");

            XmlNode nodo = doc.DocumentElement;
            doc.InsertBefore(xmlDeclaration, nodo);

            XmlNode elemento = doc.CreateElement(nodoRaiz);
            doc.AppendChild(elemento);

            new DAOXml().guardarDatosXML(doc, ruta);
        }

        public void LeerXML(string ruta)
        {
            doc = new XmlDocument();
            doc.Load(ruta);
        }
    }
}
