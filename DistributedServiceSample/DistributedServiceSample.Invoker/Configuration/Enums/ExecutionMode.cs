using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedServiceSample.Invoker.Configuration
{
    public enum ExecutionMode
    {
        Synchronous, 
        
        AsynchronousIgnoreResponse
    }
}
