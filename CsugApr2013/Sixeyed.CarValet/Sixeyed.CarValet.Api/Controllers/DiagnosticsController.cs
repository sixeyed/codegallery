using Sixeyed.CarValet.Common.Entities.Diagnostics;
using System;
using System.Web.Http;

namespace Sixeyed.CarValet.Api.Controllers
{
    public class DiagnosticsController : ApiController
    {
        public Status GetStatus()
        {
            return new Status() { ServerDate = DateTime.Now, CurrentStatus = StatusType.Healthy };
        }
    }
}