using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TriathlonResults.Central.Services.DAL
{
    /// <summary>
    /// Base class for executing SQL Server stored procedures
    /// </summary>
    public abstract class SqlStoredProcedure
    {
        protected IDataReader GetReader()
        {
            SqlCommand command = GetCommand();
            return command.ExecuteReader();
        }

        protected void ExecuteNonQuery()
        {
            SqlCommand command = GetCommand();
            command.ExecuteNonQuery();
        }

        private SqlCommand GetCommand()
        {
            SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["TriathlonResults"].ConnectionString);
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = this.StoredProcedureName;
            if (this.Parameters.Count > 0)
            {
                this.SetParameterInputValues();
                foreach (SqlParameter parameter in this.Parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            return command;
        }

        public abstract void SetParameterInputValues();
        public abstract string StoredProcedureName { get;}

        protected List<SqlParameter> _parameters;
        public List<SqlParameter> Parameters
        {
            get { return this._parameters; }
        }
    }
}
