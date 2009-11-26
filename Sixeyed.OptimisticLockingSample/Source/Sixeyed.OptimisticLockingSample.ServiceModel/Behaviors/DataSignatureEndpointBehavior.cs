using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Sixeyed.OptimisticLockingSample.ServiceModel.Behaviors
{
    /// <summary>
    /// Endpoint behavior for adding <see cref="DataSignatureOperationBehavior"/>
    /// to every operation in a client's endpoint
    /// </summary>
    public class DataSignatureEndpointBehavior : BehaviorExtensionElement, IEndpointBehavior
    {
        /// <summary>
        /// Gets the type of the behavior
        /// </summary>
        public override Type BehaviorType
        {
            get { return typeof(DataSignatureEndpointBehavior); }
        }

        /// <summary>
        /// Returns a new instance of the behavior
        /// </summary>
        /// <returns>New <see cref="DataSignatureEndpointBehavior"/></returns>
        protected override object CreateBehavior()
        {
            return new DataSignatureEndpointBehavior();
        }

        /// <summary>
        /// Add binding parameters - no work done
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) 
        { 
            //do nothing
        }

        /// <summary>
        /// Adds the <see cref="DataSignatureOperationBehavior"/> to every operation in the endpoint
        /// </summary>
        /// <param name="endpoint">Service endpoint</param>
        /// <param name="clientRuntime">Client runtim</param>
        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            //add data signature behaviour to all operations - this allows
            //us to add the client message formatter in config:
            foreach (OperationDescription operation in endpoint.Contract.Operations)
            {
                operation.Behaviors.Add(new DataSignatureOperationBehavior());
            }
        }

        /// <summary>
        /// Apply dispatch behavior - no work done
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="endpointDispatcher"></param>
        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            //do nothing
        }

        /// <summary>
        /// Validate - no work done
        /// </summary>
        /// <param name="endpoint"></param>
        public void Validate(ServiceEndpoint endpoint) 
        { 
            //do nothing
        }
    }
}
