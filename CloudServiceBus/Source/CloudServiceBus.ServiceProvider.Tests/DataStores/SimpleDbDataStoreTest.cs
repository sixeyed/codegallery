using System;
using System.Collections.Generic;
using System.Linq;
using CloudServiceBus.Core;
using CloudServiceBus.Core.DataStores;
using CloudServiceBus.Core.Serialization;
using CloudServiceBus.Entities;
using CloudServiceBus.Entities.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudServiceBus.ServiceProvider.Tests
{
    [TestClass]
    public class SimpleDbDataStoreTest
    {
        private string _storeIdentifier = "Sixeyed-CloudServiceBus-ResponseData";
        private string _itemKey = "140ed91a-a794-44c4-a270-41ee725b7e68";

        [TestMethod]
        public void Add()
        {
            var employees = GetEmployees(DateTime.Now.AddHours(-1));
            var serializer =  new JsonSerializer();
            var db = new SimpleDBDataStore();
            var serializedEmployees = new List<string>(employees.Count());
            foreach (var employee in employees)
            {
                var serializedEmployee = serializer.Serialize(typeof(Employee), employee).ToString();
                serializedEmployees.Add(serializedEmployee);                
            }
            db.Add(_storeIdentifier, _itemKey, serializedEmployees.ToArray());
        }

        [TestMethod]
        public void Fetch()
        {
            var serializer = new JsonSerializer();
            var db = new SimpleDBDataStore();
            var responseItems = db.Fetch(_storeIdentifier, _itemKey);
            var employees = new List<Employee>(responseItems.Count());
            foreach (var responseItem in responseItems)
            {
                var employee = serializer.Deserialize(typeof(Employee), responseItem) as Employee;
                Assert.IsNotNull(employee);
                employees.Add(employee);
            }
        }

        [TestMethod]
        public void Flush()
        {
            var db = new SimpleDBDataStore();
            db.Flush(_storeIdentifier);
        }

        private IEnumerable<Employee> GetEmployees(DateTime dateTime)
        {
            //stub implementation only:
            var resultCount = RandomValueGenerator.GetRandomInt(1, 100);
            var employees = new List<Employee>(resultCount);
            for (int index = 0; index < resultCount; index++)
            {
                var employee = new Employee();
                employee.DepartmentName = RandomValueGenerator.GetRandomString();
                employee.DirectDialNumber = RandomValueGenerator.GetRandomInt(1000000000).ToString();
                employee.EmailAddress = string.Format("{0}@sixeyed.com", RandomValueGenerator.GetRandomString(10, " "));
                employee.EmployeeId = RandomValueGenerator.GetRandomInt().ToString();
                employee.Status = RandomValueGenerator.GetRandomEnumValue<EmployeeStatus>();
                employee.ExtensionNumber = RandomValueGenerator.GetRandomInt(1000).ToString();
                employee.FaxNumber = RandomValueGenerator.GetRandomInt(1000000000).ToString();
                employee.FullName = RandomValueGenerator.GetRandomString(20);
                employee.LatestHireDate = RandomValueGenerator.GetRandomDateTime(2000);
                employee.LastChangeDate = RandomValueGenerator.GetRandomDateTime(dateTime.Year - 1);
                employee.MobileNumber = RandomValueGenerator.GetRandomInt(1000000000).ToString();
                employee.OfficeCountry = RandomValueGenerator.GetRandomString(15);
                employee.OfficeLocation = RandomValueGenerator.GetRandomString(30);
                employee.RoleDescription = RandomValueGenerator.GetRandomString(30);
                employee.RoleTitle = RandomValueGenerator.GetRandomString(20);
                employees.Add(employee);
            }
            return employees;
        }
    }
}
