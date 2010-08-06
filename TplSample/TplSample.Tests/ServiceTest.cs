using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TplSample;

namespace PLinqSample.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ServiceTest
    {
        public ServiceTest() { }

        private struct CollectionSize
        {
            public const int Small = 200;
            public const int Large = 3000;
            public const int VeryLarge = 50000;
        }

        [TestMethod]
        public void ProcessSequential_Small()
        {
            Run((w, s) => w.ProcessSequential(s), CollectionSize.Small);
        }

        [TestMethod]
        public void ProcessWithParallelForEach_Small()
        {
            Run((w,s)=>w.ProcessWithParallelForEach(s), CollectionSize.Small);
        }

        [TestMethod]
        public void ProcessWithTasks_Small()
        {
            Run((w, s) => w.ProcessWithTasks(s), CollectionSize.Small);
        }

        [TestMethod]
        public void ProcessSequential_Large()
        {
            Run((w, s) => w.ProcessSequential(s), CollectionSize.Large);
        }

        [TestMethod]
        public void ProcessWithParallelForEach_Large()
        {
            Run((w, s) => w.ProcessWithParallelForEach(s), CollectionSize.Large);
        }

        [TestMethod]
        public void ProcessWithTasks_Large()
        {
            Run((w, s) => w.ProcessWithTasks(s), CollectionSize.Large);
        }

        [TestMethod]
        public void ProcessSequential_VeryLarge()
        {
            Run((w, s) => w.ProcessSequential(s), CollectionSize.VeryLarge);
        }

        [TestMethod]
        public void ProcessWithParallelForEach_VeryLarge()
        {
            Run((w, s) => w.ProcessWithParallelForEach(s), CollectionSize.VeryLarge);
        }

        [TestMethod]
        public void ProcessWithTasks_VeryLarge()
        {
            Run((w, s) => w.ProcessWithTasks(s), CollectionSize.VeryLarge);
        }


        private static void Run(Action<ServiceWrapper, int> action, int collectionSize)
        {
            var wrapper = new ServiceWrapper();
            action(wrapper, collectionSize); 
        }
    }
}
