using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;

namespace Sixeyed.OptimisticLockingSample.ServiceModel.Behaviors
{
    /// <summary>
    /// Service behavior for adding <see cref="DataSignatureOperationBehavior"/>
    /// to every operation in a service
    /// </summary>
    public class DataSignatureServiceBehavior : BehaviorExtensionElement, IServiceBehavior
    {
        /// <summary>
        /// Gets the type of the behavior
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(DataSignatureServiceBehavior); }
        }

        /// <summary>
        /// Returns a new instance of the behavior
        /// </summary>
        /// <returns>New <see cref="DataSignatureServiceBehavior"/></returns>
        protected override object CreateBehavior()
        {
            return new DataSignatureServiceBehavior();
        }
        
        /// <summary>
        /// Add binding parameters - no work done
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        /// <param name="endpoints"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            //do nothing
        }

        /// <summary>
        /// the <see cref="DataSignatureOperationBehavior"/> to every operation in the service
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ServiceEndpoint endpoint in serviceDescription.Endpoints)
            {
                foreach (OperationDescription operation in endpoint.Contract.Operations)
                {
                    operation.Behaviors.Add(new DataSignatureOperationBehavior());
                }
            }
        }

        /// <summary>
        /// Validate - no work done
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //do nothing
        }
    }
}
