using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using ExcelUpload.Data.Bases;

namespace ExcelUpload.Data.Database
{
    /// <summary>
    /// Factory class returning a database connection
    /// </summary>
    public static class DatabaseFactory
    {
        /// <summary>
        /// Returns a connection for the given database name
        /// </summary>
        /// <typeparam name="TConnection">Type of connection to return</typeparam>
        /// <param name="databaseName">Name of database to connect to</param>
        /// <returns>Database connection</returns>
        public static IDbConnection GetConnection<TConnection>(string databaseName) 
            where TConnection : class, IDbConnection, new()
        {
            //check the thread for an existing connection:
            IDbConnection connection = GetConnectionFromThread<TConnection>(databaseName);
            //if none, we're not in a transaction so create a new connection:
            if (connection == null)
            {
                string connectionString = ConnectionStringBroker.GetConnectionString(databaseName);
                connection = new TConnection();
                connection.ConnectionString = connectionString;
            }
            return connection;
        }

        private static IDbConnection GetConnectionFromThread<TConnection>(string databaseName)
            where TConnection : class, IDbConnection, new()
        {
            IDbConnection connection = null;
            //check to see if there's an active transaction in the thread
            //for the specified database - if so, use its connection so clients share
            //the same transaction scope:
            if (TransactionBase<TConnection>.InTransactionScope(databaseName))
            {
                connection = TransactionBase<TConnection>.Current.Connection;
            }
            return connection;
        }
    }
}
