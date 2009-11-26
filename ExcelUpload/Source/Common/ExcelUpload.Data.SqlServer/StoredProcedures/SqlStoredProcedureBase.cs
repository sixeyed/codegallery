using ExcelUpload.Data.Bases;
using System.Data.SqlClient;
using System.Data;

namespace ExcelUpload.Data.SqlServer.StoredProcedures
{
    /// <summary>
    /// Represents a SQL Server stored procedure
    /// </summary>
    public abstract class SqlStoredProcedureBase : StoredProcedureBase<SqlConnection, SqlDataAdapter, SqlParameter>
    {
        /// <summary>
        /// Adds a parameter to the command's collection
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="type">Parameter data type</param>
        /// <param name="direction">Paremeter direction</param>
        protected void AddParameter(string name, SqlDbType type, ParameterDirection direction)
        {
            AddParameter(name, type, null, direction);
        }

        /// <summary>
        /// Adds a parameter to the command's collection
        /// </summary>
        /// <param name="name">Parameter name</param>
        /// <param name="type">Parameter data type</param>
        /// <param name="size">Parameter size</param>
        /// <param name="direction">Paremeter direction</param>
        protected void AddParameter(string name, SqlDbType type, int? size, ParameterDirection direction)
        {
            SqlParameter parameter = new SqlParameter(name, type);
            if (size.HasValue)
            {
                parameter.Size = size.Value;
            }
            parameter.Direction = direction;
            this.Parameters.Add(name, parameter);
        }
    }
}
