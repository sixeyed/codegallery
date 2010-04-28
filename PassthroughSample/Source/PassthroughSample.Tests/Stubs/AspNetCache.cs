using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassthroughSample.Tests.Stubs
{
    public class AspNetCache
    {
        public AspNetCache()
        {
            CacheName = "AspNetCache";
        }

        public string CacheName { get; set; }


        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        public object Retrieve(string key)
        {
            return _cache[key];
        }

        public void Insert(string key, object value)
        {
            _cache[key] = value;
        }
    }
}
