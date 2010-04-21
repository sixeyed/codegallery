using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace DynamicSP.Data.Mapping
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

        /// <summary>
        /// Begin fluently loading a value
        /// </summary>
        /// <typeparam name="TValue">Type of value</typeparam>
        /// <returns>LoadValue</returns>
        public static LoadValue<TValue> LoadValue<TValue>()
        {
            return new LoadValue<TValue>();
        }

        /// <summary>
        /// Begin fluently loading a dataset
        /// </summary>
        /// <typeparam name="TDataSet">Type of dataset</typeparam>
        /// <returns>LoadDataSet</returns>
        public static LoadDataSet<TDataSet> LoadDataSet<TDataSet>()
            where TDataSet : DataSet
        {
            return new LoadDataSet<TDataSet>();
        }
    }
}
