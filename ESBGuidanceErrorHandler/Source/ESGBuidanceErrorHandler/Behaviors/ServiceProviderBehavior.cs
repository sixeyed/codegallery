#region Directives
using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Description;
using System.ServiceModel;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Configuration;
using ESBGuidanceErrorHandler.ErrorHandlers;

#endregion

namespace ESBGuidanceErrorHandler.Behaviors
{
    /// <summary>
    /// Behaviour which adds generic error handler to WCF service,
    /// loggind unhandled exceptions to ESB Guidance Exception Management
    /// </summary>
    public class ServiceProviderBehavior : BehaviorExtensionElement, IServiceBehavior
    {
        #region IServiceBehaviour implementation

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters) { }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase) { }

        /// <summary>
        /// Adds a ServiceProviderErrorHandler to the dispatcher handler collection
        /// </summary>
        /// <param name="serviceDescription">Service being requested</param>
        /// <param name="serviceHostBase">Service host</param>
        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(new ServiceProviderErrorHandler(serviceDescription.Name));
            }
        }

        #endregion

        #region BehaviorExtensionElement implementation

        /// <summary>
        /// Returns behaviour type
        /// </summary>
        public override Type BehaviorType
        {
            get { return this.GetType(); }
        }

        /// <summary>
        /// Creates a new instance of behavious
        /// </summary>
        /// <returns>Returns new ServiceProviderBehavior</returns>
        protected override object CreateBehavior()
        {
            return new ServiceProviderBehavior();
        }

        #endregion
    }
}