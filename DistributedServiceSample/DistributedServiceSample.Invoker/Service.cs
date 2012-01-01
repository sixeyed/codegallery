using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using DistributedServiceSample.Contracts.Services;
using DistributedServiceSample.Core.Containers;
using DistributedServiceSample.Core.Extensions;
using DistributedServiceSample.Core.Logging;
using DistributedServiceSample.Invoker.Configuration;
using DistributedServiceSample.Core.Configuration.Elements;
using System.Threading.Tasks;

namespace DistributedServiceSample.Invoker
{
    public static class Service
    {
        private static List<string> _RegisteredAssemblies = new List<string>();
        private static object _SyncLock = new object();

        public static void RegisterImplementations(Assembly serviceAssembly)
        {
            if (!_RegisteredAssemblies.Contains(serviceAssembly.FullName))
            {
                lock (_SyncLock)
                {
                    Container.RegisterAll<IService>(serviceAssembly);
                    _RegisteredAssemblies.Add(serviceAssembly.FullName);
                }
            }            
        }

        public static void Execute<TService>(Action<TService> serviceCall)
                    where TService : IService
        {
            Invoke<TService>(serviceCall); 
        }

        private static void ClearUp<TService>(TService service) where TService : IService
        {
            if (service is ICommunicationObject)
            {
                ((ICommunicationObject)service).CloseSafely();
            }
            if (service is IDisposable)
            {
                ((IDisposable)service).Dispose();
            }
        }

        public static TResponse Execute<TService, TResponse>(Func<TService, TResponse> serviceCall)
                    where TService : IService            
        {
            var response = default(TResponse);
            var serviceResponse = Invoke<TService>(serviceCall);            
            if (typeof(TResponse).IsAssignableFrom(serviceResponse.GetType()))
            {
                response = (TResponse)serviceResponse;
            }
            else
            {
                throw new ApplicationException("Unable to convert service response of type: {0} to request response type: {1}".FormatWith(serviceResponse.GetType().Name, typeof(TResponse).Name));
            }
            return response;
        }

        private static object Invoke<TService>(Delegate serviceCall)
            where TService : IService
        {
            object response = null;
            var executionMode = GetExecutionMode<TService>();
            switch (executionMode)
            {
                case ExecutionMode.Synchronous:
                    response = InvokeInternal<TService>(serviceCall);
                    break;
                case ExecutionMode.AsynchronousIgnoreResponse:
                    Task.Factory.StartNew(() => InvokeInternal<TService>(serviceCall));
                    break;
            }
            return response;
        }

        private static object InvokeInternal<TService>(Delegate serviceCall)
            where TService : IService
        {
            object response = null;
            //this is wrapped as it may be running as a Task in a separate thread:
            try
            {
                var service = GetService<TService>();
                var parms = new object[] { service };
                response = serviceCall.DynamicInvoke(parms);
                ClearUp<TService>(service);
            }
            catch (Exception ex)
            {
                Log.Trace("Service.InvokeInternal - ERROR: {0}", ex);
            }
            return response;
        }

        private static TService GetService<TService>()
            where TService : IService
        {
            var location = GetServiceLocation<TService>();
                        TService service = default(TService);
            if (location == ServiceLocation.Local)
            {             
                service = Container.Get<TService>();
            }
            else
            {                
                //ES - this assumes one <client> entry for the service in config:
                var factory = new ChannelFactory<TService>("*");
                service = factory.CreateChannel();
            }
            if (service == null)
            {
                throw new ApplicationException("Unable to resolve implemention for service: {0}, location: {1}".FormatWith(typeof(TService).FullName, location));
            }
            return service;
        }

        private static ServiceLocation GetServiceLocation<TService>()
            where TService : IService
        { 
            var location = InvokerConfiguration.Current.DefaultServiceLocation;
            var config = (from ServiceElement s in InvokerConfiguration.Current.Services
                          where s.Contract == typeof(TService).FullName
                          select s).SingleOrDefault();
            if (config != null)
            {
                location = config.ServiceLocation;
            }
            Log.Trace("Service.GetServiceLocation using *{0}* for service: {1}", location, typeof(TService).FullName);
            return location;
        }

        private static ExecutionMode GetExecutionMode<TService>()
            where TService : IService
        {
            var mode = InvokerConfiguration.Current.DefaultExecutionMode;
            var config = (from ServiceElement s in InvokerConfiguration.Current.Services
                          where s.Contract == typeof(TService).FullName
                          select s).SingleOrDefault();
            if (config != null)
            {
                mode = config.ExecutionMode;
            }
            Log.Trace("Service.GetExecutionMode using *{0}* for service: {1}", mode, typeof(TService).FullName);
            return mode;
        }
    }
}
