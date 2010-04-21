using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Collections;
using System.Linq.Expressions;

namespace DynamicSP.Data.Mapping
{
    /// <summary>
    /// Class for fluently loading an entity from a data reader, using
    /// a stored procedure call and a map
    /// </summary>
    /// <typeparam name="TDataSet">Type of DataSet to load</typeparam>
    public class LoadDataSet<TDataSet>
        where TDataSet : DataSet
    {
        /// <summary>
        /// Calls the stored procedure and populates a dataset with the results
        /// </summary>
        /// <typeparam name="TStoredProcedure">Type of stored procedure</typeparam>
        /// <param name="initialise">Action to initialise stored procedure - e.g. set up input parameters</param>
        /// <param name="execute">Function to execute the stored procedure and return a data reader</param>
        /// <returns>Populated dataset</returns>
        public TDataSet From<TStoredProcedure>(Action<TStoredProcedure> initialise, Func<TStoredProcedure, DataSet> execute)
            where TStoredProcedure : IDisposable, new()
        {
            TDataSet dataSet = default(TDataSet);
            using (TStoredProcedure sp = new TStoredProcedure())
            {
                initialise(sp);
                dataSet = (TDataSet) execute(sp);
            }
            return dataSet;
        }
    }
}
