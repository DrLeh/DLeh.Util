using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DLeh.Util
{
    public static class XmlHelpers
    {
        public static string XmlSerializeObjectAsString<T>(T data)
        {
            using (var stringWriter = new StringWriter())
            {
                var serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(stringWriter, data);
                return stringWriter.ToString();
            }
        }

        public static XDocument XmlSerializeObject<T>(T data)
        {
            var str = XmlSerializeObjectAsString(data);
            return XDocument.Parse(str);
        }

        public static T DeserializeXml<T>(string xml)
        {
            using (var reader = new StringReader(xml))
            using (var xmlReader = XmlReader.Create(reader))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(xmlReader);
            }
        }
    }
}
