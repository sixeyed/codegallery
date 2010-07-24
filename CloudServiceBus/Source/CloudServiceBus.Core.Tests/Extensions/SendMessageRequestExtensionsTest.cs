using System;
using Amazon.SQS.Model;
using CloudServiceBus.Core.Extensions;
using CloudServiceBus.Entities.ServiceRequests;
using CloudServiceBus.Entities.ServiceRequests.Operations;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudServiceBus.Core.Tests
{
    [TestClass]
    public class SendMessageRequestExtensionsTest
    {
        private string fixedQueueName = "b1a84e89-e5b9-4720-9235-dcf9605a59c3";
        private DateTime fixedDate = new DateTime(2010, 01, 01, 12, 0, 0);
        private string fixedBody = @"{""ResponseQueueName"":""b1a84e89-e5b9-4720-9235-dcf9605a59c3"",""ServiceRequestName"":""GetChangedEmployeesRequest"",""LastChangeDate"":""\/Date(1262347200000+0000)\/""}";

        [TestMethod]
        public void RequestToBody()
        {
            var entity = new GetChangedEmployeesRequest() { LastChangeDate = fixedDate, ResponseQueueUrl = fixedQueueName };
            var message = new SendMessageRequest();
            message.RequestToBody(entity);
            Assert.AreEqual(fixedBody, message.MessageBody);
        }

        [TestMethod]
        public void ToBody_Flush()
        {
            var entity = new FlushDataStoreRequest() { StoreIdentifier="Sixeyed-CloudServiceBus-ResponseData", ResponseQueueUrl = fixedQueueName };
            var message = new SendMessageRequest();
            message.RequestToBody(entity);
            var expected = @"{""ResponseQueueName"":""b1a84e89-e5b9-4720-9235-dcf9605a59c3"",""ServiceRequestName"":""FlushDataStoreRequest"",""StoreIdentifier"":""Sixeyed-CloudServiceBus-ResponseData""}";
            Assert.AreEqual(expected, message.MessageBody);
        }
    }
}
