using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using Amazon.SQS;
using Amazon.SQS.Model;
using CloudServiceBus.Core.Aws;
using CloudServiceBus.Core.DataStores;
using CloudServiceBus.Core.Extensions;
using CloudServiceBus.Core.Serialization;
using CloudServiceBus.Entities;
using CloudServiceBus.Entities.ServiceResponses;

namespace CloudServiceBus.Client.Listeners
{
    public class MessageListener
    {
        private static int ReceiveMessageWaitMilliseconds
        {
            get { return int.Parse(ConfigurationManager.AppSettings["ReceiveMessageWaitMilliseconds"]); }
        }

        private string _queueUrl;
        private AmazonSQS _sqsClient;

        public void StartListening(object state)        
        {
            _queueUrl = state as string;

            if (!string.IsNullOrEmpty(_queueUrl))
            {
                _sqsClient = AwsFacade.GetSqsClient();

                Console.WriteLine("*");
                Console.WriteLine("Listening for messages on URL: {0}", _queueUrl);
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
            receiveMessageRequest.QueueUrl = _queueUrl;
            var receiveMessageResponse = _sqsClient.ReceiveMessage(receiveMessageRequest);
            if (receiveMessageResponse.IsSetReceiveMessageResult() && receiveMessageResponse.ReceiveMessageResult.Message.Count > 0)
            {
                Console.WriteLine("Received message at: {0}", DateTime.Now.ToLongTimeString());
                var receiveMessageResult = receiveMessageResponse.ReceiveMessageResult;
                foreach (Message message in receiveMessageResult.Message)
                {
                    Console.WriteLine("Message body: {0}", message.Body);
                }

                var response = receiveMessageResponse.FirstMessageAsResponse<QueryServiceResponse>();
                if (response != null)
                {
                    //fetch the data from the response store:  
                    Console.WriteLine("Fetching data at: {0}", DateTime.Now.ToLongTimeString());
                    var responseItems = DataStore.Current.Fetch(response.StoreIdentifier, response.ItemKey);
                    var employees = new List<Employee>(responseItems.Count());
                    foreach (var responseItem in responseItems)
                    {
                        var employee = Serializer.Current.Deserialize(typeof(Employee), responseItem) as Employee;
                        Console.WriteLine("Adding employee - {0}", employee);
                        employees.Add(employee);
                    }
                }

                Console.WriteLine("Deleting message");
                var messageRecieptHandle = receiveMessageResponse.ReceiveMessageResult.Message[0].ReceiptHandle;
                var deleteRequest = new DeleteMessageRequest()
                                            .WithQueueUrl(_queueUrl)
                                            .WithReceiptHandle(messageRecieptHandle);
                _sqsClient.DeleteMessage(deleteRequest);
                Console.WriteLine("Completed at: {0}", DateTime.Now.ToLongTimeString());
            }
        }
    }
}
