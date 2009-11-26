using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;
using log4net;
using ExcelUpload.Client.Handlers;
using Topshelf.Configuration;
using ExcelUpload.Host.Service;
using Topshelf;
using ExcelUpload.Core.ServiceLocators;
using StructureMap;

namespace ExcelUpload.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            string serviceName = "ExcelUpload Host Service";
            var cfg = RunnerConfigurator.New(x =>
                        {
                            x.ConfigureServiceInIsolation<HostService>("svc", s =>
                            {
                                s.CreateServiceLocator(() =>
                                {
                                    ObjectFactory.Initialize(i =>
                                    {
                                        i.ForConcreteType<HostService>().Configure.WithName("svc");
                                    });

                                    return new StructureMapServiceLocator();
                                });
                                s.WhenStarted(tc => tc.Start());
                                s.WhenStopped(tc => tc.Stop());
                            });

                            x.SetDisplayName(serviceName);
                            x.SetServiceName("excelupload_host");
                            x.SetDescription("Runs the Excel Upload Host Service (Batch processor)");

                            x.RunAsLocalSystem();
                        });

            Runner.Host(cfg, args);
        }
    }
}
