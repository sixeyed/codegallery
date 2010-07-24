using System;
using System.Collections;
using System.Runtime.Serialization;

namespace CloudServiceBus.Core.Serialization
{
    /// <summary>
    /// Base class for <see cref="ISerializer"/> implementations
    /// </summary>
    public abstract class BaseSerializer : ISerializer
    {
        /// <summary>
        /// Internal serialization implementation
        /// </summary>
        /// <param name="type"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        protected abstract object SerializeInternal(Type type, object returnValue);

        /// <summary>
        /// Internal deserialization implementation
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cachedValue"></param>
        /// <returns></returns>
        protected abstract object DeserializeInternal(Type type, object cachedValue);

        /// <summary>
        /// Deserialize object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="cachedValue"></param>
        /// <returns></returns>
        public object Deserialize(Type type, object cachedValue)
        {
            if (!NeedsSerializing(type, cachedValue))
                return cachedValue;

            return DeserializeInternal(type, cachedValue);      
        }        

        /// <summary>
        /// Serialize object
        /// </summary>
        /// <param name="type"></param>
        /// <param name="returnValue"></param>
        /// <returns></returns>
        public object Serialize(Type type, object returnValue)
        {            
            if (!NeedsSerializing(type, returnValue))
                return returnValue;

            return SerializeInternal(type, returnValue);            
        }

        private static bool NeedsSerializing(Type type, object value)
        {
            return (value != null &&
                    (IsDataContract(type) || 
                     IsDataContractCollection(type, value)));
        }

        private static bool IsDataContract(Type type)
        {
            return (type.GetCustomAttributes(typeof(DataContractAttribute), false).Length == 1);
        }

        private static bool IsDataContractCollection(Type type, object value)
        {
            if (type.IsArray)
            {
                return IsDataContract(type.GetElementType());
            }
            else if (value is IEnumerable)
            {
                var enumerator = ((IEnumerable)value).GetEnumerator();
                if (enumerator.MoveNext())
                {
                    return IsDataContract(enumerator.Current.GetType());
                }
            }       
            return false;
        }
    }
}
