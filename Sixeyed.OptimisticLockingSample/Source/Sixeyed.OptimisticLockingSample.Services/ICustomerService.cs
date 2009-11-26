using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Sixeyed.OptimisticLockingSample.Entities;
using Sixeyed.OptimisticLockingSample.ServiceModel.Behaviors;
using Sixeyed.OptimisticLockingSample.ServiceModel.FaultDetail;

namespace Sixeyed.OptimisticLockingSample.Services
{
    [ServiceContract(Namespace = "http://Sixeyed.OptimisticLockingSample/2009")]
    public interface ICustomerService
    {
        [OperationContract]
        Customer GetCustomer(int id);

        [OperationContract]
        [FaultContract(typeof(NoDataSignature))]
        [FaultContract(typeof(ConcurrencyViolation))]
        void UpdateCustomer(Customer customer);
    }
}
