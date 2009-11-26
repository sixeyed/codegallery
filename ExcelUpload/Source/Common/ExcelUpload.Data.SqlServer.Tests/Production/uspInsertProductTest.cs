using ExcelUpload.Data.SqlServer.StoredProcedures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
namespace ExcelUpload.Data.SqlServer.Tests
{
    
    
    /// <summary>
    ///This is a test class for uspInsertProductTest and is intended
    ///to contain all uspInsertProductTest Unit Tests
    ///</summary>
    [TestClass()]
    public class uspInsertProductTest
    {
        /// <summary>
        /// Manual test
        /// </summary>
        [Ignore]
        [TestMethod()]
        public void Execute()
        {
            using (uspInsertProduct target = new uspInsertProduct())
            {
                target.DaysToManufacture = 10;
                target.ListPrice = 100;
                target.Name = "np1";
                target.ProductNumber = "np1";
                target.ReorderPoint = 10;
                target.SafetyStockLevel = 20;
                target.SellStartDate = new DateTime(2005, 01, 01);
                target.StandardCost = 85;
                target.Execute();
            }
        }
    }
}
