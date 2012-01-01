using System;
using DistributedServiceSample.Core.Extensions;


namespace DistributedServiceSample.Core.Logging
{
    public static class Log
    {

        public static void Trace(string messageFormat, params object[] args)
        {
            System.Diagnostics.Trace.WriteLine(messageFormat.FormatWith(args));
        }       
    }
}
