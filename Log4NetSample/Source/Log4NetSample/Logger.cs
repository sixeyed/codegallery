using System;
using log4net;
using log4net.Config;

namespace Log4NetSample
{
    public static class Logger
    {
        private static ILog _logger;
        private static object syncLock = new object();

        private static ILog LoggerInternal
        {
            get
            {
                if (_logger == null)
                {
                    lock (syncLock)
                    {
                        //use double-checked locking:
                        if (_logger == null)
                        {
                            XmlConfigurator.Configure();
                            _logger = LogManager.GetLogger("Log4NetSample.Log");
                        }
                    }
                }
                return _logger;
            }
        }

        /// <summary>
        /// Logs the given message if configured logging level is active 
        /// </summary>
        /// <param name="level">Logging level</param>
        /// <param name="message">Log message</param>
        public static void Log(LogLevel level, string message)
        {
            if (IsLogLevelEnabled(level))
            {
                LogInternal(level, message);
            }
        }

        /// <summary>
        /// Evaluates and logs the given message if configured logging level is active 
        /// </summary>
        /// <param name="level">Logging level</param>
        /// <param name="fMessage">Function providing log message</param>
        public static void Log(LogLevel level, Func<string> fMessage)
        {
            if (IsLogLevelEnabled(level))
            {
                LogInternal(level, fMessage.Invoke());
            }
        }

        private static bool IsLogLevelEnabled(LogLevel level)
        {
            bool enabled = false;
            switch (level)
            {
                case LogLevel.Debug:
                    enabled = LoggerInternal.IsDebugEnabled;
                    break;
                case LogLevel.Error:
                    enabled = LoggerInternal.IsErrorEnabled;
                    break;
                case LogLevel.Fatal:
                    enabled = LoggerInternal.IsFatalEnabled;
                    break;
                case LogLevel.Info:
                    enabled = LoggerInternal.IsInfoEnabled;
                    break;
                case LogLevel.Warn:
                    enabled = LoggerInternal.IsWarnEnabled;
                    break;
            }
            return enabled;
        }

        private static void LogInternal(LogLevel level, string message)
        {
            switch (level)
            {
                case LogLevel.Debug:
                    LoggerInternal.Debug(message);
                    break;
                case LogLevel.Error:
                    LoggerInternal.Error(message);
                    break;
                case LogLevel.Fatal:
                    LoggerInternal.Fatal(message);
                    break;
                case LogLevel.Info:
                    LoggerInternal.Info(message);
                    break;
                case LogLevel.Warn:
                    LoggerInternal.Warn(message);
                    break;
            }
        }       
    }
}
