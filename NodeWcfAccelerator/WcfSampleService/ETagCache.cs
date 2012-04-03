using System;
using System.ServiceModel;
using Enyim.Caching;
using Enyim.Caching.Memcached;

namespace WcfSampleService
{
    /// <summary>
    /// Cache current entity tags against the URL to fetch them. Uses <see cref="MemcachedClient"/> so
    /// cache entries can be read by Node.js (or anything else)
    /// </summary>
    public static class ETagCache
    {
        private static MemcachedClient _Cache = new MemcachedClient();

        public static bool ExistsForCurrent()
        {
            return GetForCurrent() != Guid.Empty;
        }

        public static Guid GetForCurrent()
        {
            var eTag = Guid.Empty;
            var key = GetCacheKey();
            if (key != null)
            {
                var item = _Cache.Get(key);
                if (item != null)
                {
                    eTag = Unwrap(item.ToString());
                }
            }
            return eTag;
        }

        public static void SetForCurrent(Guid eTag)
        {
            var key = GetCacheKey();
            if (key != null)
            {
                Set(eTag, key);
            }
        }

        public static void Replace(Guid oldETag, Guid newETag)
        {
            //using the old etag, get the URL for the active etag:
            var oldValue = Wrap(oldETag);
            var key = _Cache.Get(oldValue) as string;
            if (!string.IsNullOrEmpty(key))
            {
                Set(newETag, key);
                _Cache.Remove(oldValue);
            }
        }

        private static void Set(Guid eTag, string key)
        {
            //wrap the guid in double-quotes so the ETag is stored in client header format:
            var value = Wrap(eTag);
            _Cache.Store(StoreMode.Set, key, value);
            //store the URL against the ETag so we can do a reverse-lookup for updates:
            _Cache.Store(StoreMode.Set, value, key);
        }

        private static string GetCacheKey()
        {
            string key = null;
            if (OperationContext.Current != null && OperationContext.Current.RequestContext != null)
            {
                key = OperationContext.Current.RequestContext.RequestMessage.Headers.To.PathAndQuery;
            }
            return key;
        }

        private static string Wrap(Guid eTag)
        {
            return string.Format("\"{0}\"", eTag);
        }

        private static Guid Unwrap(string eTag)
        {
            return Guid.Parse(eTag.ToString().Replace("\"", ""));
        }
    }
}