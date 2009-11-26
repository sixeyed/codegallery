using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DuckTypingSample.Tests.Samples
{
    public class BespokeDateTime
    {
        private DateTime _value;

        public DateTime Value { get { return _value; } }

        public BespokeDateTime(DateTime value)
        {
            _value = value;
        }
    }
}
