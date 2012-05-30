using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Configuration;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Sixeyed.WcfRestErrorHandler.ServiceModel.ErrorHandlers;

namespace Sixeyed.WcfRestErrorHandler.ServiceModel.Behaviors
{
    /// <summary>
    /// <see cref="IServiceBehvavior"/> which adds <see cref="RestErrorHandler"/> to all services
    /// </summary>
    public class RestErrorHandlerBehavior : BehaviorExtensionElement, IServiceBehavior
    {
        /// <summary>
        /// No work done
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
        /// Adds <see cref="BoundaryExceptionErrorHandler"/> to all channel dispatchers
        /// </summary>
        /// <param name="serviceDescription">Service description</param>
        /// <param name="serviceHostBase">Service host base</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(new RestErrorHandler());
            }
        }

        /// <summary>
        /// No work done
        /// </summary>
        /// <param name="serviceDescription"></param>
        /// <param name="serviceHostBase"></param>
        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            //do nothing
        }

        /// <summary>
        /// No work done
        /// </summary>
        public override Type BehaviorType
        {
            get { return GetType(); }
        }

        /// <summary>
        /// Returns <see cref="RestErrorHandlerBehavior"/>
        /// </summary>
        /// <returns></returns>
        protected override object CreateBehavior()
        {
            return new RestErrorHandlerBehavior();
        }
    }
}
