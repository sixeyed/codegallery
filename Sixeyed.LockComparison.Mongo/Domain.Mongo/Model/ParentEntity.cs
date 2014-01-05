using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ef.Model
{
    public class ParentEntity
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }

        public string CorrelationId { get; set; }
    }
}
