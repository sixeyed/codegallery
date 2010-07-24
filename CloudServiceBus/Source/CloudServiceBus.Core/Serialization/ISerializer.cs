using System;

namespace CloudServiceBus.Core.Serialization
{
    /// <summary>
    /// Represents a simple object serializer
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Deserialize a serialized object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializedValue"></param>
        /// <returns></returns>
        object Deserialize(Type type, object serializedValue);

        /// <summary>
        /// Serialize an object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        object Serialize(Type type, object returnValue);
    }
}
