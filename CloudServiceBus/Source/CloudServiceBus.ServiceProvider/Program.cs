using System;
using System.Collections.Generic;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudServiceBus.Core;
using CloudServiceBus.Core.Aws;
using CloudServiceBus.ServiceProvider.Listeners;
using CloudServiceBus.ServiceProvider.ServiceHandlers;

namespace CloudServiceBus.ServiceProvider
{
    class Program
    {
        private static Dictionary<string, string> _registeredHandlerUrls = new Dictionary<string, string>();
        private static AmazonSQS SqsClient { get; set; }             
        private static IEnumerable<IServiceHandler> ServiceHandlers
        {
            get { return Container.GetAll<IServiceHandler>(); }
        }        

        static void Main(string[] args)
        {
            Console.Title = "CloudServiceBus: Service Provider";
            Console.BackgroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = ConsoleColor.Black;

            //register all the service handlers:
            Container.RegisterAll<IServiceHandler>();
            
            //create the queue service client:
            SqsClient = AwsFacade.GetSqsClient();
            
            //start listening:
            StartHandlers();
            
            Console.ReadLine();
        }

        private static void StartHandlers()
        {
            foreach (var handler in ServiceHandlers)
            {
                EnsureQueue(handler);
                ListenForMessages(handler);
            }
        }

        private static void ListenForMessages(IServiceHandler handler)
        {
            var listener = new MessageListener();
            ThreadPool.QueueUserWorkItem(new WaitCallback(listener.StartListening), handler);            
        }        

        private static void EnsureQueue(IServiceHandler handler)
        {
            if (!_registeredHandlerUrls.ContainsKey(handler.QueueName))
            {
                var createQueueRequest = new CreateQueueRequest();
                createQueueRequest.QueueName = handler.QueueName;
                var createQueueResponse = SqsClient.CreateQueue(createQueueRequest);
                var queueUrl = createQueueResponse.CreateQueueResult.QueueUrl;
                _registeredHandlerUrls[handler.QueueName] = queueUrl;
            }
            handler.QueueUrl = _registeredHandlerUrls[handler.QueueName];
        }
    }
}
