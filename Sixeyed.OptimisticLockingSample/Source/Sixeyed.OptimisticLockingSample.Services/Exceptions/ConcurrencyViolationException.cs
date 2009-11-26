using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sixeyed.OptimisticLockingSample.Services.Exceptions
{
    public class ConcurrencyViolationException : Exception
    {
        public ConcurrencyViolationException(object serializableobject) : 
            base(string.Format("Concurrency validation - entity has been modified since original retreival. Update failed. Entity: {0}",
                    serializableobject)) {}
    }
}
