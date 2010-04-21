using DynamicSP.Data.Bases;
using System.Data.SqlClient;
using System.Data;
using System;
using System.Collections.Generic;
using System.Dynamic;

namespace DynamicSP.Data.SqlServer.StoredProcedures
{
    /// <summary>
    /// Represents a SQL Server stored procedure
    /// </summary>
    internal class SqlStoredProcedure : StoredProcedureBase<SqlConnection, SqlDataAdapter, SqlParameter>
    {
        public SqlStoredProcedure() { }

        private string _storedProcedureName;
        private string _databaseName;
        
        public int RETURN_VALUE { get; private set;  }
        
        /// <summary>
        /// Returns the database name to execute against
        /// </summary>
        public override string DatabaseName
        {
            get { return _databaseName; }
        }

        /// <summary>
        /// Returns the name of the stored procedure to execute
        /// </summary>
        public override string StoredProcedureName
        {
            get { return _storedProcedureName; }
        }

        /// <summary>
        /// Adds a parameter to the command's collection
        /// </summary>
        /// <param name="propertyName">Property name</param>
        /// /// <param name="propertyValue">Property value</param>
        /// <param name="type">Parameter data type</param>
        /// <param name="direction">Paremeter direction</param>
        internal void AddParameter(string propertyName, object propertyValue, SqlDbType type, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(GetParameterName(propertyName), type);
            parameter.Direction = direction;
            parameter.Value = propertyValue;
            Parameters.Add(propertyName, parameter);
        }

        /// <summary>
        /// Returns the SqlParameter with the given parameter name
        /// </summary>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        internal SqlParameter GetParameter(string parameterName)
        {
            return Parameters[parameterName];
        }

        /// <summary>
        /// Executes the stored procedure
        /// </summary>
        /// <returns>IDataReader</returns>
        internal new IDataReader ExecuteReader()
        {
            return base.ExecuteReader();
        }

        /// <summary>
        /// Sets the name of the database to execute against
        /// </summary>
        /// <param name="databaseName"></param>
        internal void SetDatabaseName(string databaseName)
        {
            _databaseName = databaseName;
        }

        /// <summary>
        /// Sets the name of the stored procedure to execute
        /// </summary>
        /// <param name="storedProcedureName"></param>
        internal void SetStoredProcedureName(string storedProcedureName)
        {
            _storedProcedureName = storedProcedureName;
        }

        /// <summary>
        /// Returns the SQL parameter name used for a property
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        internal string GetParameterName(string propertyName)
        {
            return string.Format("@{0}", propertyName);
        }
        
        protected override void CreateParameters()
        {
            this.AddParameter(ParameterName.RETURN_VALUE, SqlDbType.Int, 0, ParameterDirection.ReturnValue);
        }

        private string GetPropertyName(string parameterName)
        {
            return parameterName.Substring(1);
        }

        protected override void SetInputParameterValues()
        {
            //not used
        }

        protected override void GetOutputParameterValues()
        {
            //not used
        }

        public struct ParameterName
        {
            public const string RETURN_VALUE = "@RETURN_VALUE";
        }
    }
}
