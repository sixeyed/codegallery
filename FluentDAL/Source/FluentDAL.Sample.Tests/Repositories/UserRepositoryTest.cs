using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentDAL.Sample.Entities;
using FluentDAL.Sample.Repositories;

namespace FluentDAL.Tests.Repositories
{
    /// <summary>
    /// Summary description for UserRepositoryTest
    /// </summary>
    [TestClass]
    public class UserRepositoryTest
    {
        public UserRepositoryTest() { }

        [TestMethod]
        public void GetUser()
        {
            User user = new UserRepository().GetUser(Guid.NewGuid());
            Assert.IsNotNull(user);
            Assert.IsNotNull(user.Accounts);
            Assert.IsTrue(user.Accounts.Count == 3);
            //all accounts should have name populated:
            foreach (Account account in user.Accounts)
            {
                Assert.IsNotNull(account.Id);
                Assert.IsFalse(string.IsNullOrEmpty(account.Name));
            }
            //only "Account 3" should be activated:
            var activatedAccount = (from ac in user.Accounts
                                   where ac.Activated == true
                                   select ac).FirstOrDefault();
            Assert.IsNotNull(activatedAccount);
            Assert.AreEqual("Account 3", activatedAccount.Name);
        }
    }
}
