using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckTypingSample.Tests.Samples
{
    public struct BespokeInt
    {
        private int _value;

        public int Value { get { return _value; } }

        public BespokeInt(int value)
        {
            _value = value;
        }
    }
}
