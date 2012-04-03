using System.ServiceModel;
using System.ServiceModel.Web;

namespace WcfSampleService
{
    [ServiceContract]
    public interface IPersonService
    {
        [OperationContract]
        [WebGet(UriTemplate = "update/{personId}/{lastname}/{firstName}")]
        void Update(string personId, string lastName, string firstName);

        [OperationContract]
        [WebGet(UriTemplate = "fetch/{personId}")]
        Person Fetch(string personId);
    }
}
