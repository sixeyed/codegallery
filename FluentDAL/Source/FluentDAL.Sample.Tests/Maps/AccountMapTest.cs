using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data;
using Rhino.Mocks;
using FluentDAL.Sample.Maps;
using FluentDAL.Sample.Entities;

namespace FluentDAL.Tests
{
    /// <summary>
    /// Summary description for UserMapTest
    /// </summary>
    [TestClass]
    public class AccountMapTest
    {
        public AccountMapTest() { }

        [TestMethod]
        public void Constructor()
        {
            AccountMap map = new AccountMap();
        }

        [TestMethod]
        public void Populate_Unactivated()
        {
            Guid id = Guid.NewGuid();
            string name = Guid.NewGuid().ToString();
            string activated = "N";

            IDataReader stubReader = GetStubReader(id, name, activated);

            AccountMap map = new AccountMap();
            Account account = map.Create(stubReader);

            Assert.AreEqual(id, account.Id);
            Assert.AreEqual(name, account.Name);
            Assert.IsFalse(account.Activated);
        }

        [TestMethod]
        public void Create_Activated()
        {
            Guid id = Guid.NewGuid();
            string name = Guid.NewGuid().ToString();
            string activated = "Y";

            IDataReader stubReader = GetStubReader(id, name, activated);

            AccountMap map = new AccountMap();
            Account account = map.Create(stubReader);

            Assert.AreEqual(id, account.Id);
            Assert.AreEqual(name, account.Name);
            Assert.IsTrue(account.Activated);
        }

        private static IDataReader GetStubReader(Guid id, string name, string activated)
        {
            IDataReader stubReader = MockRepository.GenerateStub<IDataReader>();
            stubReader.Stub(x => x.FieldCount).Return(3);
            stubReader.Stub(x => x.GetName(0)).Return("AccountID");
            stubReader.Stub(x => x.GetName(1)).Return("AccountName");
            stubReader.Stub(x => x.GetName(2)).Return("AccountActivated");
            stubReader.Stub(x => x.GetValue(0)).Return(id);
            stubReader.Stub(x => x.GetValue(1)).Return(name);
            stubReader.Stub(x => x.GetValue(2)).Return(activated);
            return stubReader;
        }
    }
}
