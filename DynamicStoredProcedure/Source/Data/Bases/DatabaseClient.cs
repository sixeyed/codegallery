using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DynamicSP.Data.Database;
using System.Threading;
using DynamicSP.Data.Interfaces;
using DynamicSP.Data.Configuration;

namespace DynamicSP.Data.Bases
{
    /// <summary>
    /// Base class representing a client of a database connection
    /// </summary>
    /// <typeparam name="TConnection"></typeparam>
    public abstract class DatabaseClient<TConnection> : IDatabaseClient
        where TConnection : class, IDbConnection, new()
    {
        #region Protected instance members

        /// <summary>
        /// Gets the database connection
        /// </summary>
        private TConnection _connection;

        /// <summary>
        /// Gets the default command execution timeout
        /// </summary>
        protected virtual int DefaultCommandTimeout
        {
            get { return 30; }
        }

        /// <summary>
        /// Gets the deault separation marker to use for splitting and joining strings
        /// </summary>
        public virtual string SeparationMarker
        {
            get { return ";"; }
        }

        /// <summary>
        /// Gets an open database connection
        /// </summary>
        protected TConnection Connection
        {
            get
            {
                if (this._connection == null || string.IsNullOrEmpty(this._connection.ConnectionString))
                {
                    this._connection = this.GetConnection();
                    this.ConfigureConnectionString();
                    if (this._connection.State != ConnectionState.Open)
                    {
                        this._connection.Open();
                    }
                }
                return this._connection;
            }
        }

        /// <summary>
        /// Configures connection string
        /// </summary>
        protected virtual void ConfigureConnectionString()
        {
            //do nothing in the base class
        }
        
        /// <summary>
        /// Gets the timeout specified in databaseCommandConfiguration for the class
        /// </summary>
        protected int GetConfiguredTimeout()
        {
            int timeoutSeconds = DefaultCommandTimeout;
            if (DatabaseCommandConfiguration.Current != null)
            {
                DatabaseCommand commandConfig = DatabaseCommandConfiguration.Current.Commands[this.GetType().Name];
                if (commandConfig != null)
                {
                    timeoutSeconds = commandConfig.CommandTimeout;
                }
            }
            return timeoutSeconds;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// Name of the database - used to retrieve connection details from config
        /// </summary>
        public abstract string DatabaseName { get;}

        #endregion

        #region Private instance methods

        private TConnection GetConnection()
        {
            return (TConnection) DatabaseFactory.GetConnection<TConnection>(this.DatabaseName);
        }

        #endregion
        
        #region IDisposable Members

        /// <summary>
        /// Cleans up database resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Cleans up database resources
        /// </summary>
        /// <param name="disposing">Whether disposing</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && this._connection != null)
            {
                //check if this connection is part of a transaction:
                if (TransactionBase<TConnection>.InTransactionScope(this))
                {
                    //if so, do nothing - the connection will be closed when the 
                    //transaction is disposed
                }
                else
                {
                    //otherwise, this is a solo connection, so dispose it:
                    if (this._connection.State == ConnectionState.Open)
                    {
                        this._connection.Close();
                    }
                    this._connection.Dispose();
                    this._connection = null;
                }
            }
        }
        #endregion
    }
}
