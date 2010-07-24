using System;
using Amazon.SQS.Model;
using CloudServiceBus.Core.Extensions;
using CloudServiceBus.Entities.ServiceRequests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudServiceBus.Core.Tests
{
    [TestClass]
    public class MessageExtensionsTest
    {
        private string fixedQueueName = "b1a84e89-e5b9-4720-9235-dcf9605a59c3";
        private DateTime fixedDate = new DateTime(2010, 01, 01, 12, 0, 0);
        private string fixedBody = @"{""ResponseQueueName"":""b1a84e89-e5b9-4720-9235-dcf9605a59c3"",""ServiceRequestName"":""GetChangedEmployeesRequest"",""LastChangeDate"":""\/Date(1262347200000+0000)\/""}";

        [TestMethod]
        public void BodyAs()
        {
            var message = new Message();
            message.Body = fixedBody;
            var request = message.BodyAsRequest<GetChangedEmployeesRequest>();
            Assert.IsNotNull(request);
            Assert.AreEqual(fixedDate, request.LastChangeDate);
            Assert.AreEqual(fixedQueueName, request.ResponseQueueUrl);
        }
    }
}
