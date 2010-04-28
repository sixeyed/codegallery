using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassthroughSample.Tests.Stubs
{
    public class Memcached
    {
        public Memcached()
        {
            InstanceName = "Memcached";
        }

        public string InstanceName { get; set; }

        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        public object Read(string key)
        {
            return _cache[key];
        }

        public void Put(string key, object value)
        {
            _cache[key] = value;
        }
    }
}
