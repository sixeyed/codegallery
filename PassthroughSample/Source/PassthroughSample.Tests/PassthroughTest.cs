using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PassthroughSample.Tests.Stubs;
using System.Linq.Expressions;

namespace PassthroughSample.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class PassthroughTest
    {
        public PassthroughTest(){}

        [TestMethod]
        public void As()
        {
            var setup = new Passthrough();
            var cache = setup.Create("PassthroughSample.Tests.Stubs.AspNetCache, PassthroughSample.Tests")
                                 .WithPassthrough("Name", "CacheName")
                                 .WithPassthrough("Get", "Retrieve")
                                 .WithPassthrough("Set", "Insert")
                                 .As<ICache>();

            Assert.AreEqual("AspNetCache", cache.Name);
            cache.Name = "NewName";
            Assert.AreEqual("NewName", cache.Name);

            var value = DateTime.Now;
            cache.Set("x", value);
            object value2 = cache.Get("x");
            Assert.AreEqual(value, value2);
        }

        [TestMethod]
        public void As_Generic()
        {
            Expression<Func<ICache, string, object>> get = (o, s) => o.Get(s);
            Expression<Func<Memcached, string, object>> read = (i, s) => i.Read(s);
            Expression<Action<ICache, string, object>> set = (o, s, obj) => o.Set(s, obj);
            Expression<Action<Memcached, string, object>> insert = (i, s, obj) => i.Put(s, obj);

            ICache cache = new Passthrough<ICache, Memcached>()
                            .Create()
                            .WithPassthrough(o => o.Name, i => i.InstanceName)
                            .WithPassthrough(get, read)
                            .WithPassthrough(set, insert)
                            .As();

            Assert.AreEqual("Memcached", cache.Name);
            cache.Name = "NewName";
            Assert.AreEqual("NewName", cache.Name);

            var value = DateTime.Now;
            cache.Set("x", value);
            object value2 = cache.Get("x");
            Assert.AreEqual(value, value2);
        }

        [TestMethod]
        public void GetConfigured()
        {
            ICache cache = Passthrough.GetConfigured<ICache>();

            Assert.AreEqual("AppFabricCache", cache.Name);
            cache.Name = "NewName";
            Assert.AreEqual("NewName", cache.Name);

            var value = DateTime.Now;
            cache.Set("x", value);
            object value2 = cache.Get("x");
            Assert.AreEqual(value, value2);
        }

        [TestMethod]
        public void GetConfigured_Type()
        {
            var dateTime = new DateTime(2001, 1, 1);
            var oldDateTime = Passthrough.GetConfigured<DateTime>(dateTime);
            var oldDateTimeString = oldDateTime.ToString();
            Assert.AreEqual(DateTime.MinValue.ToString(), oldDateTimeString);
        }
    }
}
