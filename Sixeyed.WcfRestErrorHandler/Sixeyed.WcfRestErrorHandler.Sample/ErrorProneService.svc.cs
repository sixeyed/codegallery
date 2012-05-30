using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using Sixeyed.WcfRestErrorHandler.ServiceModel.Exceptions;

namespace Sixeyed.WcfRestErrorHandler.Sample
{
    /// <summary>
    /// Sample service which throws client and server errors
    /// </summary>
    /// <remarks>
    /// Host in IIS, not Cassini, if you want to return HTTP status description:
    /// http://stackoverflow.com/questions/3418959/wcf-4-rest-service-cant-return-a-statusdescription-only-statuscode
    /// </remarks>
    [ServiceContract]
    public class ErrorProneService
    {
        [OperationContract]
        [WebGet(UriTemplate = "lastLogin?userId={userId}", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public DateTime GetLastLoggedInAt(string userId)
        {
            var iUserId = 0;
            if (!int.TryParse(userId, out iUserId) || iUserId < 1)
            {
                throw new ClientException("Invalid userId. Must be provided as a positive integer");
            }
            if (iUserId < 100)
            {
                throw new ClientException("User with userId: {0} not found", iUserId);
            }
            return DateTime.Now.AddDays(-1);
        }

        [OperationContract]
        [WebGet(UriTemplate = "dbz", BodyStyle = WebMessageBodyStyle.Bare, ResponseFormat = WebMessageFormat.Json)]
        public void DivideByZero()
        {
            var iUserId = 0;
            var dbz = 1 / iUserId;
        }
    }
}
