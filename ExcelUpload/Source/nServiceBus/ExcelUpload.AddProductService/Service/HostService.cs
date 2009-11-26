using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using NServiceBus;
using ExcelUpload.Client.Handlers;
using ExcelUpload.Core.Logging;
using System.Configuration;

namespace ExcelUpload.AddProductService.Service
{
    public class HostService
    {
        private IBus _bus;

        public void Start()
        {
            Log.Info("** AddProductService Started **");

            _bus = NServiceBus.Configure.With()
                .SpringBuilder()
                .XmlSerializer()
                .MsmqTransport()
                    .IsTransactional(true)
                    .PurgeOnStartup(true)
                    .MsmqSubscriptionStorage()
                .UnicastBus()
                    .ImpersonateSender(false)
                    .LoadMessageHandlers()
                .CreateBus()
                .Start();

            if (bool.Parse(ConfigurationManager.AppSettings["ExitAfterSubscribing"]))
            {
                Environment.Exit(0);
            }
        }

        public void Stop()
        {
            
        }
    }
}
