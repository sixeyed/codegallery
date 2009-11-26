#region Directives

using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.ServiceModel;
using System.Web.Services.Protocols;
using Microsoft.Practices.ESB.ExceptionManagement;
using ESBGuidanceErrorHandler.Enums;
using System.Configuration;
using System.Security.Cryptography;
using ESBGuidanceErrorHandler.Configuration;

#endregion

namespace ESBGuidanceErrorHandler.ErrorHandlers
{
    /// <summary>
    /// Error handler to log all unhandled exceptions as faults with ESBG
    /// </summary>
    public class ServiceProviderErrorHandler : IErrorHandler
    {
        #region Private instance fields

        private string _serviceProviderName;

        #endregion

        #region Private properties

        private string ServiceProviderName
        {
            get { return _serviceProviderName; }
            set { _serviceProviderName = value; }
        }

        private string ServiceName
        {
            get
            {
                string serviceName;
                try
                {
                    serviceName = OperationContext.Current.RequestContext.RequestMessage.Headers.Action;
                }
                catch
                {
                    serviceName = "<Unknown>";
                }
                return serviceName;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with initial state
        /// </summary>
        /// <param name="serviceProviderName">Name of service provider</param>
        public ServiceProviderErrorHandler(string serviceProviderName)
        {
            this._serviceProviderName = serviceProviderName;
        }

        #endregion

        #region Public instance methods

        /// <summary>
        /// Submits all unhandled exceptions as faults to the ESB Exception Management service
        /// </summary>
        /// <param name="error">Unhandled exception</param>
        /// <returns>Whether submission of fault succeeded</returns>
        public bool HandleError(Exception error)
        {
            SubmitFault(this.ServiceProviderName, this.ServiceName, string.Format("Unhandled exception: {0}", error.Message), FaultSeverity.Error, error);
            return true;
        }

        /// <summary>
        /// Builds a fault message for to return to the caller
        /// </summary>
        /// <remarks>
        /// Builds an undeclared SOAP fault (using FaultException, rather than generic FaultException), 
        /// for interop with non-WCF clients which won't recognise declared SOAP faults
        /// </remarks>
        /// <param name="error">Unhandled exception</param>
        /// <param name="version">Message version</param>
        /// <param name="fault">Fault message</param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            //build undeclared SOAP fault:
            FaultException ex = new FaultException(error.Message, FaultCode.CreateSenderFaultCode(SoapException.ServerFaultCode.Name, SoapException.ServerFaultCode.Namespace));
            MessageFault messageFault = ex.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, this.ServiceName);
        }

        #endregion

        #region Public static methods

        /// <summary>
        /// Submits an exception as a fault to the ESB Exception database
        /// </summary>
        /// <remarks>
        /// The majority of the properties populated in the fault are needed for the WCF-generated
        /// fault to look the same as an ESB-generated fault, as null values are allowed in the 
        /// database, but will prevent records showing in the ESB management portal.
        /// </remarks>
        /// <param name="serviceProviderName">Name of the service provider</param>
        /// <param name="serviceName">Name of the service submitting the fault</param>
        /// <param name="faultDescription">Description or fault/error message</param>
        /// <param name="severity">Severity of fault</param>
        /// <param name="ex">Exception that was caught</param>
        public static void SubmitFault(string serviceProviderName, string serviceName, 
                                       string faultDescription, FaultSeverity severity, Exception ex)
        {
            DateTime now = DateTime.Now;

            //populate the header:
            FaultMessageHeader header = new FaultMessageHeader();
            header.DateTime = now.ToString();
            header.MachineName = Environment.MachineName;
            header.Application = ServiceProviderErrorHandlerConfiguration.Current.BizTalkApplication;
            header.ServiceName = string.Format("{0}.{1}", serviceProviderName, serviceName);
            header.ErrorType = ServiceProviderErrorHandlerConfiguration.Current.ErrorType;
            header.FailureCategory = ServiceProviderErrorHandlerConfiguration.Current.FailureCategory;
            header.FaultCode = ServiceProviderErrorHandlerConfiguration.Current.FaultCode;
            header.FaultSeverity = (int)severity;
            header.FaultDescription = faultDescription;
            header.FaultGenerator = ServiceProviderErrorHandlerConfiguration.Current.FaultGeneratorName;
            header.Description = string.Format("Fault occurred in provider: {0}, processing service: {1}", serviceProviderName, serviceName);
            header.MessageID = GetDeterministicGuid("{0}_{1}_{2}", serviceProviderName, serviceName, now.Ticks).ToString();
            header.ActivityIdentity = GetDeterministicGuid("{0}_{1}", serviceProviderName, serviceName).ToString();
            header.ServiceInstanceID = GetDeterministicGuid(serviceProviderName).ToString();
            //OperationContext is thread static, no may be null if call from child threads:
            if (OperationContext.Current == null)
            {
                header.Scope = "Worker thread";
            }
            else
            {
                header.Scope = OperationContext.Current.EndpointDispatcher.EndpointAddress.Uri.AbsoluteUri;
            }

            //populate exception details:
            FaultMessageExceptionObject faultException = new FaultMessageExceptionObject();
            faultException.Source = ex.Source;
            faultException.StackTrace = ex.StackTrace;
            faultException.Message = ex.Message;
            faultException.Type = ex.GetType().FullName;
            if (ex.TargetSite != null)
            {
                faultException.TargetSite = ex.TargetSite.ToString();
            }
            if (ex.InnerException != null)
            {
                faultException.InnerExceptionMessage = ex.InnerException.Message;
            }
            else
            {
                //ESB uses the fault code here:
                faultException.InnerExceptionMessage = ServiceProviderErrorHandlerConfiguration.Current.FaultCode;
            }

            FaultMessage message = new FaultMessage();
            message.Header = header;
            message.ExceptionObject = faultException;
            SubmitFault(message);
        }

        /// <summary>
        /// Submits the fault message to the configured ESB service
        /// </summary>
        /// <param name="faultMessage">Fault message to submit</param>
        private static void SubmitFault(FaultMessage faultMessage)
        {
            using (ExceptionHandling proxy = new ExceptionHandling())
            {
                proxy.Url = ServiceProviderErrorHandlerConfiguration.Current.ExceptionHandlingUrl;
                proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                proxy.SubmitFault(faultMessage);
            }
        }

        /// <summary>
        /// Returns a deterministic GUID based on the input string
        /// </summary>
        /// <remarks>
        /// Uses MD5 hash to ensure the same GUID is always returned from the same string
        /// </remarks>
        /// <param name="inputFormat">Input string format</param>
        /// <param name="args">Input arguments</param>
        /// <returns>Determinstic GUID</returns>
        private static Guid GetDeterministicGuid(string inputFormat, params object[] args)
        {
            //use MD5 hash to get a 16-byte hash of the string: 
            string input = string.Format(inputFormat, args);
            MD5CryptoServiceProvider provider = new MD5CryptoServiceProvider();
            byte[] inputBytes = Encoding.Default.GetBytes(input);
            byte[] hashBytes = provider.ComputeHash(inputBytes);

            //generate a guid from the hash: 
            Guid hashGuid = new Guid(hashBytes);
            return hashGuid;
        }

        #endregion
    }
}
