using System;
using System.Collections.Generic;
using System.Xml;
using CapaObjetos;

namespace CapaDatos
{
    public class DAOPersona
    {
        private XmlDocument doc;

        // Crear una nueva persona en el archivo XML
        public void CrearPersona(ObjPersona persona, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode personaNode = doc.CreateElement("persona");

            XmlElement cedula = doc.CreateElement("cedula");
            cedula.InnerText = persona.Cedula;
            personaNode.AppendChild(cedula);

            XmlElement nombre = doc.CreateElement("nombre");
            nombre.InnerText = persona.Nombre;
            personaNode.AppendChild(nombre);

            XmlElement genero = doc.CreateElement("genero");
            genero.InnerText = persona.Genero;
            personaNode.AppendChild(genero);

            XmlElement fechaNacimiento = doc.CreateElement("fechaNacimiento");
            fechaNacimiento.InnerText = persona.FechaNacimiento.ToString("yyyy-MM-dd");
            personaNode.AppendChild(fechaNacimiento);

            XmlElement edad = doc.CreateElement("edad");
            edad.InnerText = persona.Edad.ToString();
            personaNode.AppendChild(edad);

            XmlElement lugarResidencia = doc.CreateElement("lugarResidencia");
            lugarResidencia.InnerText = persona.LugarResidencia;
            personaNode.AppendChild(lugarResidencia);

            XmlElement estadoCivil = doc.CreateElement("estadoCivil");
            estadoCivil.InnerText = persona.EstadoCivil;
            personaNode.AppendChild(estadoCivil);

            XmlElement fallecido = doc.CreateElement("fallecido");
            fallecido.InnerText = persona.Fallecido.ToString();
            personaNode.AppendChild(fallecido);

            XmlElement casado = doc.CreateElement("casado");
            casado.InnerText = persona.Casado.ToString();
            personaNode.AppendChild(casado);

            XmlElement relacionFamiliar = doc.CreateElement("relacionFamiliar");
            relacionFamiliar.InnerText = persona.RelacionFamiliar;
            personaNode.AppendChild(relacionFamiliar);

            XmlElement foto = doc.CreateElement("foto");
            foto.InnerText = persona.Foto;
            personaNode.AppendChild(foto);

            // Si tiene cónyuge, agregar la relación
            if (persona.Conyuge != null)
            {
                XmlElement conyuge = doc.CreateElement("conyuge");
                conyuge.InnerText = persona.Conyuge.Cedula;
                personaNode.AppendChild(conyuge);
            }

            // Agregar los hijos
            if (persona.Hijos.Count > 0)
            {
                foreach (var hijo in persona.Hijos)
                {
                    XmlElement hijoElement = doc.CreateElement("hijo");
                    hijoElement.InnerText = hijo.Cedula;
                    personaNode.AppendChild(hijoElement);
                }
            }

            XmlNode rootPersonas = doc.SelectSingleNode("/datos/personas");
            rootPersonas.AppendChild(personaNode);

            new DAOXml().guardarDatosXML(doc, ruta);
        }

        // Leer personas desde el archivo XML
        public List<ObjPersona> LeerPersonas(string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            List<ObjPersona> personas = new List<ObjPersona>();
            XmlNodeList nodeList = doc.SelectNodes("/datos/personas/persona");

            foreach (XmlNode node in nodeList)
            {
                ObjPersona persona = new ObjPersona(
                    node["cedula"].InnerText,
                    node["nombre"].InnerText,
                    node["genero"].InnerText,
                    Convert.ToDateTime(node["fechaNacimiento"].InnerText),
                    node["lugarResidencia"].InnerText,
                    node["estadoCivil"].InnerText,
                    node["relacionFamiliar"].InnerText
                )
                {
                    Edad = Convert.ToInt32(node["edad"].InnerText),
                    Fallecido = Convert.ToBoolean(node["fallecido"].InnerText),
                    Casado = Convert.ToBoolean(node["casado"].InnerText),
                    Foto = node["foto"].InnerText
                };

                // Leer cónyuge (si tiene)
                if (node["conyuge"] != null)
                {
                    persona.Conyuge = new ObjPersona(node["conyuge"].InnerText, "", "", DateTime.MinValue, "", "", "");
                }

                // Leer los hijos (si los tiene)
                persona.Hijos = new List<ObjPersona>();
                foreach (XmlNode hijoNode in node.SelectNodes("hijo"))
                {
                    ObjPersona hijo = new ObjPersona(hijoNode.InnerText, "", "", DateTime.MinValue, "", "", "");
                    persona.Hijos.Add(hijo);
                }

                personas.Add(persona);
            }

            return personas;
        }

