using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Sixeyed.CarValet.Services.Entities
{
    [DataContract]
    public class Model
    {
        [DataMember]
        public string MakeCode { get; set; }

        [DataMember]
        public string Code { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}