using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sixeyed.CarValet.Common.Entities.Diagnostics
{
    [DataContract]
    public enum StatusType
    {
        [EnumMember]
        Healthy,

        [EnumMember]
        MaxCapacity
    }
}