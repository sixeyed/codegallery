using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace ExcelUpload.Messages
{
    [Recoverable]
    public class StartBatchUpload : IMessage
    {
        public Guid BatchId { get; set; }
        public string BatchSourcePath { get; set; }
    }
}
