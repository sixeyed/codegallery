using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ef.Model
{
    public class EntityEvent
    {
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }

        public int StatusCode { get; set; }
    }
}
