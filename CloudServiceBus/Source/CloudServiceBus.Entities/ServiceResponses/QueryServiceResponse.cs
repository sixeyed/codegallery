using System.Runtime.Serialization;

namespace CloudServiceBus.Entities.ServiceResponses
{
    [DataContract(Namespace = TargetNamespace.CloudServiceBus)]
    public class QueryServiceResponse : ServiceResponse
    {
        [DataMember]
        public string StoreIdentifier { get; set; }

        [DataMember]
        public string ItemKey { get; set; }
    }
}
