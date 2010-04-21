using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DynamicSP.Data.Configuration;
using System.Collections.ObjectModel;
using DynamicSP.Core.Logging;

namespace DynamicSP.Data.Bases
{
    /// <summary>
    /// Base class providing functionality to execute a stored procedure
    /// </summary>
    /// <typeparam name="TConnection">Type of IDbConnection</typeparam>
    /// <typeparam name="TDataAdapter">Type of IDbDataAdapter</typeparam>
    public abstract class QueryBase<TConnection, TDataAdapter> : DatabaseClient<TConnection> 
        where TConnection : class, IDbConnection, new()
        where TDataAdapter : IDbDataAdapter, new()
    {
        #region Private instance fields

        private string _commandText;

        #endregion

        #region Protected instance properties

        /// <summary>
        /// Gets the command type of the query
        /// </summary>
        protected virtual CommandType CommandType
        {
            get { return CommandType.Text; }
        }

        /// <summary>
        /// Gets the command behaviour of the query
        /// </summary>
        protected virtual CommandBehavior CommandBehavior
        {
            get { return CommandBehavior.Default; }
        }

        /// <summary>
        /// Gets the command text of the query
        /// </summary>
        protected virtual string CommandText
        {
            get { return this._commandText; }
        }

        #endregion

        #region Protected instance methods

        /// <summary>
        /// Placeholder to allow inheritors to react after database call;
        /// </summary>
        protected virtual void AfterExecute(){}
        
        /// <summary>
        /// Builds the command to be executed
        /// </summary>
        /// <returns>Prepared IDbCommand</returns>
        protected virtual IDbCommand PrepareCommand()
        {
            IDbCommand command = this.Connection.CreateCommand();
            command.CommandType = this.CommandType;
            command.CommandText = this.CommandText;
            //join transaction if present:
            if (TransactionBase<TConnection>.InTransactionScope(this))
            {
                command.Transaction = TransactionBase<TConnection>.Current;
            }
            this.SetCommandTimeout(command);
            return command;
        }

        /// <summary>
        /// Checks databaseCommandConfiguration to see if the class has a specified timeout
        /// </summary>
        /// <param name="command">Command to set timeout</param>
        protected virtual void SetCommandTimeout(IDbCommand command)
        {
            //check if the command has a specific timeout configured:
            command.CommandTimeout = GetConfiguredTimeout();
        }

        /// <summary>
        /// Executes the command, returning a single value of type T
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="commandText">Command to execute</param>
        /// <returns>Stored procedure call result</returns>
        protected T ExecuteScalar<T>(string commandText)
        {
            this._commandText = commandText;
            return this.ExecuteScalar<T>();
        }

        /// <summary>
        /// Executes the stored procedure, returning a single value of type T
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <returns>Stored procedure call result</returns>
        protected T ExecuteScalar<T>() 
        {
            object value;
            T typedValue = default(T);
            using (IDbCommand command = this.PrepareCommand())
            {
                value = command.ExecuteScalar();
                AfterExecute();
                command.Parameters.Clear();
            }
            if (value != null)
            {
                try
                {
                    typedValue = (T) value;
                }
                catch (Exception ex)
                {
                    Log.Error("{0}.ExecuteScalar - error casting return value {1} to {2}: {3}", this.GetType().Name, value, typeof(T).Name, ex.Message);
                    try
                    {
                        value = (T)Convert.ChangeType(value, typeof(T));
                    }
                    catch (Exception ex2) 
                    {
                        Log.Error("{0}.ExecuteScalar - error changing type of return value {1} to {2}: {3}", this.GetType().Name, value, typeof(T).Name, ex2.Message);
                    }
                }
            }
            return typedValue;
        }

        /// <summary>
        /// Executes the stored command, returning a DataSet
        /// </summary>
        /// <param name="commandText">Command to execute</param>
        /// <returns>Command result</returns>
        protected DataSet ExecuteDataSet(string commandText)
        {
            this._commandText = commandText;
            return this.ExecuteDataSet();
        }

        /// <summary>
        /// Executes the stored procedure, returning a DataSet
        /// </summary>
        /// <returns>Stored procedure call result</returns>
        protected DataSet ExecuteDataSet()
        {
            DataSet ds = new DataSet();
            using (IDbCommand command = this.PrepareCommand())
            {
                IDbDataAdapter adapter = new TDataAdapter();
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                AfterExecute();
                command.Parameters.Clear();
            }
            return ds;
        }

        /// <summary>
        /// Executes the command, returning an IDataReader
        /// </summary>
        /// <param name="commandText">Command to execute</param>
        /// <returns>Command result</returns>
        protected IDataReader ExecuteReader(string commandText)
        {
            this._commandText = commandText;
            return this.ExecuteReader();
        }

        /// <summary>
        /// Executes the stored procedure, returning an IDataReader
        /// </summary>
        /// <returns>Stored procedure call result</returns>
        protected virtual IDataReader ExecuteReader()
        {
            IDataReader reader;
            using (IDbCommand command = this.PrepareCommand())
            {
                reader = command.ExecuteReader(this.CommandBehavior);
                AfterExecute();
                command.Parameters.Clear();
            }
            return reader;
        }

        /// <summary>
        /// Executes command without returning a result
        /// </summary>
        /// <param name="commandText">Command to execute</param>
        /// <returns>Number of rows affected by call</returns>
        protected int ExecuteNonQuery(string commandText)
        {
            this._commandText = commandText;
            return this.ExecuteNonQuery();            
        }

        /// <summary>
        /// Executes the stored procedure without returning a result
        /// </summary>
        /// <returns>Return value of query call</returns>
        protected int ExecuteNonQuery()
        {
            int returnValue;
            using (IDbCommand command = this.PrepareCommand())
            {
                returnValue = command.ExecuteNonQuery();
                AfterExecute();
                command.Parameters.Clear();
            }
            return returnValue;
        }

        /// <summary>
        /// Joins the given list parts into a single string, separated by commas
        /// </summary>
        /// <remarks>
        /// Use for constructing comma-separated lists for IN() SQL clauses 
        /// </remarks>
        /// <param name="listParts">Parts to join</param>
        /// <returns>Joined list</returns>
        protected string GetSeparatedList(List<string> listParts)
        {
            return GetSeparatedList(listParts.ToArray());
        }

        /// <summary>
        /// Joins the given list parts into a single string, separated by commas
        /// </summary>
        /// <remarks>
        /// Use for constructing comma-separated lists for IN() SQL clauses 
        /// </remarks>
        /// <param name="listParts">Parts to join</param>
        /// <returns>Joined list</returns>
        protected string GetSeparatedList(string[] listParts)
        {
            return GetSeparatedList(listParts, ",");
        }

        /// <summary>
        /// Joins the given list parts into a single string, separated by the given separator
        /// </summary>
        /// <param name="listParts">Parts to join</param>
        /// <param name="separator">Separator to use</param>
        /// <returns>Joined list</returns>
        protected string GetSeparatedList(string[] listParts, string separator)
        {
            return string.Join(separator, listParts);
        }

        #endregion
    }
}
