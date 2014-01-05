using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ef.Model
{
    public class EntityEvent
    {
        public int Id { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public int EntityId { get; set; }

        public DateTime CreatedAt { get; set; }

        public int StatusCode { get; set; }

        public virtual Entity Entity {get; set;}
    }
}
