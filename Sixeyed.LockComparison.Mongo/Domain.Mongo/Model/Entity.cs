using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ef.Model
{
    public class Entity
    {
        public Entity()
        {
            Events = new List<EntityEvent>();
        }

        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public int StatusCode { get; set; }

        public string CorrelationId { get; set; }

        public List<EntityEvent> Events { get; set; }
    }
}
