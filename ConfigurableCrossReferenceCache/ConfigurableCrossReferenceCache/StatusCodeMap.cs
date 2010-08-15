using System;
using System.Collections.Generic;
using System.Threading;
using ConfigurableCrossReferenceCache.Configuration;
using ConfigurableCrossReferenceCache.Logging;

namespace ConfigurableCrossReferenceCache
{
    public class StatusCodeMap
    {
        private static object _syncLock = new object();
        private static Dictionary<string, string> _statusCodes = new Dictionary<string, string>();

        private static object _lastCacheLoadSyncLock = new object();
        private static DateTime _lastCacheLoad;

        static StatusCodeMap()
        {
            ResetCache();
        }

        public string GetStatusCode(string inputCode)
        {
            try
            {
                Log.Debug("GetStatusCode called with inputCode: {0}", inputCode);
                EnsureStatusCode(inputCode);
                return _statusCodes[inputCode];
            }
            catch (Exception ex)
            {
                Log.Error("StatusCodeMap.GetStatusCode exception: {0}", ex.Message);
            }
            return inputCode;
        }

        private static void EnsureStatusCode(string inputCode)
        {
            FlushCacheIfExpired();
            if (!_statusCodes.ContainsKey(inputCode))
            {
                Log.Debug("Status code: {0} not found in lookup", inputCode);
                string outputCode = GetLookupValue(inputCode);
                lock (_syncLock)
                {
                    _statusCodes[inputCode] = outputCode;
                }
            }
            else
            {
                Log.Debug("Status code: {0} found in lookup", inputCode);
            }
        }

        private static void FlushCacheIfExpired()
        {
        TimeSpan lifespan = MapConfiguration.Current.Caching.GetLifespan(CacheConfigKey.StatusCodeLookup);
        DateTime expiry = _lastCacheLoad.Add(lifespan);
            if (DateTime.Now > expiry)
            {
                Log.Debug("{0} cache expired at: {1}", CacheConfigKey.StatusCodeLookup, expiry);
                ResetCache();
            }
        }

        private static void ResetCache()
        {
            lock (_syncLock)
            {
                _statusCodes = new Dictionary<string, string>();
            }
            lock (_lastCacheLoadSyncLock)
            {
                _lastCacheLoad = DateTime.Now;
            }
            Log.Debug("{0} cache reset, lifespan is: {1}", CacheConfigKey.StatusCodeLookup,
                    MapConfiguration.Current.Caching.GetLifespanDuration(CacheConfigKey.StatusCodeLookup));
        }

        private struct CacheConfigKey
        {
            public const string StatusCodeLookup = "StatusCodeLookup";
        }

        private static string GetLookupValue(string inputCode)
        {
            //stub method - would go to the source data here
            Thread.Sleep(500);
            return string.Format("_{0}", inputCode);
        }
    }
}
