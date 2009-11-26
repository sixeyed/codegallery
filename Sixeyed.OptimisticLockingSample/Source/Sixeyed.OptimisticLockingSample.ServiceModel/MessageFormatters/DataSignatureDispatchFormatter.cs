using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Sixeyed.OptimisticLockingSample.ServiceModel.MessageFormatters
{
    /// <summary>
    /// Dispatch formatter which computes a DataSignature from a DataContract object 
    /// and adds it to outgoing message headers
    /// </summary>
    public class DataSignatureDispatchFormatter : IDispatchMessageFormatter
    {
        /// <summary>
        /// Gets/sets the original formatter in the WCF stack
        /// </summary>
        public IDispatchMessageFormatter OrginalFormatter { get; set; }

        /// <summary>
        /// Deserializes incoming requests using the original formatter
        /// </summary>
        /// <param name="message">Incoming message</param>
        /// <param name="parameters">Parameters</param>
        public void DeserializeRequest(Message message, object[] parameters)
        {
            this.OrginalFormatter.DeserializeRequest(message, parameters);
        }

        /// <summary>
        /// Serializes outgoing response using the original formatter, and adding
        /// the computed DataSignature as a message header
        /// </summary>
        /// <param name="messageVersion"></param>
        /// <param name="parameters"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public Message SerializeReply(MessageVersion messageVersion, object[] parameters, object result)
        {
            //create the message using the standard formatter:
            Message reply = this.OrginalFormatter.SerializeReply(messageVersion, parameters, result);
            if (result != null)
            {
                //if the result is a DataContract, compute data signature 
                //and add it as a message header:
                object[] attributes = result.GetType().GetCustomAttributes(typeof(DataContractAttribute), true);
                if (attributes != null && attributes.Length > 0)
                {
                    reply.Headers.Add(DataSignature.CreateHeader(result));
                }
            }

            return reply;
        }
    }
}