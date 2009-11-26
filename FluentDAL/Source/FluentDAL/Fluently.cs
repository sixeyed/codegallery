using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentDAL
{
    /// <summary>
    /// Helper class for fluently loading objects from database calls
    /// </summary>
    public static class Fluently
    {
        /// <summary>
        /// Begin fluently loading an entity
        /// </summary>
        /// <typeparam name="TEntity">Type of entity</typeparam>
        /// <returns>Load</returns>
        public static Load<TEntity> Load<TEntity>()
            where TEntity : class, new()
        {
            return new Load<TEntity>();
        }
    }
}
