using System;
using Amazon.SQS.Model;
using CloudServiceBus.Core.Aws;
using CloudServiceBus.Core.Extensions;
using CloudServiceBus.Entities.ServiceRequests;
using CloudServiceBus.Entities.ServiceResponses;

namespace CloudServiceBus.ServiceProvider.ServiceHandlers
{
    /// <summary>
    /// Base class for service handlers
    /// </summary>
    /// <remarks>
    /// Handles extracting the service request object from the incoming message,
    /// and sending the outgoing message response
    /// </remarks>
    /// <typeparam name="TRequest">Type of request handled</typeparam>
    /// <typeparam name="TResponse">Type of response sent</typeparam>
    public abstract class ServiceHandlerBase<TRequest, TResponse> : IServiceHandler
        where TRequest : ServiceRequest
        where TResponse : ServiceResponse
    {
        public abstract void HandleRequest(Message request);
        
        protected TRequest ServiceRequest { get; set; }

        public virtual string QueueName
        {
            get { return "Sixeyed-CloudServiceBus-RequestQueue";  }
        }

        public string QueueUrl { get; set; }

        public virtual bool IsRequestHandled(Message request)
        {
            ServiceRequest = request.BodyAsRequest<TRequest>();
            return ServiceRequest != null;
        }

        public void SendResponse(TResponse response)
        {
            var sqsClient = AwsFacade.GetSqsClient();
            var request = new SendMessageRequest();
            request.QueueUrl = ServiceRequest.ResponseQueueUrl;
            request.ResponseToBody<TResponse>(response);

            Console.WriteLine("*");
            Console.WriteLine("Sending response message: {0} to URL: {1}", request.MessageBody, ServiceRequest.ResponseQueueUrl);
            Console.WriteLine("*");

            sqsClient.SendMessage(request);
        }
    }
}
