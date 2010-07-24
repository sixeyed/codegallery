using System.Runtime.Serialization;
using CloudServiceBus.Entities.Enums;

namespace CloudServiceBus.Entities.ServiceResponses
{
    [DataContract(Namespace = TargetNamespace.CloudServiceBus)]
    public class ServiceResponse
    {
        public ServiceResponse()
        {
            ServiceResponseName = GetType().Name;
        }

        [DataMember]
        public bool IsRequestValid { get; set; }

        [DataMember]
        public ServiceResponseState State { get; set; }

        [DataMember]
        public string ServiceResponseName { get; set; }
    }
}
