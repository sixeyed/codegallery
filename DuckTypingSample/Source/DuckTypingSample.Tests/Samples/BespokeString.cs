using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckTypingSample.Tests.Samples
{
    public class BespokeString
    {
        private string _value;

        public string StringValue { get { return _value; } }

        public BespokeString(string value)
        {
            _value = value;
        }
    }
}
