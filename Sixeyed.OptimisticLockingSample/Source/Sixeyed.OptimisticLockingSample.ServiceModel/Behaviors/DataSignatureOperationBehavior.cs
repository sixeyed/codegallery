using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using Sixeyed.OptimisticLockingSample.ServiceModel.MessageFormatters;

namespace Sixeyed.OptimisticLockingSample.ServiceModel.Behaviors
{
    /// <summary>
    /// Operation behavior for adding <see cref="DataSignatureClientFormatter"/> to client
    /// operations and <see cref="DataSignatureDispatchFormatter"/> to dispatch operations
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class DataSignatureOperationBehavior : Attribute, IOperationBehavior
    {    
        /// <summary>
        /// Add binding parameters - no work done
        /// </summary>
        /// <param name="operationDescription"></param>
        /// <param name="bindingParameters"></param>
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
            //do nothing
        }

        /// <summary>
        /// Adds the <see cref="DataSignatureClientFormatter"/> to the operation
        /// </summary>
        /// <param name="operationDescription">OperationDescription</param>
        /// <param name="clientOperation">ClientOperation</param>
        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
            DataSignatureClientFormatter formatter = new DataSignatureClientFormatter();
            formatter.OrginalFormatter = clientOperation.Formatter;
            clientOperation.Formatter = formatter;            
        }

        /// <summary>
        /// Adds the <see cref="DataSignatureDispatchFormatter"/> to the operation
        /// </summary>
        /// <param name="operationDescription">OperationDescription</param>
        /// <param name="dispatchOperation">DispatchOperation</param>
        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
            DataSignatureDispatchFormatter formatter = new DataSignatureDispatchFormatter();
            formatter.OrginalFormatter = dispatchOperation.Formatter;
            dispatchOperation.Formatter = formatter; 
        }

        /// <summary>
        /// Validate - no work done
        /// </summary>
        /// <param name="operationDescription"></param>
        public void Validate(OperationDescription operationDescription)
        {
            //do nothing
        }
    }
}
