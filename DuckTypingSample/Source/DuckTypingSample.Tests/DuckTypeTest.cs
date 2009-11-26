using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DuckTypingSample.Tests.Samples;
using DuckTypingSample.Extensions;

namespace DuckTypingSample.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class DuckTypeTest
    {
        private const int TestIterations = 500000;
        private Random _random = new Random();

        public DuckTypeTest() { }

        [TestMethod]
        public void As()
        {
            DateTime expected = new DateTime(_random.Next(1900, 2000), _random.Next(1, 12), _random.Next(1, 28));
            BespokeDateTime bespoke = new BespokeDateTime(expected);
            Assert.IsNotInstanceOfType(bespoke, typeof(IHasValue));
            IHasValue duck = DuckType.As<IHasValue>(bespoke);
            Assert.IsInstanceOfType(duck, typeof(IHasValue));
            Assert.AreEqual(expected, duck.Value);
        }

        [TestMethod]
        public void As_Struct()
        {
            int expected = _random.Next();
            BespokeInt bespoke = new BespokeInt(expected);
            Assert.IsNotInstanceOfType(bespoke, typeof(IHasValue));
            IHasValue duck = DuckType.As<IHasValue>(bespoke);
            Assert.IsInstanceOfType(duck, typeof(IHasValue));
            Assert.AreEqual(expected, duck.Value);
        }

        [TestMethod]
        public void As_Invalid()
        {
            string expected = Guid.NewGuid().ToString();
            BespokeString bespoke = new BespokeString(expected);
            Assert.IsNotInstanceOfType(bespoke, typeof(IHasValue));
            IHasValue duck = DuckType.As<IHasValue>(bespoke);
            Assert.IsNull(duck);
        }

        [TestMethod]
        public void As_Extension()
        {
            DateTime expected = DateTime.Today;
            BespokeDateTime bespoke = new BespokeDateTime(expected);
            Assert.IsNotInstanceOfType(bespoke, typeof(IHasValue));
            DateTime actual = (DateTime) bespoke.As<IHasValue>().Value;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [Ignore] //Manual test
        public void As_Extension_Loop()
        {
            foreach (object obj in GetObjects())
            {
                Assert.IsNotNull(obj.As<IHasValue>().Value);
            }
        }

        [TestMethod]
        [Ignore] //Manual test
        public void As_Loop()
        {
            foreach (object obj in GetObjects())
            {
                Assert.IsNotNull(DuckType.As<IHasValue>(obj).Value);
            }
        }

        [TestMethod]
        [Ignore] //Manual test
        public void Real_Loop()
        {
            foreach (object obj in GetObjects())
            {
                BespokeDateTime bespokeDateTime = obj as BespokeDateTime;
                if (bespokeDateTime != null)
                {
                    Assert.IsNotNull(bespokeDateTime.Value);
                }
                else
                {
                    BespokeInt bespokeInt = (BespokeInt) obj;
                    Assert.IsNotNull(bespokeInt.Value);
                }
            }
        }

        private List<object> GetObjects()
        {
            List<object> objects = new List<object>(TestIterations);
            bool useInt = false;
            for (int i = 0; i < TestIterations; i++)
            {
                if (useInt)
                {
                    objects.Add(new BespokeInt(_random.Next()));
                }
                else
                {
                    objects.Add(new BespokeDateTime(new DateTime(_random.Next(1900, 2000), _random.Next(1, 12), _random.Next(1, 28))));
                }
                useInt = !useInt;
            }
            return objects;
        }
    }
}
