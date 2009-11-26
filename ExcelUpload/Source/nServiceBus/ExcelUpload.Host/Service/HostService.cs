using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using NServiceBus;
using ExcelUpload.Client.Handlers;
using ExcelUpload.Core.Logging;

namespace ExcelUpload.Host.Service
{
    public class HostService
    {
        private IBus _bus;

        public void Start()
        {
            Log.Info("** Host Started **");

            _bus = NServiceBus.Configure.With()
                .SpringBuilder()
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(true)
                    .PurgeOnStartup(true)
                    .MsmqSubscriptionStorage()
                .UnicastBus()
                    .ImpersonateSender(false)
                    //.LoadMessageHandlers(First<StartBatchUploadHandler>.Then<AddProductHandler>())
                    .LoadMessageHandlers()
                .CreateBus()
                .Start();
        }

        public void Stop()
        {
            
        }
    }
}
