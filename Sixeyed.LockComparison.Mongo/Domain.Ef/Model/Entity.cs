using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public int Id { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public int ParentId { get; set; }

        public int StatusCode { get; set; }

        public virtual ParentEntity ParentEntity { get; set; }

        public virtual ICollection<EntityEvent> Events { get; private set; }
    }
}
