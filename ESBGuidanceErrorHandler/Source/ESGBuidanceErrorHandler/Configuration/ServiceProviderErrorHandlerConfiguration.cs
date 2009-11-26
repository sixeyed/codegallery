#region Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

#endregion

namespace ESBGuidanceErrorHandler.Configuration
{
    /// <summary>
    /// Configuration section for defining settings written to ESB Guidance
    /// </summary>
    public class ServiceProviderErrorHandlerConfiguration : ConfigurationSection
    {
        private static ServiceProviderErrorHandlerConfiguration current;

        /// <summary>
        /// Sets up the current settings from application config
        /// </summary>
        static ServiceProviderErrorHandlerConfiguration()
        {
            current = ConfigurationManager.GetSection("serviceProviderErrorHandlerConfiguration") as ServiceProviderErrorHandlerConfiguration;
        }

        /// <summary>
        /// Returns the currently configured settings
        /// </summary>
        public static ServiceProviderErrorHandlerConfiguration Current
        {
            get { return current; }
        }

        /// <summary>
        /// URL for ESB Exception Handling service
        /// </summary>
        [ConfigurationProperty(ServiceProviderErrorHandlerConfiguration.SettingName.ExceptionHandlingUrl)]
        public string ExceptionHandlingUrl
        {
            get { return (string)this[SettingName.ExceptionHandlingUrl]; }
        }

        /// <summary>
        /// Name of BizTalk Application to log fault against
        /// </summary>
        /// <remarks>
        /// An existing BizTalk Application must be specified
        /// </remarks>
        [ConfigurationProperty(ServiceProviderErrorHandlerConfiguration.SettingName.BizTalkApplication)]
        public string BizTalkApplication
        {
            get { return (string)this[SettingName.BizTalkApplication]; }
        }

        /// <summary>
        /// Any code to identify this error handler
        /// </summary>
        [ConfigurationProperty(ServiceProviderErrorHandlerConfiguration.SettingName.FaultCode)]
        public string FaultCode
        {
            get { return (string)this[SettingName.FaultCode]; }
        }

        /// <summary>
        /// Any string to identify this error handler
        /// </summary>
        [ConfigurationProperty(ServiceProviderErrorHandlerConfiguration.SettingName.ErrorType)]
        public string ErrorType
        {
            get { return (string)this[SettingName.ErrorType]; }
        }

        /// <summary>
        /// Any category to identify this error handler
        /// </summary>
        [ConfigurationProperty(ServiceProviderErrorHandlerConfiguration.SettingName.FailureCategory)]
        public string FailureCategory
        {
            get { return (string)this[SettingName.FailureCategory]; }
        }

        /// <summary>
        /// Any string to identify this error handler
        /// </summary>
        /// <remarks>
        /// 50-character maximum length
        /// </remarks>
        [ConfigurationProperty(ServiceProviderErrorHandlerConfiguration.SettingName.FaultGeneratorName)]
        public string FaultGeneratorName
        {
            get { return (string)this[SettingName.FaultGeneratorName]; }
        }

        /// <summary>
        /// Constants used for indexing settings
        /// </summary>
        private struct SettingName
        {
            public const string ExceptionHandlingUrl = "exceptionHandlingUrl";
            public const string BizTalkApplication = "bizTalkApplication";
            public const string FaultCode = "faultCode";
            public const string ErrorType = "errorType";
            public const string FailureCategory = "failureCategory";
            public const string FaultGeneratorName = "faultGeneratorName";
        }
    }
}
