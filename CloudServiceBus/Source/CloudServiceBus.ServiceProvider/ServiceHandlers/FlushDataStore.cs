using System;
using Amazon.SQS.Model;
using CloudServiceBus.Core.DataStores;
using CloudServiceBus.Entities.ServiceRequests.Operations;
using CloudServiceBus.Entities.ServiceResponses;

namespace CloudServiceBus.ServiceProvider.ServiceHandlers
{
    /// <summary>
    /// Sample command service handler - flushes the
    /// data in the current data store
    /// </summary>
    public class FlushDataStore : ServiceHandlerBase<FlushDataStoreRequest, ServiceResponse>
    {
        public override void HandleRequest(Message request)
        {
            if (ServiceRequest == null)
                return;

            Console.WriteLine("FlushDataStore - Request received, StoreIdentifier: {0}", ServiceRequest.StoreIdentifier);
            DataStore.Current.Flush(ServiceRequest.StoreIdentifier);
            Console.WriteLine("FlushDataStore - Request completed", ServiceRequest.StoreIdentifier);
        }
    }
}
