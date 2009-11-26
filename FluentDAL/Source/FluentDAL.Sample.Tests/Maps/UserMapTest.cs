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
    public class UserMapTest
    {
        public UserMapTest() { }

        [TestMethod]
        public void Constructor()
        {
            UserMap map = new UserMap();
        }

        [TestMethod]
        public void Populate()
        {
            string userName = Guid.NewGuid().ToString();
            Guid userId = Guid.NewGuid();
            string line1 = Guid.NewGuid().ToString();
            string postcodeInward = "BS6";
            string postcodeOutward = "6LT";

            IDataReader stubReader = GetStubReader(userName, userId, line1, postcodeInward, postcodeOutward);
            
            User user = new User();
            UserMap map = new UserMap();
            map.Populate(user, stubReader);

            Assert.AreEqual(userName, user.Name);
            Assert.AreEqual(userId, user.Id);
            Assert.IsNotNull(user.Address);
            Assert.AreEqual(line1, user.Address.Line1);
            Assert.IsNotNull(user.Address.PostCode);
            Assert.AreEqual("BS6 6LT", user.Address.PostCode.Value);
        }

        [TestMethod]
        public void Create()
        {
            string userName = Guid.NewGuid().ToString();
            Guid userId = Guid.NewGuid();
            string line1 = Guid.NewGuid().ToString();
            string postcodeInward = "PL21";
            string postcodeOutward = "9TY";

            IDataReader stubReader = GetStubReader(userName, userId, line1, postcodeInward, postcodeOutward);

            UserMap map = new UserMap();
            User user = map.Create(stubReader);

            Assert.AreEqual(userName, user.Name);
            Assert.AreEqual(userId, user.Id);
            Assert.IsNotNull(user.Address);
            Assert.AreEqual(line1, user.Address.Line1);
            Assert.IsNotNull(user.Address.PostCode);
            Assert.AreEqual("PL21 9TY", user.Address.PostCode.Value);
        }

        private static IDataReader GetStubReader(string userName, Guid userId, string line1, string postcodeInward, string postcodeOutward)
        {
            IDataReader stubReader = MockRepository.GenerateStub<IDataReader>();
            stubReader.Stub(x => x.FieldCount).Return(5);
            stubReader.Stub(x => x.GetName(0)).Return("UserId");
            stubReader.Stub(x => x.GetName(1)).Return("UserName");
            stubReader.Stub(x => x.GetName(2)).Return("AddressLine1");
            stubReader.Stub(x => x.GetName(3)).Return("PS_IN");
            stubReader.Stub(x => x.GetName(4)).Return("PS_OUT");
            stubReader.Stub(x => x.GetValue(0)).Return(userId);
            stubReader.Stub(x => x.GetValue(1)).Return(userName);
            stubReader.Stub(x => x.GetValue(2)).Return(line1);
            stubReader.Stub(x => x.GetValue(3)).Return(postcodeInward);
            stubReader.Stub(x => x.GetValue(4)).Return(postcodeOutward);
            return stubReader;
        }
    }
}
