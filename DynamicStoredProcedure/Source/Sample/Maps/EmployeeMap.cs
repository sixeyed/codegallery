using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DynamicSP.Data.Mapping;
using DynamicSP.Sample.Entities;

namespace DynamicSP.Sample.Data.Maps
{
    /// <summary>
    /// Maps a <see cref="Employee"/> domain object from a populated data reader
    /// </summary>
    public class EmployeeMap : DataReaderMap<Employee>
    {
        /// <summary>
        /// Default constructor, initialises mapping
        /// </summary>
        public EmployeeMap()
        {
            Map(x => x.Id, ColumnName.Id);
            Map(x => x.FirstName, ColumnName.FirstName);
            Map(x => x.LastName, ColumnName.LastName);
            Map(x => x.ManagerId, ColumnName.ManagerID);
            Map(x => x.ManagerFirstName, ColumnName.ManagerFirstName);
            Map(x => x.ManagerLastName, ColumnName.ManagerLastName);
        }

        private struct ColumnName
        {
            public const string Id = "EmployeeID";
            public const string FirstName = "FirstName";
            public const string LastName = "LastName";
            public const string ManagerID = "ManagerID";
            public const string ManagerFirstName = "ManagerFirstName";
            public const string ManagerLastName = "ManagerLastName";
        }
    }
}
