using System;
using System.Dynamic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace DXC.Technology.Objects
{
    /// <summary>
    /// Helper methods for serialization/deserialization to and from strings
    /// Namespace is not optimal yet...
    /// </summary>
    public class SerializationHelper
    {
        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public SerializationHelper() { }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Deserialize JSON text to an object.
        /// </summary>
        /// <param name="jsonText">The JSON string to deserialize.</param>
        /// <returns>The deserialized object.</returns>
        public static object JsonDeserializeToObject(string jsonText)
        {
            dynamic deserializedObject = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            return deserializedObject;
        }

        /// <summary>
        /// Deserialize JSON text to a strongly-typed object.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="jsonText">The JSON string to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T JsonDeserializeToObject<T>(string jsonText)
        {
            dynamic deserializedObject = JsonConvert.DeserializeObject<T>(jsonText);
            return deserializedObject;
        }

        /// <summary>
        /// Serialize an object to JSON.
        /// </summary>
        /// <param name="item">The object to serialize.</param>
        /// <returns>A JSON string representation of the object.</returns>
        public static string ObjectSerializeToJson(object item)
        {
            return JsonConvert.SerializeObject(item);
        }

        /// <summary>
        /// Serialize an object to XML using the XmlSerializer.
        /// </summary>
        /// <param name="item">The object to serialize.</param>
        /// <returns>An XML string representation of the object.</returns>
        public static string XmlSerialize(object item)
        {
            string serializedXml = string.Empty;
            try
            {
                var xmlSerializer = new XmlSerializer(item.GetType());
                using (var stringWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(stringWriter, item);
                    serializedXml = stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Serialization failed.", ex);
            }
            return serializedXml;
        }

        /// <summary>
        /// Deserialize an XML string to an object using the XmlSerializer.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="xmlString">The XML string to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T XmlDeserialize<T>(string xmlString)
        {
            using (var stringReader = new StringReader(xmlString))
            {
                var serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(stringReader);
            }
        }

        /// <summary>
        /// Serialize an object to basic XML with no namespace and no XML declaration tag.
        /// </summary>
        /// <param name="item">The object to serialize.</param>
        /// <returns>A pure XML string representation of the object.</returns>
        public static string XmlSerializeToBasicXml(object item)
        {
            string serializedXml = string.Empty;
            var xmlWriterSettings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true,
                CloseOutput = true,
                Encoding = Encoding.UTF8
            };

            StringWriter stringWriter = null;
            try
            {
                stringWriter = new StringWriter();
                using (var xmlWriter = XmlWriter.Create(stringWriter, xmlWriterSettings))
                {
                    var xmlSerializer = new XmlSerializer(item.GetType());
                    var xmlSerializerNamespaces = new XmlSerializerNamespaces();
                    xmlSerializerNamespaces.Add(string.Empty, string.Empty);

                    xmlSerializer.Serialize(xmlWriter, item, xmlSerializerNamespaces);
                    serializedXml = stringWriter.ToString();
                    return serializedXml;
                }
            }
            finally
            {
                stringWriter?.Dispose();
            }
        }

        /// <summary>
        /// Serialize an object to XML using the DataContractSerializer.
        /// </summary>
        /// <param name="item">The object to serialize.</param>
        /// <returns>An XML string representation of the object.</returns>
        public static string DataContractSerialize(object item)
        {
            if (item != null)
            {
                using (var memoryStream = new MemoryStream())
                {
                    var serializer = new DataContractSerializer(item.GetType());
                    serializer.WriteObject(memoryStream, item);
                    return Encoding.UTF8.GetString(memoryStream.ToArray());
                }
            }
            return null;
        }

        /// <summary>
        /// Deserialize an XML string to an object using the DataContractSerializer.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize to.</typeparam>
        /// <param name="xmlString">The XML string to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T DataContractDeserialize<T>(string xmlString)
        {
            XmlDictionaryReader xmlDictionaryReader = null;
            try
            {
                xmlDictionaryReader = XmlDictionaryReader.CreateTextReader(Encoding.UTF8.GetBytes(xmlString), XmlDictionaryReaderQuotas.Max);
                var serializer = new DataContractSerializer(typeof(T));
                return (T)serializer.ReadObject(xmlDictionaryReader, false);
            }
            finally
            {
                xmlDictionaryReader?.Close();
            }
        }

        #endregion
    }
}