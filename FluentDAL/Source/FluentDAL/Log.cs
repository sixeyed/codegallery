using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace FluentDAL
{
    /// <summary>
    /// Simple log implementation, writes to Trace output
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Writes a warning message to the log
        /// </summary>
        /// <param name="messageFormat">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Warn(string messageFormat, params object[] args) 
        { 
            Trace.WriteLine("FluentDAL.WARN: " + string.Format(messageFormat, args));
        }

        /// <summary>
        /// Writes an error message to the log
        /// </summary>
        /// <param name="messageFormat">Message format</param>
        /// <param name="args">Message arguments</param>
        public static void Error(string messageFormat, params object[] args) 
        {
            Trace.WriteLine("FluentDAL.ERROR: " + string.Format(messageFormat, args));
        }
    }
}
