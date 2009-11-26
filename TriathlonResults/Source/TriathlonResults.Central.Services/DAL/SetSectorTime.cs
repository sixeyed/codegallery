using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace TriathlonResults.Central.Services.DAL
{
    /// <summary>
    /// Class for executing SetSectorTime stored procedure
    /// </summary>
    public class SetSectorTime : SqlStoredProcedure
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

        private DateTime _startTime = DateTime.MinValue;
        public DateTime StartTime
        {
            get { return _startTime; }
            set { _startTime = value; }
        }

        private DateTime _endTime = DateTime.MinValue;
        public DateTime EndTime
        {
            get { return _endTime; }
            set { _endTime = value; }
        }

        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }

        public SetSectorTime()
        {
            this._parameters = new List<SqlParameter>();
            this._parameters.Add(new SqlParameter(ParameterName.RaceId, SqlDbType.Int));
            this._parameters.Add(new SqlParameter(ParameterName.SectorId, SqlDbType.Int));
            this._parameters.Add(new SqlParameter(ParameterName.AthleteId, SqlDbType.Int));
            this._parameters.Add(new SqlParameter(ParameterName.StartTime, SqlDbType.DateTime));
            this._parameters.Add(new SqlParameter(ParameterName.EndTime, SqlDbType.DateTime));
            this._parameters.Add(new SqlParameter(ParameterName.Duration, SqlDbType.Int));
        }

        public override void SetParameterInputValues()
        {
            this.Parameters[0].Value = this.RaceId;
            this.Parameters[1].Value = this.SectorId;
            this.Parameters[2].Value = this.AthleteId;
            SetDateParameter(this.Parameters[3], this.StartTime);
            SetDateParameter(this.Parameters[4], this.EndTime);
            this.Parameters[5].Value = this.Duration;
        }

        private void SetDateParameter(SqlParameter parameter, DateTime value)
        {
            if (value != DateTime.MinValue)
            {
                parameter.Value = value;
            }
            else
            {
                parameter.Value = DBNull.Value;
            }
        }

        public override string StoredProcedureName
        {
            get { return "SetSectorTime"; }
        }

        public void Execute()
        {
            this.ExecuteNonQuery();
        }

        private struct ParameterName
        {
            public const string RaceId = "@RaceId";
            public const string SectorId = "@SectorId";
            public const string AthleteId = "@AthleteId";
            public const string StartTime = "@StartTime";
            public const string EndTime = "@EndTime";
            public const string Duration = "@Duration";
        }
    }
}
