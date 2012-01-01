using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedServiceSample.Contracts.Entities;
using System.ServiceModel;

namespace DistributedServiceSample.Contracts.Services
{
    [ServiceContract(Namespace = TargetNamespace.DistributedServiceSample)]
    public interface IComputeService : IService
    {
        [OperationContract]
        ComputeResult Compute(int jobId);
    }
}
