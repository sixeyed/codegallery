using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DistributedServiceSample.Contracts.Entities
{
    [DataContract(Namespace=TargetNamespace.DistributedServiceSample)]
    public class ComputeResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ResponseMessage { get; set; }

        [DataMember]
        public long DurationMilliseconds { get; set; }
    }
}
