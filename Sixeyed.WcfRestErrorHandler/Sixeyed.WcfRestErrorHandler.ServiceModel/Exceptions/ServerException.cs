using System;
using System.Runtime.Serialization;

namespace Sixeyed.WcfRestErrorHandler.ServiceModel.Exceptions
{
    /// <summary>
    /// Represents an exception which has been caused by unexpected server behaviour
    /// </summary>
    [DataContract]
    public class ServerException : Exception
    {
        [DataMember]
        public Guid ErrorId { get; private set; }

        [DataMember]
        public string Message { get; private set; }

        public ServerException()
        {
            ErrorId = Guid.NewGuid();
            Message = string.Format("Something has gone wrong. Please contact our support team with helpdesk ID: {0}", ErrorId);
        }
    }
}
