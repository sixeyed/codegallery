using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelUpload.Data.Interfaces
{
    /// <summary>
    /// Represents a client of a database connection
    /// </summary>
    public interface IDatabaseClient : IDisposable
    {
        /// <summary>
        /// Returns the database name to act against
        /// </summary>
        /// <remarks>
        /// Used for connection string lookup in configuration
        /// </remarks>
        string DatabaseName { get;}
    }
}
