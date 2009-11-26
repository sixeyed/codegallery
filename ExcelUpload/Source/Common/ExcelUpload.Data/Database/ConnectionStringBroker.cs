using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace ExcelUpload.Data.Database
{
    /// <summary>
    /// Broker for retrieving connection strings for a database
    /// </summary>
    public static class ConnectionStringBroker
    {
        private static ConnectionStringSettingsCollection _connectionStrings;
        private static ConnectionStringSettingsCollection ConnectionStrings
        {
            get
            {
                if (_connectionStrings == null)
                {
                    _connectionStrings = ConfigurationManager.ConnectionStrings;
                }
                return _connectionStrings;
            }
        }

        private static Dictionary<string, string> _fileConnectionStrings;
        private static Dictionary<string, string> FileConnectionStrings
        {
            get
            {
                if (_fileConnectionStrings == null)
                {
                    _fileConnectionStrings = new Dictionary<string, string>();
                    _fileConnectionStrings.Add(FileSuffix.Excel, FileConnectionString.Excel);
                }
                return _fileConnectionStrings;
            }
        }

        /// <summary>
        /// Gets the connection string for the specified database name
        /// </summary>
        /// <param name="databaseName">Name of database</param>
        /// <returns>Database connection string</returns>
        public static string GetConnectionString(string databaseName)
        {
            string connectionString = null;
            //use the strings from application config:
            if (ConnectionStrings[databaseName] != null)
            {
                connectionString = ConnectionStrings[databaseName].ConnectionString;
            }
            else
            {
                //check if the database is a file:
                connectionString = GetFileConnectionString(databaseName);                
            }
            //if none found, throw:
            if (connectionString == null)
            {
                throw new ConfigurationErrorsException(string.Format("DatabaseName: {0} not configured and is not a file source", databaseName));
            }
            return connectionString;
        }

        /// <summary>
        /// Resets the cached connection strings
        /// </summary>
        public static void Reset()
        {
            _connectionStrings = null;
        }

        private static string GetFileConnectionString(string databaseName)
        {
            string connectionString = null;
            foreach (KeyValuePair<string, string> fileConnectionString in FileConnectionStrings)
            {
                //key contains the file suffix, e.g. ".xls":
                if (databaseName.EndsWith(fileConnectionString.Key))
                {
                    //value contains the connection string, to be formatted with the file name:
                    connectionString = string.Format(fileConnectionString.Value, databaseName);
                    break;
                }
            }
            return connectionString;
        }

        private struct FileSuffix
        {
            public const string Excel = ".xls";
        }

        private struct FileConnectionString
        {
            public const string Excel = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=\"Excel 8.0;HDR=YES\"";
        }
    }
}
