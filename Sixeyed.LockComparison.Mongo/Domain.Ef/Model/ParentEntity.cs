using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ef.Model
{
    public class ParentEntity
    {
        public ParentEntity()
        {
            ChildEntities = new List<Entity>();
        }

        public int Id { get; set; }

        [MaxLength(50)]
        public string CorrelationId { get; set; }

        [MaxLength(255)]
        public string Name { get; set; }

        public virtual ICollection<Entity> ChildEntities { get; private set; }
    }
}