        // Modificar una persona en el archivo XML
        public void ModificarPersona(ObjPersona persona, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode personaNode = doc.SelectSingleNode($"/datos/personas/persona[cedula='{persona.Cedula}']");

            if (personaNode != null)
            {
                personaNode["nombre"].InnerText = persona.Nombre;
                personaNode["genero"].InnerText = persona.Genero;
                personaNode["fechaNacimiento"].InnerText = persona.FechaNacimiento.ToString("yyyy-MM-dd");
                personaNode["edad"].InnerText = persona.Edad.ToString();
                personaNode["lugarResidencia"].InnerText = persona.LugarResidencia;
                personaNode["estadoCivil"].InnerText = persona.EstadoCivil;
                personaNode["fallecido"].InnerText = persona.Fallecido.ToString();
                personaNode["casado"].InnerText = persona.Casado.ToString();
                personaNode["relacionFamiliar"].InnerText = persona.RelacionFamiliar;
                personaNode["foto"].InnerText = persona.Foto;

                // Modificar cónyuge
                if (persona.Conyuge != null)
                {
                    personaNode["conyuge"].InnerText = persona.Conyuge.Cedula;
                }

                // Modificar los hijos
                var hijosNodeList = personaNode.SelectNodes("hijo");
                foreach (XmlNode hijoNode in hijosNodeList)
                {
                    personaNode.RemoveChild(hijoNode);
                }

                foreach (var hijo in persona.Hijos)
                {
                    XmlElement hijoElement = doc.CreateElement("hijo");
                    hijoElement.InnerText = hijo.Cedula;
                    personaNode.AppendChild(hijoElement);
                }

                new DAOXml().guardarDatosXML(doc, ruta);
            }
            else
            {
                throw new Exception("Persona no encontrada.");
            }
        }

        // Eliminar una persona del archivo XML
        public void EliminarPersona(string cedula, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode personaNode = doc.SelectSingleNode($"/datos/personas/persona[cedula='{cedula}']");

            if (personaNode != null)
            {
                personaNode.ParentNode.RemoveChild(personaNode);
                new DAOXml().guardarDatosXML(doc, ruta);
            }
            else
            {
                throw new Exception("Persona no encontrada.");
            }
        }

        // Buscar una persona por cédula
        public ObjPersona BuscarPersonaPorCedula(string cedula, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode personaNode = doc.SelectSingleNode($"/datos/personas/persona[cedula='{cedula}']");

            if (personaNode != null)
            {
                return new ObjPersona(
                    personaNode["cedula"].InnerText,
                    personaNode["nombre"].InnerText,
                    personaNode["genero"].InnerText,
                    Convert.ToDateTime(personaNode["fechaNacimiento"].InnerText),
                    personaNode["lugarResidencia"].InnerText,
                    personaNode["estadoCivil"].InnerText,
                    personaNode["relacionFamiliar"].InnerText
                )
                {
                    Edad = Convert.ToInt32(personaNode["edad"].InnerText),
                    Fallecido = Convert.ToBoolean(personaNode["fallecido"].InnerText),
                    Casado = Convert.ToBoolean(personaNode["casado"].InnerText),
                    Foto = personaNode["foto"].InnerText
                };
            }
            else
            {
                return null;
            }
        }
    }
}
