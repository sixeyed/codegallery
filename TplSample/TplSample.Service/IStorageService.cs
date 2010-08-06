using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace TplSample.Service
{
    [ServiceContract]
    public interface IStorageService
    {
        [OperationContract]
        void Store(string value);
    }

}
