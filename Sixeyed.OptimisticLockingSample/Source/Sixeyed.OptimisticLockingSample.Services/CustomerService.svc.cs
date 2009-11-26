using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Sixeyed.OptimisticLockingSample.Entities;
using entityServices = Sixeyed.OptimisticLockingSample.EntityServices;
using Sixeyed.OptimisticLockingSample.EntityServices;
using System.ServiceModel.Channels;
using Sixeyed.OptimisticLockingSample.ServiceModel.Behaviors;
using Sixeyed.OptimisticLockingSample.ServiceModel;
using Sixeyed.OptimisticLockingSample.ServiceModel.FaultDetail;

namespace Sixeyed.OptimisticLockingSample.Services
{
    [ServiceBehavior(Namespace = "http://Sixeyed.OptimisticLockingSample/2009")]
    public class CustomerService : ICustomerService
    {
        private entityServices.CustomerService CustomerEntityService { get; set; }

        public CustomerService()
        {
            CustomerEntityService = new entityServices.CustomerService();
        }

        public Customer GetCustomer(int id)
        {
            Customer customer = new Customer();
            customer.Id = id;
            customer = new entityServices.CustomerService().Load(customer);
            return customer;
        }

        public void UpdateCustomer(Customer customer)
        {
            Guid dataSignature = DataSignature.Current;
            if (dataSignature == Guid.Empty)
            {
                //this is an update method, so no data signature to 
                //compare against is an exception:
                throw new FaultException<NoDataSignature>(new NoDataSignature());
            }

            Customer currentState = CustomerEntityService.Load(customer);
            Guid currentDataSignature = DataSignature.Sign(currentState);
            //if the data signatures match then update:
            if (currentDataSignature == dataSignature)
            {
                CustomerEntityService.Update(customer);
            }
            else
            {
                //otherwise, throw concurrency violation exception:
                throw new FaultException<ConcurrencyViolation>(new ConcurrencyViolation());
            }
        }
    }
}
