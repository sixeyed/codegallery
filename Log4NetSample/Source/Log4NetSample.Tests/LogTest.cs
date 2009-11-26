using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Log4NetSample.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class LogTest
    {
        public LogTest() { }

        private const int LogOffCount = 1000000;
        private const int LogOnCount = 5000;
       
        [TestMethod]
        public void Debug()
        {
            for (int i = 0; i < LogOffCount; i++)
            {
                Logger.Log(LogLevel.Debug, 
                    string.Format("Time: {0}, Config setting: {1}", 
                        DateTime.Now.TimeOfDay, 
                        ConfigurationManager.AppSettings["configValue"]));
            }
        }

        [TestMethod]
        public void DebugDelegate()
        {
            for (int i = 0; i < LogOffCount; i++)
            {
                Logger.Log(LogLevel.Debug, 
                    delegate() { return string.Format("Time: {0}, Config setting: {1}", 
                        DateTime.Now.TimeOfDay, 
                        ConfigurationManager.AppSettings["configValue"]); });
            }
        }

        [TestMethod]
        public void DebugLambda()
        {
            for (int i = 0; i < LogOffCount; i++)
            {
                Logger.Log(LogLevel.Debug, 
                    () => string.Format("Time: {0}, Config setting: {1}", 
                        DateTime.Now.TimeOfDay, 
                        ConfigurationManager.AppSettings["configValue"]));
            }
        }

        [TestMethod]
        public void Warn()
        {
            for (int i = 0; i < LogOnCount; i++)
            {
                Logger.Log(LogLevel.Warn, 
                    string.Format("Time: {0}, Config setting: {1}", 
                        DateTime.Now.TimeOfDay, 
                        ConfigurationManager.AppSettings["configValue"]));
            }
        }

        [TestMethod]
        public void WarnDelegate()
        {
            for (int i = 0; i < LogOnCount; i++)
            {
                Logger.Log(LogLevel.Warn, 
                    delegate() { return string.Format("Time: {0}, Config setting: {1}", 
                        DateTime.Now.TimeOfDay, 
                        ConfigurationManager.AppSettings["configValue"]); });
            }
        }

        [TestMethod]
        public void WarnLambda()
        {
            for (int i = 0; i < LogOnCount; i++)
            {
                Logger.Log(LogLevel.Warn, 
                    () => string.Format("Time: {0}, Config setting: {1}",
                        DateTime.Now.TimeOfDay, 
                        ConfigurationManager.AppSettings["configValue"]));
            }
        }
    }
}
