using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.OptimisticLockingSample.EntityServices.Interfaces
{
    public interface IEntityService<TEntity>
    {
        Type EntityType { get; }
        TEntity Load(TEntity entity);
        void Update(TEntity entity);
    }
}
