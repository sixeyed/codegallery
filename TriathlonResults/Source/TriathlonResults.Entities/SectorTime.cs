using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace TriathlonResults.Entities
{
    /// <summary>
    /// Represents an athlete's time for completing a given sector
    /// </summary>
    [Serializable()]
    [XmlRoot(Namespace = "http://TriathlonResults.Central.Schemas/1.0/")]
    public class SectorTime
    {
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
            get 
            { 
                CalculateDuration(this);
                return this._duration;
            }
            set { _duration = value; }
        }

        public static void CalculateDuration(SectorTime time)
        {
            if (time._duration < 1)
            {
                //calculate:
                time._duration = CalculateDuration(time.StartTime, time.EndTime);
            }
        }

        public static int CalculateDuration(DateTime startTime, DateTime endTime)
        {
            return (int)new TimeSpan(endTime.TimeOfDay.Ticks - startTime.TimeOfDay.Ticks).TotalSeconds;
        }
    }
}
