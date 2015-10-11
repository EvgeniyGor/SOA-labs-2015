using System;
using System.Collections.Concurrent;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Serialization
{
    public class XmlSerializer : ISerializer
    {
        private readonly ConcurrentDictionary<Type, System.Xml.Serialization.XmlSerializer> _cache = new ConcurrentDictionary<Type, System.Xml.Serialization.XmlSerializer>(); 

        public string Serialize<T>(T obj) where T : class
        {
            var serializer = _cache.GetOrAdd(typeof (T), i => new System.Xml.Serialization.XmlSerializer(typeof(T)));

            using (var stringWriter = new StringWriter())
            {
                var xmlSetting = new XmlWriterSettings
                {
                    OmitXmlDeclaration = true,
                    NewLineHandling = NewLineHandling.None
                };

                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlSetting))
                {
                    var xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add("", "");
                    serializer.Serialize(xmlWriter, obj, xmlSerializerNamespaces);
                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(string serializedObj) where T : class
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof (T));

            using (var stringReader = new StringReader(serializedObj))
            {
                using (var xmlReader = XmlReader.Create(stringReader))
                {
                    return serializer.Deserialize(xmlReader) as T;
                }
            }
        }
    }
}