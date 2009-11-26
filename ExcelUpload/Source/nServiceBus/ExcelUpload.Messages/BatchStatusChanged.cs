using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using ExcelUpload.Messages.Enums;

namespace ExcelUpload.Messages
{
    [Recoverable]
    public class BatchStatusChanged : IMessage
    {
        public Guid BatchId { get; set; }
        public string BatchSourcePath { get; set; }
        public BatchStatus Status { get; set; }
    }
}
