using System.Diagnostics;

namespace ConfigurableCrossReferenceCache.Logging
{
    public static class Log
    {
        public static void Debug(string format, params object[] args)
        {
            WriteLog("DEBUG", format, args);
        }

        public static void Error(string format, params object[] args)
        {
            WriteLog("ERRO", format, args);
        }

        public static void Info(string format, params object[] args)
        {
            WriteLog("INFO", format, args);
        }

        public static void Warn(string format, params object[] args)
        {
            WriteLog("WARN", format, args);
        }

        private static void WriteLog(string category, string format, params object[] args)
        {
            Trace.WriteLine(string.Format(format, args), category);
        }
    }
}
