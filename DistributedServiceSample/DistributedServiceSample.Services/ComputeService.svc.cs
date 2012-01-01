using DistributedServiceSample.Contracts.Entities;
using DistributedServiceSample.Contracts.Services;
using DistributedServiceSample.Core.Logging;

namespace DistributedServiceSample.Services
{
    public class ComputeService : IComputeService
    {
        public ComputeResult Compute(int jobId)
        {
            Log.Trace("ComputeService.Compute called with jobId: {0}", jobId);
            return new ComputeResult();
        }
    }
}
