using System;
using System.Configuration;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudServiceBus.Core;
using CloudServiceBus.Core.Aws;
using CloudServiceBus.ServiceProvider.ServiceHandlers;

namespace CloudServiceBus.ServiceProvider.Listeners
{
    /// <summary>
    /// Listener which waits for messages for a specific <see cref="IServiceHandler"/>
    /// </summary>
    public class MessageListener
    {
        private IServiceHandler _handler;
        private AmazonSQS _sqsClient;
        
        private static int ReceiveMessageWaitMilliseconds
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ReceiveMessageWaitMilliseconds"]); }
        }

        public void StartListening(object state)        
        {           
            _handler = state as IServiceHandler;

            if (_handler != null)
            {
                _sqsClient = AwsFacade.GetSqsClient();

                Console.WriteLine("*");
                Console.WriteLine("Listening for messages on Queue: {0} with URL: {1}", _handler.QueueName, _handler.QueueUrl);
                Console.WriteLine("*");

                while (true)
                {
                    ReceiveMessage();
                    Thread.Sleep(ReceiveMessageWaitMilliseconds);
                }
            }
        }

        private void ReceiveMessage()
        {
            var receiveMessageRequest = new ReceiveMessageRequest();
            receiveMessageRequest.QueueUrl = _handler.QueueUrl;
            var receiveMessageResponse = _sqsClient.ReceiveMessage(receiveMessageRequest);
            if (receiveMessageResponse.IsSetReceiveMessageResult() && receiveMessageResponse.ReceiveMessageResult.Message.Count > 0)
            {
                Console.WriteLine("Received message");
                var receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
                foreach (Message message in receiveMessageResult.Message)
                {
                    Console.WriteLine("Message body: {0}", message.Body);
                    foreach (var handler in Container.GetAll<IServiceHandler>())
                    {
                        //TODO - need to refactor this, multiple handlers means multiple deserialization
                        if (handler.QueueName == _handler.QueueName && handler.IsRequestHandled(message))
                        {
                            Console.WriteLine("Passing request to handler: {0}", handler.GetType().Name);
                            handler.HandleRequest(message);
                        }
                    }
                }

                Console.WriteLine("Deleting message");
                var messageRecieptHandle = receiveMessageResponse.ReceiveMessageResult.Message[0].ReceiptHandle;
                var deleteRequest = new DeleteMessageRequest()
                                            .WithQueueUrl(_handler.QueueUrl)
                                            .WithReceiptHandle(messageRecieptHandle);
                _sqsClient.DeleteMessage(deleteRequest);
            }
        }
    }
}
