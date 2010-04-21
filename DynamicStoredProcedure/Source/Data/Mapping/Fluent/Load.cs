using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Linq.Expressions;
//using DynamicSP.Data.Mapping.Fluent;
using System.Dynamic;

namespace DynamicSP.Data.Mapping
{
    /// <summary>
    /// Class for fluently loading an entity from a data reader, using
    /// a stored procedure call and a map
    /// </summary>
    /// <typeparam name="TEntity">Type of entity to load</typeparam>
    public class Load<TEntity>
        where TEntity : class, new()
    {
        private IDataReaderMap _dataReaderMap;

        /// <summary>
        /// Adds a map to use in loading the entity
        /// </summary>
        /// <typeparam name="TMap">Type of map</typeparam>
        /// <returns>Load</returns>
        public Load<TEntity> With<TMap>()
            where TMap : IDataReaderMap, new()
        {
            this._dataReaderMap = new TMap();
            return this;
        }

        /// <summary>
        /// Calls the dynamic stored procedure wrapper and maps the result to the returned entity
        /// </summary>
        /// <param name="DynamicSP">Initialised stored procedure wrapper</param>
        /// <returns>Populated entity</returns>
        public TEntity From(Bases.DynamicStoredProcedure DynamicSP)
        {
            ValidateMap();
            TEntity entity = new TEntity();
            using (IDataReader reader = DynamicSP.Execute())
            {
                RunMap(entity, reader);
            }
            DynamicSP.Dispose();
            return entity;
        }

        /// <summary>
        /// Calls the stored procedure wrapper and maps the result to the returned entity
        /// </summary>
        /// <typeparam name="TStoredProcedure">Type of stored procedure</typeparam>
        /// <param name="initialise">Action to initialise stored procedure - e.g. set up input parameters</param>
        /// <param name="execute">Function to execute the stored procedure and return a data reader</param>
        /// <returns>Populated entity</returns>
        public TEntity From<TStoredProcedure>(Action<TStoredProcedure> initialise, Func<TStoredProcedure, IDataReader> execute)
            where TStoredProcedure : IDisposable, new()
        {
            ValidateMap();
            TEntity entity = new TEntity();
            using (TStoredProcedure sp = new TStoredProcedure())
            {
                initialise(sp);
                using (IDataReader reader = execute(sp))
                {
                    RunMap(entity, reader);
                }
            }
            return entity;
        }

        private void ValidateMap()
        {
            if (_dataReaderMap == null)
            {
                throw new InvalidOperationException("Map must be populated from With<> call");
            }
        }
        
        private void RunMap(TEntity entity, IDataReader reader)
        {
            IList list = entity as IList;
            if (list != null)
            {
                _dataReaderMap.PopulateList(list, reader);
            }
            else
            {
                if (reader.Read())
                {
                    _dataReaderMap.Populate(entity, reader);
                }
            }
        }
    }
}
