using Amazon.SQS.Model;
using CloudServiceBus.ServiceProvider.ServiceHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CloudServiceBus.ServiceProvider.Tests
{
    [TestClass]
    public class GetChangedEmployeesTest
    {
        private string fixedBody = @"{""ResponseQueueName"":""b1a84e89-e5b9-4720-9235-dcf9605a59c3"",""LastChangeDate"":""\/Date(1262347200000+0000)\/""}";

        [TestMethod]
        public void IsRequestHandled()
        {
            var message = new Message();
            message.Body = fixedBody;
            var handler = new GetChangedEmployees();
            Assert.IsTrue(handler.IsRequestHandled(message));
        }

        [TestMethod]
        public void IsRequestHandled_Empty()
        {
            var message = new Message();
            var handler = new GetChangedEmployees();
            Assert.IsFalse(handler.IsRequestHandled(message));
        }

        [TestMethod]
        public void IsRequestHandled_Other()
        {
            var message = new Message();
            message.Body = @"{""ResponseQueueName"":""b1a84e89-e5b9-4720-9235-dcf9605a59c3"",""StoreIdentifier"":""Sixeyed-CloudServiceBus-ResponseData""}";
            var handler = new GetChangedEmployees();
            Assert.IsFalse(handler.IsRequestHandled(message));
        }
    }
}
