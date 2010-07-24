using System.Runtime.Serialization;

namespace CloudServiceBus.Entities.ServiceRequests.Operations
{
    [DataContract(Namespace = TargetNamespace.CloudServiceBus)]
    public class FlushDataStoreRequest : ServiceRequest
    {
        [DataMember]
        public string StoreIdentifier { get; set; }
    }
}
