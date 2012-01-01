using System.Configuration;
using DistributedServiceSample.Core.Configuration.Collections;
using DistributedServiceSample.Core.Logging;

namespace DistributedServiceSample.Invoker.Configuration
{
    /// <summary>
    /// Configuration section for configuring caching
    /// </summary>
    public class InvokerConfiguration : ConfigurationSection
    {
        private static bool _loggedWarning;

        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static InvokerConfiguration Current
        {
            get
            {
                var current = ConfigurationManager.GetSection("distributedservicesample.invoker") as InvokerConfiguration;
                if (current == null)
                {
                    current = new InvokerConfiguration();
                    if (!_loggedWarning)
                    {
                        Log.Trace("Configuration section: <distributedservicesample.invoker> not specified. Default configuration will be used");
                        _loggedWarning = true;
                    }
                }
                return current; 
            }
        }

        [ConfigurationProperty(SettingName.Services)]
        public ServiceCollection Services
        {
            get { return (ServiceCollection)this[SettingName.Services]; }
        }

        /// <summary>
        /// Returns the configured name of the out-of-process cache
        /// </summary>
        [ConfigurationProperty(SettingName.DefaultServiceLocation, DefaultValue = ServiceLocation.Local)]
        public ServiceLocation DefaultServiceLocation
        {
            get { return (ServiceLocation)this[SettingName.DefaultServiceLocation]; }
        }

        /// <summary>
        /// Returns the configured name of the out-of-process cache
        /// </summary>
        [ConfigurationProperty(SettingName.DefaultExecutionMode, DefaultValue = ExecutionMode.Synchronous)]
        public ExecutionMode DefaultExecutionMode
        {
            get { return (ExecutionMode)this[SettingName.DefaultExecutionMode]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// defaultServiceLocation
            /// </summary>
            public const string DefaultServiceLocation = "defaultServiceLocation";

            /// <summary>
            /// defaultExecutionMode
            /// </summary>
            public const string DefaultExecutionMode = "defaultExecutionMode";

            /// <summary>
            /// services
            /// </summary>
            public const string Services = "services";
        }
    }
}
