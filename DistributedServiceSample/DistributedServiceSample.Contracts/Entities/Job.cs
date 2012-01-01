using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace DistributedServiceSample.Contracts.Entities
{
    [DataContract(Namespace=TargetNamespace.DistributedServiceSample)]
    public class Job
    {
        [DataMember]
        public string Name { get; set; }
    }
}
