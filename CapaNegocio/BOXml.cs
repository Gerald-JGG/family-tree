using System;
using System.Collections.Generic;
using CapaDatos;
using CapaObjetos;
using System.Xml;

namespace CapaNegocio
{
    public class BOXml
    {
        private DAOXml daoXml;
        public BOXml()
        {
            daoXml = new DAOXml();
        }
        public void CrearXML(string ruta, string nodoRaiz)
        {
            daoXml.CrearXML(ruta, nodoRaiz);
        }
        public void LeerXML(string ruta)
        {
            daoXml.LeerXML(ruta);
        }
        public void guardarDatosXML(XmlDocument doc, string ruta)
        {
            daoXml.guardarDatosXML(doc, ruta);
        }
    }
}