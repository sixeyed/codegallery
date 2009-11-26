using System;
using System.Collections.Generic;
using System.Text;

namespace TriathlonResults.Central.Services.Tests.TestSQL
{
    public partial class TriathlonResult
    {
        public struct SectorTimes
        {
            public const string GetDurationFormat = "select Duration from SectorTimes where RaceId={0} and SectorId={1} and AthleteId={2};";
        }
    }
}
