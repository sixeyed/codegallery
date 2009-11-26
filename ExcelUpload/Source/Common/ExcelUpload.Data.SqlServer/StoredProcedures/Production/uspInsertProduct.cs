using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace ExcelUpload.Data.SqlServer.StoredProcedures
{
    /// <summary>
    /// Wraps access to [Production].[uspInsertProduct] stored procedure on AdventureWorks
    /// </summary>
    [System.CodeDom.Compiler.GeneratedCode("SqlStoredProcedure.cs.tt", "1.0.0.0")]
    public class uspInsertProduct : SqlStoredProcedureBase
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public uspInsertProduct() { }

        /// <summary>
        /// Gets/sets @RETURN_VALUE parameter value
        /// </summary>
        public virtual Int32 RETURN_VALUE { get; set; }
        /// <summary>
        /// Gets/sets @Name parameter value
        /// </summary>
        public virtual String Name { get; set; }
        /// <summary>
        /// Gets/sets @ProductNumber parameter value
        /// </summary>
        public virtual String ProductNumber { get; set; }
        /// <summary>
        /// Gets/sets @SafetyStockLevel parameter value
        /// </summary>
        public virtual Int32 SafetyStockLevel { get; set; }
        /// <summary>
        /// Gets/sets @ReorderPoint parameter value
        /// </summary>
        public virtual Int32 ReorderPoint { get; set; }
        /// <summary>
        /// Gets/sets @StandardCost parameter value
        /// </summary>
        public virtual Decimal StandardCost { get; set; }
        /// <summary>
        /// Gets/sets @ListPrice parameter value
        /// </summary>
        public virtual Decimal ListPrice { get; set; }
        /// <summary>
        /// Gets/sets @DaysToManufacture parameter value
        /// </summary>
        public virtual Int32 DaysToManufacture { get; set; }
        /// <summary>
        /// Gets/sets @SellStartDate parameter value
        /// </summary>
        public virtual DateTime SellStartDate { get; set; }


        /// <summary>
        /// Gets the name of the stored procedure to execute
        /// </summary>
        public override string SPName
        {
            get { return "[Production].[uspInsertProduct]"; }
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
            this.AddParameter(ParameterName.Name, SqlDbType.NVarChar, 50, ParameterDirection.Input);
            this.AddParameter(ParameterName.ProductNumber, SqlDbType.NVarChar, 25, ParameterDirection.Input);
            this.AddParameter(ParameterName.SafetyStockLevel, SqlDbType.SmallInt, 0, ParameterDirection.Input);
            this.AddParameter(ParameterName.ReorderPoint, SqlDbType.SmallInt, 0, ParameterDirection.Input);
            this.AddParameter(ParameterName.StandardCost, SqlDbType.Money, 0, ParameterDirection.Input);
            this.AddParameter(ParameterName.ListPrice, SqlDbType.Money, 0, ParameterDirection.Input);
            this.AddParameter(ParameterName.DaysToManufacture, SqlDbType.Int, 0, ParameterDirection.Input);
            this.AddParameter(ParameterName.SellStartDate, SqlDbType.DateTime, 0, ParameterDirection.Input);

        }

        /// <summary>
        /// Maps property values to parameter input values
        /// </summary>
        protected override void SetInputParameterValues()
        {
            this.Parameters[ParameterName.Name].Value = this.Name;
            this.Parameters[ParameterName.ProductNumber].Value = this.ProductNumber;
            this.Parameters[ParameterName.SafetyStockLevel].Value = this.SafetyStockLevel;
            this.Parameters[ParameterName.ReorderPoint].Value = this.ReorderPoint;
            this.Parameters[ParameterName.StandardCost].Value = this.StandardCost;
            this.Parameters[ParameterName.ListPrice].Value = this.ListPrice;
            this.Parameters[ParameterName.DaysToManufacture].Value = this.DaysToManufacture;
            this.Parameters[ParameterName.SellStartDate].Value = this.SellStartDate;
        }

        /// <summary>
        /// Maps property return values to parameter output values
        /// </summary>
        protected override void GetOutputParameterValues()
        {
            this.RETURN_VALUE = (Int32)this.Parameters[ParameterName.RETURN_VALUE].Value;
        }

        /// <summary>
        /// Executes the Stored Procedure
        /// </summary>
        public virtual void Execute()
        {
            this.ExecuteNonQuery();
        }

        /// <summary>
        /// Parameter names used in the stored procedure
        /// </summary>
        public struct ParameterName
        {
            public const string RETURN_VALUE = "@RETURN_VALUE";
            public const string Name = "@Name";
            public const string ProductNumber = "@ProductNumber";
            public const string SafetyStockLevel = "@SafetyStockLevel";
            public const string ReorderPoint = "@ReorderPoint";
            public const string StandardCost = "@StandardCost";
            public const string ListPrice = "@ListPrice";
            public const string DaysToManufacture = "@DaysToManufacture";
            public const string SellStartDate = "@SellStartDate";
        }
    }
}