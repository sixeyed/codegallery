using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelUpload.Messages;
using NServiceBus;
using ExcelUpload.Messages.Enums;
using ExcelUpload.Core.Logging;
using System.IO;

namespace ExcelUpload.Client.Handlers
{
    public class BatchStatusChangedHandler : IMessageHandler<BatchStatusChanged>
    {
        public void Handle(BatchStatusChanged message)
        {
            Log.Info("Batch id: {0}, status: {1}", message.BatchId, Enum.GetName(typeof(BatchStatus), message.Status));
            if (message.Status != BatchStatus.InProgress)
            {
                //rename the file:
                string newFileName = string.Format("{0}-{1}.{2}", message.BatchSourcePath, DateTime.Now.ToString("dd-MM-yy.hh-mm-ss"), message.Status);
                File.Move(message.BatchSourcePath, newFileName);
            }
        }
    }
}
