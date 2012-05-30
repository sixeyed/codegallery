using System;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Web;
using Sixeyed.WcfRestErrorHandler.ServiceModel.Exceptions;
using Sixeyed.WcfRestErrorHandler.ServiceModel.Messages;

namespace Sixeyed.WcfRestErrorHandler.ServiceModel.ErrorHandlers
{
    /// <summary>
    /// Error handler returning HTTP status codes
    /// </summary>
    public class RestErrorHandler : IErrorHandler
    {
        /// <summary>
        /// Handle error; does nothing
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool HandleError(Exception error)
        {                       
            return true;
        }

        /// <summary>
        /// Builds a fault message to return to the caller
        /// </summary>
        /// <remarks>
        /// For <see cref="ClientException"/>s, returns a 400 status to the client with the message in the status description, 
        /// and a friendly HTML message body. For <see cref="ServerException"/>s, returns a 500 status to the client and hides
        /// the actual error, returns a GUID for the error Id in the status description and an empty response body.
        /// </remarks>
        /// <param name="error"></param>
        /// <param name="version"></param>
        /// <param name="fault"></param>
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            var response = WebOperationContext.Current.OutgoingResponse;
            var clientEx = error as ClientException;
            if (clientEx != null)
            {
                Log("Client exception, returning 400, message: {0}. Exception: {1}", clientEx.Message, clientEx);
                var htmlFault = new HtmlFaultMessage();
                response.StatusCode = HttpStatusCode.BadRequest;
                response.StatusDescription = clientEx.Message;
                htmlFault.Message = clientEx.Message;
                response.ContentType = "text/html";
                fault = htmlFault;
            }
            else
            {
                var serverEx = new ServerException();
                Log("Server exception, returning 500, error ID: {0}. Exception: {1}", serverEx.ErrorId, error);
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.StatusDescription = serverEx.Message;
                response.SuppressEntityBody = true;
            }
        }

        private static void Log(string messageFormat, params object[] args)
        {
            var message = string.Format(messageFormat, args);
            Debug.WriteLine(string.Format("Error: {0}, serving URI: {1}", message, OperationContext.Current.RequestContext.RequestMessage.Headers.To.OriginalString));
        }
    }
}
