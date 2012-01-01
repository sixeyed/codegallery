
using System.Configuration;
using DistributedServiceSample.Invoker.Configuration;

namespace DistributedServiceSample.Core.Configuration.Elements
{
    public class ServiceElement : ConfigurationElement
    {
        [ConfigurationProperty(SettingName.Contract)]
        public string Contract
        {
            get { return (string)this[SettingName.Contract]; }
        }

        [ConfigurationProperty(SettingName.ServiceLocation, DefaultValue = ServiceLocation.Local)]
        public ServiceLocation ServiceLocation
        {
            get { return (ServiceLocation)this[SettingName.ServiceLocation]; }
        }

        [ConfigurationProperty(SettingName.ExecutionMode, DefaultValue = ExecutionMode.Synchronous)]
        public ExecutionMode ExecutionMode
        {
            get { return (ExecutionMode)this[SettingName.ExecutionMode]; }
        }

        /// <summary>
        /// Constants for indexing settings
        /// </summary>
        private struct SettingName
        {
            /// <summary>
            /// contract
            /// </summary>
            public const string Contract = "contract";

            /// <summary>
            /// serviceLocation
            /// </summary>
            public const string ServiceLocation = "serviceLocation";

            /// <summary>
            /// executionMode
            /// </summary>
            public const string ExecutionMode = "executionMode";
        }
    }
}

