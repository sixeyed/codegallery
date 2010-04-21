using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using DynamicSP.Data.SqlServer.Mapping;
using System.Data;
using System.Data.SqlClient;
using DynamicSP.Core.Logging;

namespace DynamicSP.Data.SqlServer.StoredProcedures
{
    public class DynamicSqlStoredProcedure : Bases.DynamicStoredProcedure
    {
        SqlStoredProcedure _proc = new SqlStoredProcedure();

        public DynamicSqlStoredProcedure(string procedureName, string databaseName)
        {
            _proc.SetStoredProcedureName(procedureName);
            _proc.SetDatabaseName(databaseName);
        }

        public int RETURN_VALUE
        {
            get { return _proc.RETURN_VALUE; }
        }

        /// <summary>
        /// Adds a dynamic member as a stored procedure parameter
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SqlDbType sqlType = default(SqlDbType);
            if (SqlTypeMap.TryGetSqlType(value.GetType(), ref sqlType))
            {
                _proc.AddParameter(binder.Name, value, sqlType, ParameterDirection.Input);
                return true;
            }
            else
            {
                Log.Error("DynamicSqlStoredProcedure.TrySetMember - Unable to add member name: {0}, object type: {1}", binder.Name, value.GetType());
                return false;
            }
        }

        /// <summary>
        /// Retrieves a dynamic member as a stored procedure parameter value
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var parameterName = _proc.GetParameterName(binder.Name);
            var parameter = _proc.GetParameter(parameterName);
            if (parameter == null)
            {
                result = null;
                return false;
            }
            result = parameter.Value;
            return true;
        }

        public override IDataReader Execute()
        {
            return _proc.ExecuteReader();
        }

        public override void Dispose()
        {
            _proc.Dispose();
        }
    }
}
