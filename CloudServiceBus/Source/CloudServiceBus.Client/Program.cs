using System;
using System.Configuration;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudServiceBus.Client.Listeners;
using CloudServiceBus.Core.Aws;
using CloudServiceBus.Core.Extensions;
using CloudServiceBus.Entities.ServiceRequests;
using CloudServiceBus.Entities.ServiceRequests.Operations;

namespace CloudServiceBus.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "CloudServiceBus: Client";

            AmazonSQS sqs = AwsFacade.GetSqsClient(); 
            var requestQueueUrl = ConfigurationManager.AppSettings["QueueUrl"];            

            //create a queue for responses:
            var queueName = Guid.NewGuid().ToString();
            var createQueueRequest = new CreateQueueRequest();
            createQueueRequest.QueueName = queueName;
            var createQueueResponse = sqs.CreateQueue(createQueueRequest);
            var responseQueueUrl = createQueueResponse.CreateQueueResult.QueueUrl;

            var listener = new MessageListener();
            ThreadPool.QueueUserWorkItem(new WaitCallback(listener.StartListening), responseQueueUrl);    

            Console.WriteLine("*");
            Console.WriteLine("Sending messages on URL: {0}", requestQueueUrl);
            Console.WriteLine("Receiving responses on URL: {0}", responseQueueUrl);
            Console.WriteLine("*");

            var messageBody = Console.ReadLine();
            while (messageBody != "x")
            {
                var parts = messageBody.Split(' ');
                if (parts[0] == "get")
                {
                    var duration = int.Parse(parts[1]);
                    var serviceRequest = new GetChangedEmployeesRequest();
                    serviceRequest.LastChangeDate = DateTime.Now.AddDays(duration).Date;
                    serviceRequest.ResponseQueueUrl = responseQueueUrl;
                    var request = new SendMessageRequest();
                    request.QueueUrl = requestQueueUrl;
                    request.RequestToBody(serviceRequest);
                    SendMessage(request, sqs, serviceRequest);
                }
                if (parts[0] == "flush")
                {
                    var serviceRequest = new FlushDataStoreRequest();
                    serviceRequest.StoreIdentifier = "Sixeyed-CloudServiceBus-ResponseData";
                    var request = new SendMessageRequest();
                    request.QueueUrl = requestQueueUrl;
                    request.RequestToBody(serviceRequest);
                    SendMessage(request, sqs, serviceRequest);
                }

                messageBody = Console.ReadLine();
            }
        }

        private static void SendMessage(SendMessageRequest request, AmazonSQS sqs, ServiceRequest serviceRequest)
        {            
            Console.WriteLine("Sending message, body: {0}, at: {1}", request.MessageBody, DateTime.Now.ToLongTimeString());
            var response = sqs.SendMessage(request);
            Console.WriteLine("Sent message, id: {0}, at: {1}", response.SendMessageResult.MessageId, DateTime.Now.ToLongTimeString());
        }
    }
}
