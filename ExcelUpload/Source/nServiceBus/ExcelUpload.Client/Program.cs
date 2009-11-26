using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using System.IO;
using ExcelUpload.Messages;
using NServiceBus;
using StructureMap;
using Topshelf.Configuration;
using ExcelUpload.Client.Service;
using ExcelUpload.Core.ServiceLocators;
using Topshelf;

namespace ExcelUpload.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceName = "ExcelUpload Client Service";
            var cfg = RunnerConfigurator.New(x =>
            {
                x.ConfigureServiceInIsolation<ClientService>("svc", s =>
                {
                    s.CreateServiceLocator(() =>
                    {
                        ObjectFactory.Initialize(i =>
                        {
                            i.ForConcreteType<ClientService>().Configure.WithName("svc");
                        });

                        return new StructureMapServiceLocator();
                    });
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.SetDisplayName(serviceName);
                x.SetServiceName("excelupload_client");
                x.SetDescription("Runs the Excel Upload Client Service (File monitor)");

                x.RunAsLocalSystem();
            });

            Runner.Host(cfg, args);
        }
    }
}
