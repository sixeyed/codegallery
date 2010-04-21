using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DynamicSP.Data.SqlServer.StoredProcedures;
using DynamicSP.Data.Mapping;
using DynamicSP.Sample.Entities;
using DynamicSP.Sample.Data.Maps;
using DynamicSP.Sample.Data.SqlServer.StoredProcedures;
using System.Xml.Linq;
using Microsoft.CSharp.RuntimeBinder;

namespace DynamicSP.Sample.Data.SqlServer.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class uspGetManagerEmployeesTest
    {
        private const int ITERATIONS = 1000;

        public uspGetManagerEmployeesTest() { }

        [TestMethod]
        public void Dynamic()
        {
            dynamic getUser = new DynamicSqlStoredProcedure("uspGetManagerEmployees", Database.AdventureWorks);
            getUser.ManagerID = 16;

            var employees = Fluently.Load<List<Employee>>()
                                    .With<EmployeeMap>()
                                    .From(getUser);

            Assert.IsNotNull(employees);
            Assert.AreEqual(12, employees.Count);
            Assert.AreEqual(0, getUser.RETURN_VALUE);
        }

        [TestMethod]
        public void DynamicLoop()
        {
            for (int index = 0; index < ITERATIONS; index++)
            {
                dynamic getUser = new DynamicSqlStoredProcedure("uspGetManagerEmployees", Database.AdventureWorks);
                getUser.ManagerID = 16;

                var employees = Fluently.Load<List<Employee>>()
                                        .With<EmployeeMap>()
                                        .From(getUser);

                Assert.IsNotNull(employees);
                Assert.AreEqual(12, employees.Count);
                Assert.AreEqual(0, getUser.RETURN_VALUE);
            }
        }

        [TestMethod]
        public void DynamicLoop_Reused()
        {
            dynamic getUser = new DynamicSqlStoredProcedure("uspGetManagerEmployees", Database.AdventureWorks);
            getUser.ManagerID = 16;

            for (int index = 0; index < ITERATIONS; index++)
            {
                var employees = Fluently.Load<List<Employee>>()
                                        .With<EmployeeMap>()
                                        .From(getUser);

                Assert.IsNotNull(employees);
                Assert.AreEqual(12, employees.Count);
                Assert.AreEqual(0, getUser.RETURN_VALUE);
            }
        }

        [TestMethod]
        public void Static()
        {
            var employees = Fluently.Load<List<Employee>>()
                                    .With<EmployeeMap>()
                                    .From<uspGetManagerEmployees>(i => i.ManagerID = 16, x => x.Execute());

            Assert.IsNotNull(employees);
            Assert.AreEqual(12, employees.Count);
        }

        [TestMethod]
        public void StaticLoop()
        {
            for (int index = 0; index < ITERATIONS; index++)
            {
                var employees = Fluently.Load<List<Employee>>()
                                        .With<EmployeeMap>()
                                        .From<uspGetManagerEmployees>(i => i.ManagerID = 16, x => x.Execute());

                Assert.IsNotNull(employees);
                Assert.AreEqual(12, employees.Count);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(RuntimeBinderException))]
        public void Dynamic_Invalid()
        {
            dynamic getUser = new DynamicSqlStoredProcedure("uspGetManagerEmployees", Database.AdventureWorks);
            getUser.ManagerID = 16;
            getUser.InvalidProperty = new XElement("invalid");
        }
    }
}
