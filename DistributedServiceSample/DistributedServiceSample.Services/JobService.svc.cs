using System;
using DistributedServiceSample.Contracts.Entities;
using DistributedServiceSample.Contracts.Services;
using DistributedServiceSample.Core.Logging;
using DistributedServiceSample.Invoker;

namespace DistributedServiceSample.Services
{
    public class JobService : IJobService
    {
        private static Random _Random = new Random();

        public int CreateJob(Job job)
        {
            Log.Trace("JobService.CreateJob called with Name: {0}", job.Name);
            var jobId = _Random.Next();
            var saved = Service.Execute<IJobService, bool>(svc => svc.SaveJob(job));
            if (saved)
            {
                Service.Execute<IComputeService>(svc => svc.Compute(jobId));
            }
            return jobId;
        }

        public bool SaveJob(Job job)
        {
            Log.Trace("JobService.SaveJob called with jobId: {0}", job.Name);
            return true;
        }
    }
}
