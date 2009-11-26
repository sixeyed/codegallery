using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Sixeyed.OptimisticLockingSample.Entities
{
    [DataContract(Namespace="http://Sixeyed.OptimisticLockingSample/2009")]
    public class Customer : IExtensibleDataObject
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public DateTime StartDate { get; set; }

        [DataMember]
        public Single CreditLimit { get; set; }

        [DataMember]
        public byte[] Logo { get; set; }

        public ExtensionDataObject ExtensionData {get; set;}
    }
}
