using System;
using System.Runtime.Serialization;

namespace Sixeyed.WcfRestErrorHandler.ServiceModel.Exceptions
{    
    /// <summary>
    /// Represents an exception which has been caused by the client rerquest - 
    /// e.g. validation failure, or requesting a resource which doesn't exist
    /// </summary>
    [DataContract]
    public class ClientException : Exception
    {
        [DataMember]
        public string Message { get; private set; }

        public ClientException(string messageFormat, params object[] args)
        {
            Message = string.Format(messageFormat, args);
        }
    }
}
