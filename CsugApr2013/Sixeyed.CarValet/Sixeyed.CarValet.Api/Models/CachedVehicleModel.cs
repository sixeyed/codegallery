using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sixeyed.CarValet.Api.Models
{
    public class CachedVehicleModel
    {
        public ObjectId Id {get; set;}

        public string CacheKey { get; set; }

        public VehicleModel Model { get; set; }

        public static string GetCacheKey(string makeCode, string modelCode)
        {
            return string.Format("{0}:{1}", makeCode, modelCode);
        }
    }
}