using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelUpload.Messages;
using NServiceBus;
using ExcelUpload.Messages.Enums;
using ExcelUpload.Data.SqlServer.StoredProcedures;
using Sixeyed.Mapping;
using ExcelUpload.Core.Logging;

namespace ExcelUpload.Client.Handlers
{
    public class AddProductHandler : IMessageHandler<AddProduct>
    {
        public IBus Bus { get; set; }

        public void Handle(AddProduct message)
        {
            Log.Info("Received AddProduct with batch id: {0}, product number: {1}", message.BatchId, message.ProductNumber);
            AddProduct(message);
            SendCompleteNotification(message);
        }

        private void AddProduct(AddProduct message)
        {
            using (var sp = new uspInsertProduct())
            {
                AutoMap<AddProduct, uspInsertProduct>.Populate(message, sp);
                sp.Execute();
            }
        }

        private void SendCompleteNotification(AddProduct message)
        {
            if (!string.IsNullOrEmpty(message.OriginatorDestination) && message.RegistrationIndex == message.RegistrationsInBatch)
            {
                BatchStatusChanged notification = new BatchStatusChanged
                {
                    BatchId = message.BatchId,
                    BatchSourcePath = message.BatchSourcePath,
                    Status = BatchStatus.Completed
                };
                Bus.Send(message.OriginatorDestination, notification);
            }
        }
    }
}
