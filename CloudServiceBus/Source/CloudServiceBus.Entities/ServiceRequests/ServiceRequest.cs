using System.Runtime.Serialization;

namespace CloudServiceBus.Entities.ServiceRequests
{
    [DataContract(Namespace = TargetNamespace.CloudServiceBus)]
    public class ServiceRequest
    {
        public ServiceRequest()
        {
            ServiceRequestName = GetType().Name;
        }

        [DataMember]
        public string ResponseQueueUrl { get; set; }

        [DataMember]
        public string ServiceRequestName { get; set; }
    }
}
