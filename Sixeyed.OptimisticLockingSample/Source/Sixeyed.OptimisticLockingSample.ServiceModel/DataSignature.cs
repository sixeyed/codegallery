using System;
using System.IO;
using System.Security.Cryptography;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml.Serialization;

namespace Sixeyed.OptimisticLockingSample.ServiceModel
{
    /// <summary>
    /// Generates data signatures for serializable objects
    /// </summary>
    public static class DataSignature
    {
        /// <summary>
        /// Creates a message header containing a data signature for the object
        /// </summary>
        /// <param name="serializableObject">Object to sign</param>
        /// <returns>Message header</returns>
        public static MessageHeader CreateHeader(object serializableObject)
        {
            Guid dataSignature = Sign(serializableObject);
            return MessageHeader.CreateHeader(Header.Name, Header.Namespace, dataSignature);
        }

        /// <summary>
        /// Gets the data signature from the current WCF call
        /// </summary>
        /// <returns>Data signature or Guid.Empty</returns>
        public static Guid Current
        {
            get { return GetDataSignature(OperationContext.Current.IncomingMessageHeaders); }
        }

        /// <summary>
        /// Gets the data signature in the given message header collection
        /// </summary>
        /// <param name="headers">Message headers</param>
        /// <returns>Data signature or Guid.Empty</returns>
        public static Guid GetDataSignature(MessageHeaders headers)
        {
            Guid dataSignature = Guid.Empty;
            int headerIndex = headers.FindHeader(Header.Name, Header.Namespace);
            if (headerIndex != -1)
            {
                dataSignature = headers.GetHeader<Guid>(headerIndex);
            }
            return dataSignature;
        }

        /// <summary>
        /// Returns the data signature for the given object
        /// </summary>
        /// <param name="serializableObject">Objetc to sign</param>
        /// <returns>Data signature</returns>
        public static Guid Sign(object serializableObject)
        {
            //get XML representation:
            string xml = Serialize(serializableObject);

            //use MD5 hash to get a 16-byte hash of the string: 
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(xml);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            //generate a guid from the hash: 
            Guid hashGuid = new Guid(hashBytes);
            return hashGuid;
        }

        /// <summary>
        /// Serializes the given object using the XML serializer
        /// </summary>
        /// <param name="serializableObject">Object to serialize</param>
        /// <returns>XML representation</returns>
        private static string Serialize(object serializableObject)
        {
            string serializedString = null;
            XmlSerializer fromSerializer = new XmlSerializer(serializableObject.GetType());
            using (StringWriter writer = new StringWriter())
            {
                fromSerializer.Serialize(writer, serializableObject);
                writer.Flush();
                serializedString = writer.ToString();
            }
            return serializedString;
        }

        /// <summary>
        /// Constants used to reference data signatures
        /// </summary>
        public struct Header
        {
            public const string Name = "DataSignature";
            public const string Namespace = "http://Sixeyed.OptimisticLockingSample/2009";
        }

        /// <summary>
        /// Constants used to reference data signatures
        /// </summary>
        public struct ExtensionDataMember
        {
            public const string Name = "dataSignature";
        }
    }
}