using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.InteropServices;

namespace TriathlonResults.Central.ServiceRequests.Entities
{
    /// <summary>
    /// Represents an athlete's time for completing a given sector
    /// </summary>
    [Guid("F8B52B94-32C6-4804-966C-3A7133D94ED1")]
    [ComDefaultInterface(typeof(ISectorTime))]
    [Serializable()]
    [XmlRoot(Namespace = "http://TriathlonResults.Central.Schemas/1.0/")]
    public class SectorTime : ISectorTime
    {
        public SectorTime() { }

        private int _raceId;

        public int RaceId
        {
            get { return _raceId; }
            set { _raceId = value; }
        }
        private int _sectorId;

        public int SectorId
        {
            get { return _sectorId; }
            set { _sectorId = value; }
        }
        private int _athleteId;

        public int AthleteId
        {
            get { return _athleteId; }
            set { _athleteId = value; }
        }
        private DateTime _startTime;

        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }
        private DateTime _endTime;

        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }
        private int _duration;

        /// <summary>
        /// Athlete's duration in the sector, recorded in seconds
        /// </summary>
        public int Duration
        {
            get { return this._duration; }
            set { _duration = value; }
        }
    }

    [Guid("9D3E0674-FD47-4f89-9DC6-1922A7EEC314")]
    public interface ISectorTime
    {
        int AthleteId { get; set; }
        int Duration { get; set; }
        DateTime EndTime { get; set; }
        int RaceId { get; set; }
        int SectorId { get; set; }
        DateTime StartTime { get; set; }
    }
}
