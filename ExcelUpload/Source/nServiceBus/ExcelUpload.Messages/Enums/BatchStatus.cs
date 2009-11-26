using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace ExcelUpload.Messages.Enums
{
    public enum BatchStatus
    {
        InProgress,
        Errored,
        Completed
    }
}
