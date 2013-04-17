using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sixeyed.CarValet.Common.Entities.Diagnostics
{
    [DataContract]
    public class Status
    {
        [DataMember]
        public DateTime ServerDate { get; set; }

        [DataMember]
        public StatusType CurrentStatus { get; set; }
    }
}