using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sixeyed.CarValet.Api.Models
{
    public class VehicleModel
    {
        public string Make { get; set; }

        public string Model { get; set; }

        public string ImageUrl { get; set; }

        public DateTime ProducedFromUtc { get; set; }

        public DateTime? ProducedToUtc { get; set; }
    }
}