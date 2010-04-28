using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PassthroughSample.Tests.Stubs
{
    public class OldDateTime
    {
        public string ToOldString()
        {
            return DateTime.MinValue.ToString();
        }
    }
}
