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
            int edad = DateTime.Now.Year - persona.FechaNacimiento.Year;

            AgregarElemento("cedula", persona.Cedula);
            AgregarElemento("familia", persona.Familia.ToString());
            AgregarElemento("nombre", persona.Nombre);
            AgregarElemento("genero", persona.Genero);
            AgregarElemento("fechaNacimiento", persona.FechaNacimiento.ToString("yyyy-MM-dd"));
            AgregarElemento("edad", edad.ToString());
            AgregarElemento("lugarResidencia", persona.LugarResidencia);
            AgregarElemento("estadoCivil", persona.EstadoCivil);
            AgregarElemento("conyuge", persona.Conyuge.ToString());
            AgregarElemento("fallecido", persona.Fallecido.ToString());
            AgregarElemento("relacionFamiliar", persona.RelacionFamiliar);
            AgregarElemento("foto", persona.Foto);
            AgregarElemento("padre", persona.Padre.ToString());
            AgregarElemento("madre", persona.Madre.ToString());

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
                ObjPersona persona = new ObjPersona
                {
                    Cedula = node["cedula"].InnerText,
                    Familia = node["familia"] != null ? Convert.ToInt32(node["familia"].InnerText) : 0,
                    Nombre = node["nombre"].InnerText,
                    Genero = node["genero"].InnerText,
                    FechaNacimiento = Convert.ToDateTime(node["fechaNacimiento"].InnerText),
                    LugarResidencia = node["lugarResidencia"].InnerText,
                    EstadoCivil = node["estadoCivil"].InnerText,
                    Fallecido = Convert.ToBoolean(node["fallecido"].InnerText),
                    RelacionFamiliar = node["relacionFamiliar"].InnerText,
                    Foto = node["foto"].InnerText,
                    Conyuge = node["conyuge"] != null ? Convert.ToInt32(node["conyuge"].InnerText) : 0,
                    Padre = node["padre"] != null ? Convert.ToInt32(node["padre"].InnerText) : 0,
                    Madre = node["madre"] != null ? Convert.ToInt32(node["madre"].InnerText) : 0,
                    Edad = Convert.ToInt32(node["edad"].InnerText)
                };
                personas.Add(persona);
            }
            return personas;
        }



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
                personaNode["relacionFamiliar"].InnerText = persona.RelacionFamiliar;
                personaNode["foto"].InnerText = persona.Foto;
                personaNode["conyuge"].InnerText = persona.Conyuge.ToString();
                personaNode["padre"].InnerText = persona.Padre.ToString();
                personaNode["madre"].InnerText = persona.Madre.ToString();
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

        // Buscar una persona por cédula en el archivo XML
        public ObjPersona BuscarPersonaPorCedula(string cedula, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode personaNode = doc.SelectSingleNode($"/datos/personas/persona[cedula='{cedula}']");

            if (personaNode != null)
            {
                ObjPersona persona = new ObjPersona
                {
                    Cedula = personaNode["cedula"].InnerText,
                    Familia = Convert.ToInt32(personaNode["familia"].InnerText),
                    Nombre = personaNode["nombre"].InnerText,
                    Genero = personaNode["genero"].InnerText,
                    FechaNacimiento = Convert.ToDateTime(personaNode["fechaNacimiento"].InnerText),
                    LugarResidencia = personaNode["lugarResidencia"].InnerText,
                    EstadoCivil = personaNode["estadoCivil"].InnerText,
                    RelacionFamiliar = personaNode["relacionFamiliar"].InnerText,
                    Fallecido = Convert.ToBoolean(personaNode["fallecido"].InnerText),
                    Foto = personaNode["foto"]?.InnerText ?? (personaNode["genero"].InnerText == "Masculino" ? "avatar_masculino.jpg" : "avatar_femenino.jpg"),
                    Conyuge = Convert.ToInt32(personaNode["conyuge"]?.InnerText ?? "0"),
                    Padre = Convert.ToInt32(personaNode["padre"]?.InnerText ?? "0"),
                    Madre = Convert.ToInt32(personaNode["madre"]?.InnerText ?? "0"),
                    Edad = Convert.ToInt32(personaNode["edad"]?.InnerText)
                };

                return persona;
            }
            return null;
        }

        // Buscar una persona por nombre en el archivo XML
        public ObjPersona BuscarPersonaPorNombre(string nombre, string ruta)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(ruta);
            XmlNode personaNode = doc.SelectSingleNode($"/datos/personas/persona[nombre='{nombre}']");

            if (personaNode != null)
            {
                ObjPersona persona = new ObjPersona
                {
                    Cedula = personaNode["cedula"].InnerText,
                    Familia = Convert.ToInt32(personaNode["familia"].InnerText),
                    Nombre = personaNode["nombre"].InnerText,
                    Genero = personaNode["genero"].InnerText,
                    FechaNacimiento = Convert.ToDateTime(personaNode["fechaNacimiento"].InnerText),
                    LugarResidencia = personaNode["lugarResidencia"].InnerText,
                    EstadoCivil = personaNode["estadoCivil"].InnerText,
                    RelacionFamiliar = personaNode["relacionFamiliar"].InnerText,
                    Fallecido = Convert.ToBoolean(personaNode["fallecido"].InnerText),
                    Foto = personaNode["foto"]?.InnerText ?? (personaNode["genero"].InnerText == "Masculino" ? "avatar_masculino.jpg" : "avatar_femenino.jpg"),
                    Conyuge = Convert.ToInt32(personaNode["conyuge"]?.InnerText ?? "0"),
                    Padre = Convert.ToInt32(personaNode["padre"]?.InnerText ?? "0"),
                    Madre = Convert.ToInt32(personaNode["madre"]?.InnerText ?? "0"),
                    Edad = Convert.ToInt32(personaNode["edad"]?.InnerText)
                };
                return persona;
            }
            return null;
        }
    }
}
