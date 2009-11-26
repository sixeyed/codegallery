using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentDAL.Sample.Maps.Conversion
{
    public static class Legacy
    {
        public static bool FromBoolean(string legacyBoolean)
        {
            return (legacyBoolean.Trim().ToUpper() == "Y");
        }
    }
}
