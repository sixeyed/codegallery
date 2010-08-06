using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading;

namespace TplSample.Service
{
    public class StorageService : IStorageService
    {
        public void Store(string value)
        {
            Thread.Sleep(30);
        }
    }
}
