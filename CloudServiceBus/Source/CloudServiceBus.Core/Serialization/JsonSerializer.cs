using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CloudServiceBus.Core.Serialization
{
    /// <summary>
    /// Serializes and deserializes objects using JSON
    /// </summary>
    public class JsonSerializer : BaseSerializer
    {
        /// <summary>
        /// Serialize to JSON format
        /// </summary>
        /// <param name="type"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        protected override object SerializeInternal(Type type, object returnValue)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, returnValue);
                stream.Flush();
                string value = Encoding.UTF8.GetString(stream.GetBuffer(), 0, (int)stream.Length);
                return value;
            }
        }

        /// <summary>
        /// Deserialize from JSON format
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cachedValue"></param>
        /// <returns></returns>
        protected override object DeserializeInternal(Type type, object cachedValue)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(type);
            byte[] data = Encoding.UTF8.GetBytes(cachedValue.ToString());
            using (MemoryStream stream = new MemoryStream(data))
            {
                return serializer.ReadObject(stream);
            }
        }
    }
}
