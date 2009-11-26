using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Linq.Expressions;
using FluentDAL.Mapping;

namespace FluentDAL
{
    /// <summary>
    /// Class for fluently loading an entity from a data reader, using
    /// a stored procedure call and a map
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to load</typeparam>
    public class Load<TEntity>
        where TEntity : class, new()
    {
        private IDataReaderMap _map;

        /// <summary>
        /// Adds a map to use in loading the entity
        /// </summary>
        /// <typeparam name="TMap">Type of map</typeparam>
        /// <returns>Load</returns>
        public Load<TEntity> With<TMap>()
            where TMap : IDataReaderMap, new()
        {
            this._map = new TMap();
            return this;
        }

        /// <summary>
        /// Calls the stored procedure and maps the result to the returned entity
        /// </summary>
        /// <typeparam name="TStoredProcedure">Type of stored procedure</typeparam>
        /// <param name="initialise">Action to initialise stored procedure - e.g. set up input parameters</param>
        /// <param name="execute">Function to execute the stored procedure and return a data reader</param>
        /// <returns>Populated entity</returns>
        public TEntity From<TStoredProcedure>(Action<TStoredProcedure> initialise, Expression<Func<TStoredProcedure, IDataReader>> execute)
            where TStoredProcedure : IDisposable, new()
        {
            if (_map == null)
            {
                throw new InvalidOperationException("Map must be populated from With<> call");
            }
            TEntity entity = new TEntity();
            using (TStoredProcedure sp = new TStoredProcedure())
            {
                initialise(sp);
                Func<TStoredProcedure, IDataReader> executor = execute.Compile();
                using (IDataReader reader = executor.Invoke(sp))
                {
                    if (entity is IList)
                    {
                        _map.PopulateList((IList)entity, reader);
                    }
                    else
                    {
                        if (reader.Read())
                        {
                            _map.Populate(entity, reader);
                        }
                    }
                }
            }
            return entity;
        }
    }
}
