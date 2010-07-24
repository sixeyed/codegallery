using Amazon.SQS.Model;

namespace CloudServiceBus.ServiceProvider.ServiceHandlers
{
    /// <summary>
    /// Represents a service handler
    /// </summary>
    public interface IServiceHandler
    {
        string QueueName { get; }

        string QueueUrl { get; set; }

        bool IsRequestHandled(Message request);

        void HandleRequest(Message request);
    }
}
