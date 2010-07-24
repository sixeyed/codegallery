using System;
using System.Runtime.Serialization;

namespace CloudServiceBus.Entities.ServiceRequests
{
    [DataContract(Namespace = TargetNamespace.CloudServiceBus)]
    public class GetChangedEmployeesRequest : ServiceRequest
    {
        [DataMember]
        public DateTime LastChangeDate { get; set; }

        public override string ToString()
        {
            return string.Format("GetChangedEmployeesRequest[LastChangeDate: {0}]", LastChangeDate);
        }
    }
}
