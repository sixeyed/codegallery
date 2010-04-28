using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassthroughSample.Tests.Stubs
{
    public class AppFabricCache
    {
        public AppFabricCache()
        {
            RegionName = "AppFabricCache";
        }

        public string RegionName { get; set; }


        private Dictionary<string, object> _cache = new Dictionary<string, object>();

        public object Out(string key)
        {
            return _cache[key];
        }

        public void In(string key, object value)
        {
            _cache[key] = value;
        }
    }
}
