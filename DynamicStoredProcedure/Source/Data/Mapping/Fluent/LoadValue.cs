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
    /// <typeparam name="TValue">Type of value to load</typeparam>
    public class LoadValue<TValue>
    {
        /// <summary>
        /// Calls the stored procedure and maps the result to the returned entity
        /// </summary>
        /// <typeparam name="TStoredProcedure">Type of stored procedure</typeparam>
        /// <param name="initialise">Action to initialise stored procedure - e.g. set up input parameters</param>
        /// <param name="execute">Action to execute the stored procedure and return a data reader</param>
        /// <param name="getValue">Function to retrieve value from stored procedure output</param>
        /// <returns>Populated value</returns>
        public TValue From<TStoredProcedure>(Action<TStoredProcedure> initialise,
                                             Action<TStoredProcedure> execute,
                                             Func<TStoredProcedure, TValue> getValue)
            where TStoredProcedure : IDisposable, new()
        {
            TValue value = default(TValue);
            using (TStoredProcedure sp = new TStoredProcedure())
            {
                initialise(sp);
                execute(sp);
                value = getValue(sp);
            }
            return value;
        }
    }
}
