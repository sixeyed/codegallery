using System;
using System.Collections.Generic;
using System.Text;
using TriathlonResults.Entities;
using System.Web.Services;
using TriathlonResults.Central.Services.DAL;

namespace TriathlonResults.Central.Services
{
    /// <summary>
    /// Web service for submitting sector results
    /// </summary>
    [WebService(Namespace = "http://TriathlonResults.Central.Schemas/1.0/")]
    public class ResultService : WebService
    {
        [WebMethod]
        public bool RecordResult(SectorTime result)
        {
            SetSectorTime set = new SetSectorTime();
            set.RaceId = result.RaceId;
            set.SectorId = result.SectorId;
            set.AthleteId = result.AthleteId;
            set.StartTime = result.StartTime;
            set.EndTime = result.EndTime;
            set.Duration = result.Duration;
            set.Execute();
            return true;
        }
    }
}
