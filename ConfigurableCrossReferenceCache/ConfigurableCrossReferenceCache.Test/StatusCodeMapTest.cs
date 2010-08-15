using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConfigurableCrossReferenceCache.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class StatusCodeMapTest
    {
        public StatusCodeMapTest() { }

        public const int LookupMilliseconds = 500;
        public const int CacheLifespanMilliseconds = 2000;

        [TestMethod]
        public void LookupIsCached()
        {
            var inputCode = Guid.NewGuid().ToString();
            var lookup = new StatusCodeMap();
            var outputCode = lookup.GetStatusCode(inputCode);
            var stopwatch = Stopwatch.StartNew();
            var outputCode2 = lookup.GetStatusCode(inputCode);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < LookupMilliseconds);
        }

        [TestMethod]
        public void DifferentInstancesShareCache()
        {
            var inputCode = Guid.NewGuid().ToString();
            var lookup1 = new StatusCodeMap();
            var outputCode = lookup1.GetStatusCode(inputCode);
            var lookup2 = new StatusCodeMap();
            var stopwatch = Stopwatch.StartNew();
            var outputCode2 = lookup2.GetStatusCode(inputCode);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds < CacheLifespanMilliseconds);
        }

        [TestMethod]
        public void CacheExpires()
        {
            var inputCode = Guid.NewGuid().ToString();
            var lookup = new StatusCodeMap();
            var outputCode = lookup.GetStatusCode(inputCode);
            Thread.Sleep(CacheLifespanMilliseconds);
            var stopwatch = Stopwatch.StartNew();
            var outputCode2 = lookup.GetStatusCode(inputCode);
            Thread.Sleep(1);
            Assert.IsTrue(stopwatch.ElapsedMilliseconds > LookupMilliseconds);
        }
    }
}
