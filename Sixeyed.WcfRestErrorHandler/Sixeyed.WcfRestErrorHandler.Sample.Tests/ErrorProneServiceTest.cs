using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Sixeyed.WcfRestErrorHandler.Sample.Tests
{
    [TestClass]
    public class ErrorProneServiceTest
    {
        [TestMethod]
        public void ServiceExceptionReturns500()
        {
            int statusCode;
            string statusDescription;
            var url = "http://localhost/Sixeyed.WcfRestErrorHandler.Sample/ErrorProneService.svc/dbz";
            DownloadString(url, out statusCode, out statusDescription);
            Assert.AreEqual(500, statusCode);
            Assert.IsTrue(statusDescription.StartsWith("Something has gone wrong. Please contact our support team with helpdesk ID: "));
        }

        [TestMethod]
        public void ClientExceptionReturns400()
        {
            int statusCode;
            string statusDescription;
            
            var url = "http://localhost/Sixeyed.WcfRestErrorHandler.Sample/ErrorProneService.svc/lastLogin?userId=96";
            DownloadString(url, out statusCode, out statusDescription);
            Assert.AreEqual(400, statusCode);
            Assert.AreEqual("User with userId: 96 not found", statusDescription);

            url = "http://localhost/Sixeyed.WcfRestErrorHandler.Sample/ErrorProneService.svc/lastLogin?userId=xyz";
            DownloadString(url, out statusCode, out statusDescription);
            Assert.AreEqual(400, statusCode);
            Assert.AreEqual("Invalid userId. Must be provided as a positive integer", statusDescription);
        }

        [TestMethod]
        public void ValidReturns200()
        {
            int statusCode;
            string statusDescription;

            var url = "http://localhost/Sixeyed.WcfRestErrorHandler.Sample/ErrorProneService.svc/lastLogin?userId=196";
            DownloadString(url, out statusCode, out statusDescription);
            Assert.AreEqual(200, statusCode);
            Assert.AreEqual(string.Empty, statusDescription);
        }

        private void DownloadString(string url, out int statusCode, out string statusDescription)
        {
            string response = null;
            using (var client = new WebClient())
            {
                try
                {
                    response = client.DownloadString(url);
                    statusCode = 200;
                    statusDescription = string.Empty;
                }
                catch (WebException webEx)
                {
                    var webResponse = (HttpWebResponse)webEx.Response;
                    statusCode = (int)webResponse.StatusCode;
                    statusDescription = webResponse.StatusDescription;
                }
            }
        }
    }
}
