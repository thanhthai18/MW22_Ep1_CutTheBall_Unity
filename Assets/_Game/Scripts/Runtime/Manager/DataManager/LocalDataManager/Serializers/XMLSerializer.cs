using System.IO;
using System.Xml.Serialization;

namespace Runtime.Manager.Data
{
    public static class XMLSerializer
    {
        #region Class Methods

        /// <summary>
        /// Serialize the input object and return the corresponding string of it.
        /// </summary>
        public static string Serialize<T>(T objectToSerialize)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringWriter writer = new StringWriter();
            xml.Serialize(writer, objectToSerialize);
            return writer.ToString();
        }

        /// <summary>
        /// Deserialize the input string and return the corresponding object of it.
        /// </summary>
        public static T Deserialize<T>(string stringToDeserialize)
        {
            XmlSerializer xml = new XmlSerializer(typeof(T));
            StringReader reader = new StringReader(stringToDeserialize);
            return (T)xml.Deserialize(reader);
        }

        #endregion Class Methods
    }
}