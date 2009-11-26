using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using ExcelUpload.Data.Bases;

namespace ExcelUpload.Data.SqlServer.Transactions
{
    /// <summary>
    /// A transaction over the AdventureWorks database
    /// </summary>
    public class AdventureWorksTransaction : TransactionBase<SqlConnection>
    {
        /// <summary>
        /// Returns the name of the AdventureWorks database
        /// </summary>
        public override string DatabaseName
        {
            get { return Database.AdventureWorks; }
        }
    }
}
