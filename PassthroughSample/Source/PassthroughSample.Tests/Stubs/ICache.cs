using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassthroughSample.Tests.Stubs
{
    public interface ICache
    {
        string Name { get; set; }

        object Get(string key);

        void Set(string key, object value);
    }
}
