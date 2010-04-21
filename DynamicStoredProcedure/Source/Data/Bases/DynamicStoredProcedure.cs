using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Data;

namespace DynamicSP.Data.Bases
{
    public abstract class DynamicStoredProcedure : DynamicObject, IDisposable
    {
        public abstract IDataReader Execute();

        public abstract void Dispose();
    }
}
