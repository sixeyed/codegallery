using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;

namespace DynamicSP.Core.Logging
{
    /// <summary>
    /// Centralized logging class, logs to configured log4net sink
    /// </summary>
    public static class Log
    {
        #region Private static fields

        private static ILog _logger;
        private static object _syncLock = new object();

        #endregion

        #region Private static properties

        private static ILog Logger
        {
            get
            {
                if (_logger == null)
                {
                    lock (_syncLock)
                    {
                        //use double-checked locking:
                        if (_logger == null)
                        {
                            XmlConfigurator.Configure();
                            _logger = LogManager.GetLogger("DynamicSP.Log");
                        }
                    }
                }
                return _logger;
            }
        }
        #endregion

        /// <summary>
        /// Logs debug message
        /// </summary>
        /// <param name="format">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Debug(string format, params object[] args)
        {
            Logger.DebugFormat(format, args);
        }

        /// <summary>
        /// Logs informational message
        /// </summary>
        /// <param name="format">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Info(string format, params object[] args)
        {
            Logger.InfoFormat(format, args);
        }

        /// <summary>
        /// Logs warning message
        /// </summary>
        /// <param name="format">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Warn(string format, params object[] args)
        {
            Logger.WarnFormat(format, args);
        }

        /// <summary>
        /// Logs error message
        /// </summary>
        /// <param name="format">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Error(string format, params object[] args)
        {
            Logger.ErrorFormat(format, args);
        }

        /// <summary>
        /// Logs error message
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="format">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Error(Exception ex, string format, params object[] args)
        {
            string message = string.Format(format, args);
            Logger.ErrorFormat("{0}. Exception: {1}", message, ex);
        }

        /// <summary>
        /// Logs error message
        /// </summary>
        /// <param name="ex">Exception</param>
        public static void Error(Exception ex)
        {
            Logger.Error(ex.Message);
        }

        /// <summary>
        /// Log fatal message
        /// </summary>
        /// <param name="format">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Fatal(string format, object[] args)
        {
            Logger.FatalFormat(format, args);
        }
    }
}
