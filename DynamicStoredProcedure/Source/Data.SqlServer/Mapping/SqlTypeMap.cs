using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DynamicSP.Data.SqlServer.Mapping
{
    /// <summary>
    /// Maps known SQL Server parameter types to .NET types
    /// </summary>
    public static class SqlTypeMap
    {
        private static List<DbTypeMapEntry> _DbTypeList = new List<DbTypeMapEntry>();

        /// <summary>
        /// Populates the map of known SqlDbType, DbType and framework types
        /// </summary>
        static SqlTypeMap()
        {
            _DbTypeList.Add(new DbTypeMapEntry(typeof(bool), DbType.Boolean, SqlDbType.Bit));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(byte), DbType.Double, SqlDbType.TinyInt));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(byte[]), DbType.Binary, SqlDbType.Image));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(DateTime), DbType.DateTime, SqlDbType.DateTime));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Decimal), DbType.Decimal, SqlDbType.Decimal));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(double), DbType.Double, SqlDbType.Float));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Guid), DbType.Guid, SqlDbType.UniqueIdentifier));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Int16), DbType.Int16, SqlDbType.SmallInt));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Int32), DbType.Int32, SqlDbType.Int));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(Int64), DbType.Int64, SqlDbType.BigInt));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(object), DbType.Object, SqlDbType.Variant));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.VarChar));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.NVarChar));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.NChar));
            _DbTypeList.Add(new DbTypeMapEntry(typeof(string), DbType.String, SqlDbType.NText));
        }

        public static bool TryGetSqlType(Type frameworkType, ref SqlDbType sqlType)
        {
            var matches = (from type in _DbTypeList
                    where type.Type == frameworkType
                    select type.SqlDbType);
            sqlType = matches.FirstOrDefault();
            return (matches.Count() == 1);
        }

        private struct DbTypeMapEntry
        {
            public Type Type;
            public DbType DbType;
            public SqlDbType SqlDbType;
            public DbTypeMapEntry(Type type, DbType dbType, SqlDbType sqlDbType)
            {
                this.Type = type;
                this.DbType = dbType;
                this.SqlDbType = sqlDbType;
            }
        }
    }
}
