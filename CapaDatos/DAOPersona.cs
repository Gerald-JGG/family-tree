using System;
using System.Collections.Generic;
using System.Xml;
using CapaObjetos;

namespace CapaDatos
{
    public class DAOPersona
    {
        // Crear una nueva persona en el archivo XML
        public void CrearPersona(ObjPersona persona, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);

            XmlNode personaNode = doc.CreateElement("persona");

            void AgregarElemento(string nombre, string valor)
            {
                XmlElement element = doc.CreateElement(nombre);
                element.InnerText = valor;
                personaNode.AppendChild(element);
            }

            AgregarElemento("cedula", persona.Cedula);
            AgregarElemento("nombre", persona.Nombre);
            AgregarElemento("genero", persona.Genero);
            AgregarElemento("fechaNacimiento", persona.FechaNacimiento.ToString("yyyy-MM-dd"));
            AgregarElemento("edad", persona.Edad.ToString());
            AgregarElemento("lugarResidencia", persona.LugarResidencia);
            AgregarElemento("estadoCivil", persona.EstadoCivil);
            AgregarElemento("fallecido", persona.Fallecido.ToString());
            AgregarElemento("casado", persona.Casado.ToString());
            AgregarElemento("relacionFamiliar", persona.RelacionFamiliar);
            AgregarElemento("foto", persona.Foto);

            if (persona.Conyuge != null)
                AgregarElemento("conyuge", persona.Conyuge.Cedula);

            foreach (var hijo in persona.Hijos)
                AgregarElemento("hijo", hijo.Cedula);

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
                // Modificar para no asignar un valor nulo a Familia (se usa 0 si no existe el campo en XML)
                ObjPersona persona = new ObjPersona(
                    node["cedula"].InnerText,
                    0,  // Valor por defecto para Familia
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

                // Asignar Conyuge solo si existe en XML
                if (node["conyuge"] != null && !string.IsNullOrEmpty(node["conyuge"].InnerText))
                {
                    persona.Conyuge = new ObjPersona(node["conyuge"].InnerText, 0, "", "", DateTime.MinValue, "", "", "");
                }

                // Asignar Hijos
                foreach (XmlNode hijoNode in node.SelectNodes("hijo"))
                {
                    persona.Hijos.Add(new ObjPersona(hijoNode.InnerText, 0, "", "", DateTime.MinValue, "", "", ""));
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

                if (persona.Conyuge != null)
                    personaNode["conyuge"].InnerText = persona.Conyuge.Cedula;

                foreach (XmlNode hijoNode in personaNode.SelectNodes("hijo"))
                    personaNode.RemoveChild(hijoNode);

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

        public ObjPersona BuscarPersonaPorCedula(string cedula, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode personaNode = doc.SelectSingleNode($"/datos/personas/persona[cedula='{cedula}']");

            if (personaNode != null)
            {
                ObjPersona persona = new ObjPersona(
                    personaNode["cedula"].InnerText,
                    Convert.ToInt32(personaNode["familia"].InnerText),
                    personaNode["nombre"].InnerText,
                    personaNode["genero"].InnerText,
                    Convert.ToDateTime(personaNode["fechaNacimiento"].InnerText),
                    personaNode["lugarResidencia"].InnerText,
                    personaNode["estadoCivil"].InnerText,
                    personaNode["relacionFamiliar"].InnerText
                );

                // Datos adicionales
                persona.Fallecido = Convert.ToBoolean(personaNode["fallecido"].InnerText);
                persona.Casado = Convert.ToBoolean(personaNode["casado"].InnerText);
                persona.Foto = personaNode["foto"]?.InnerText ?? (persona.Genero == "Masculino" ? "avatar_masculino.jpg" : "avatar_femenino.jpg");

                // Relaciones (Hijos y Padres)
                persona.Hijos = new List<ObjPersona>();
                persona.Padres = new List<ObjPersona>();

                XmlNodeList hijosNodes = personaNode.SelectNodes("hijos/persona");
                foreach (XmlNode hijoNode in hijosNodes)
                {
                    ObjPersona hijo = BuscarPersonaPorCedula(hijoNode.InnerText, ruta);
                    if (hijo != null)
                    {
                        persona.Hijos.Add(hijo);
                    }
                }

                XmlNodeList padresNodes = personaNode.SelectNodes("padres/persona");
                foreach (XmlNode padreNode in padresNodes)
                {
                    ObjPersona padre = BuscarPersonaPorCedula(padreNode.InnerText, ruta);
                    if (padre != null)
                    {
                        persona.Padres.Add(padre);
                    }
                }

                return persona;
            }
            return null;
        }
    }
}
