using System;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Xml;
using Sixeyed.OptimisticLockingSample.ServiceModel.Helpers;

namespace Sixeyed.OptimisticLockingSample.ServiceModel.MessageFormatters
{
    /// <summary>
    /// Client formatter which extracts a DataSignature from incoming message headers
    /// and adds it to the ExtensionData property of a DataContract, and extracts
    /// the DataSignature from ExtensionData to add to outgoing message headers
    /// </summary>
    public class DataSignatureClientFormatter : IClientMessageFormatter
    {
        /// <summary>
        /// Gets/sets the original formatter in the WCF stack
        /// </summary>
        public IClientMessageFormatter OrginalFormatter { get; set; }

        /// <summary>
        /// Deserializes the response message, extracting the DataSignature from the message
        /// headers and adding it to the XML content of the message's data payload
        /// </summary>
        /// <remarks>
        /// Modifies the XML payload then uses the original formatter to deserialize the message. 
        /// If the original formatter uses DataContractSerializer then the DataContract from the
        /// message will have the DataSignature header value added to its ExtensionDataObject
        /// </remarks>
        /// <param name="message">Incoming message</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Deserialized object</returns>
        public object DeserializeReply(Message message, object[] parameters)
        {
            //get the DataSignature from the message headers:
            Guid dataSignature = DataSignature.GetDataSignature(message.Headers);
            if (dataSignature != Guid.Empty)
            {
                //TODO - replace with XLINQ
                XmlDictionaryReader reader = message.GetReaderAtBodyContents();
                XmlDocument document = new XmlDocument();
                document.Load(reader);

                //we add the data signature as a node under the document namespace:
                XmlNode resultNode = document.DocumentElement.FirstChild;
                XmlElement dataSignatureNode = document.CreateElement(DataSignature.ExtensionDataMember.Name, document.NamespaceURI);
                dataSignatureNode.InnerText = dataSignature.ToString();
                resultNode.InsertAfter(dataSignatureNode, resultNode.LastChild);

                MemoryStream memStream = new MemoryStream();
                document.Save(memStream);
                memStream.Position = 0;

                XmlDictionaryReaderQuotas quotas = new XmlDictionaryReaderQuotas();
                XmlDictionaryReader xdr = XmlDictionaryReader.CreateTextReader(memStream, quotas);

                //replace the message - the data signature will be desrialized into
                //the ExtendedData property of the IExtensibleDataObject:
                Message replacedMessage = Message.CreateMessage(message.Version, null, xdr);
                replacedMessage.Headers.CopyHeadersFrom(message.Headers);
                replacedMessage.Properties.CopyProperties(message.Properties);
                message = replacedMessage;
            }

            //now use the standard formatter - DataContractSerializer will
            //add the new dataSignature element as an ExtensionData object:         
            return this.OrginalFormatter.DeserializeReply(message, parameters); ;
        }

        /// <summary>
        /// Uses the original formatter to serialize the response, then extracts the DataSignature
        /// (if available) and adds it to the message headers of the outgoing request
        /// </summary>
        /// <remarks>
        /// Extracts DataSignature from ExtensionDataObject using non-public members, so will fail
        /// if the internal representation of ExtensionDataObject is modified
        /// </remarks>
        /// <param name="messageVersion"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Message SerializeRequest(MessageVersion messageVersion, object[] parameters)
        {
            //create the message using the standard formatter:
            Message request = this.OrginalFormatter.SerializeRequest(messageVersion, parameters);
            //if any parameters have a data signature, add the first found
            //as a header to the outgoing message:
            foreach (object parameter in parameters)
            {
                if (parameter is IExtensibleDataObject)
                {
                    object dataSignature = ExtensionDataHelper.GetExtensionDataMemberValue((IExtensibleDataObject) parameter, DataSignature.ExtensionDataMember.Name);
                    if (dataSignature != null)
                    {
                        request.Headers.Add(MessageHeader.CreateHeader(DataSignature.Header.Name, DataSignature.Header.Namespace, dataSignature));
                        break;
                    }  
                }
            }

            return request;
        }
    }
}