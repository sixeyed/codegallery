using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using NServiceBus;
using ExcelUpload.Client.Handlers;
using System.IO;
using ExcelUpload.Messages;
using System.Configuration;
using ExcelUpload.Core.Logging;

namespace ExcelUpload.Client.Service
{
    public class ClientService
    {
        private IBus _bus;
        private FileSystemWatcher _watcher;

        public void Start()
        {
            Log.Info("** Client Started **");
            string uploadPath = ConfigurationManager.AppSettings["UploadPath"]; //TODO - move to typed config section
            if (!string.IsNullOrEmpty(uploadPath) && Directory.Exists(uploadPath))
            {
                InitialiseBus();
                StartFileSystemWatcher(uploadPath);
                Log.Info("Monitoring path: {0}", uploadPath);
            }
            else
            {
                Log.Error("Configured path: {0} not found. Not monitoring.", uploadPath);
            }
        }

        private void StartFileSystemWatcher(string uploadPath)
        {
            _watcher = new FileSystemWatcher(uploadPath);
            _watcher.EnableRaisingEvents = true;
            _watcher.Created += new FileSystemEventHandler(watcher_Created);
        }

        void watcher_Created(object sender, FileSystemEventArgs e)
        {
            string path = Path.GetFullPath(e.FullPath);
            string extension = Path.GetExtension(path).ToLower();
            if (extension == ".xls" || extension == ".xslx")
            {
                Log.Info("Submitting file for processing: {0}", path);
                Guid batchId = Guid.NewGuid();
                _bus.Send<StartBatchUpload>(m =>
                {
                    m.BatchId = batchId;
                    m.BatchSourcePath = path;
                });
            }
        }

        private void InitialiseBus()
        {
            _bus = NServiceBus.Configure.With()
                                .SpringBuilder()
                                .XmlSerializer()
                                .MsmqTransport()
                                    .IsTransactional(true)
                                    .PurgeOnStartup(true)
                                .UnicastBus()
                                    .ImpersonateSender(false)
                                    .LoadMessageHandlers()
                                .CreateBus()
                                .Start();
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
            _bus = null;
        }
    }
}
