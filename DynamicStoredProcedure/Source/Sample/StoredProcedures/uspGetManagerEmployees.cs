using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using DynamicSP.Data.SqlServer.StoredProcedures;
using DynamicSP.Data.SqlServer;

namespace DynamicSP.Sample.Data.SqlServer.StoredProcedures
{

#pragma warning disable 1591

    /// <summary>
    /// Wraps access to uspGetManagerEmployees stored procedure
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SqlStoredProcedure.cs.tt", "1.0.0.0")]
    public class uspGetManagerEmployees : SqlStoredProcedureBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public uspGetManagerEmployees() { }

        /// <summary>
        /// Gets/sets @RETURN_VALUE parameter value
        /// </summary>
        public virtual Int32 RETURN_VALUE { get; set; }
        /// <summary>
        /// Gets/sets @ManagerID parameter value
        /// </summary>
        public virtual Int32 ManagerID { get; set; }

        /// <summary>
        /// Gets the name of the stored procedure to execute
        /// </summary>
        public override string StoredProcedureName
        {
            get { return "uspGetManagerEmployees"; }
        }

        /// <summary>
        /// Gets the name of the database to execute on
        /// </summary>
        public override string DatabaseName
        {
            get { return Database.AdventureWorks; }
        }

        /// <summary>
        /// Creates the parameter collection for the procedure
        /// </summary>
        protected override void CreateParameters()
        {
            this.AddParameter(ParameterName.RETURN_VALUE, SqlDbType.Int, 0, ParameterDirection.ReturnValue);
            this.AddParameter(ParameterName.ManagerID, SqlDbType.Int, ParameterDirection.Input);
        }

        /// <summary>
        /// Maps property values to parameter input values
        /// </summary>
        protected override void SetInputParameterValues()
        {
            this.Parameters[ParameterName.ManagerID].Value = this.ManagerID;
        }

        /// <summary>
        /// Maps property return values to parameter output values
        /// </summary>
        protected override void GetOutputParameterValues()
        {
            //Nothing returned
        }

        /// <summary>
        /// Executes the Stored Procedure
        /// </summary>
        public virtual IDataReader Execute()
        {
            return this.ExecuteReader();
        }

        /// <summary>
        /// Parameter names used in the stored procedure
        /// </summary>
        public struct ParameterName
        {
            public const string RETURN_VALUE = "@RETURN_VALUE";
            public const string ManagerID = "@ManagerID";
        }
    }

#pragma warning restore 1591

}


