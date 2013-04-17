using Sixeyed.CarValet.Common.Entities.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace Sixeyed.CarValet.Services
{
    [ServiceContract]
    public class DiagnosticsService
    {
        [OperationContract]
        [WebGet(UriTemplate = "status", ResponseFormat = WebMessageFormat.Json)]
        public Status GetStatus()
        {
            return new Status() { ServerDate = DateTime.Now, CurrentStatus = StatusType.Healthy };
        }
    }
}
