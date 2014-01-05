using Domain.Ef.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Ef
{
    public class EntityContext : DbContext
    {
        public DbSet<EntityEvent> EntityEvents { get; set; }
        public DbSet<Entity> Entities { get; set; }
        public DbSet<ParentEntity> ParentEntities { get; set; }
    }
}
