using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExcelUpload.Messages;
using NServiceBus;
using ExcelUpload.Messages.Enums;
using System.IO;
using Excel;
using ExcelUpload.Core.Logging;

namespace ExcelUpload.Client.Handlers
{
    public class StartBatchUploadHandler : IMessageHandler<StartBatchUpload>
    {
        public IBus Bus { get; set; }

        public void Handle(StartBatchUpload message)
        {
            Log.Info("Received StartBatchUpload with batch id: {0}, path: {1}", message.BatchId, message.BatchSourcePath);

            FileInfo info = new FileInfo(message.BatchSourcePath);
            if (info == null)
                return;

            using (FileStream stream = new FileStream(info.FullName, FileMode.Open, FileAccess.Read))
            {
                //ES - ResultsCount isn't reliable on ExcelDataReader, so do a
                //pass through the results to get the count:
                int registrationCount = 0;
                using (IExcelDataReader reader = GetDataReader(info, stream))
                {
                    if (!IsValid(reader))
                    {
                        SendErrorNotification(message);
                        return;
                    }
                    while (reader.Read())
                    {
                        registrationCount++;
                    }
                }

                //index is 1-based as may be published:
                int registrationIndex = 1; 
                using (IExcelDataReader reader = GetDataReader(info, stream))
                {
                    while (reader.Read())
                    {
                        //specify the queue so we can use BizTalk as a replacement subscriber:
                        Bus.Send<AddProduct>("ExcelUpload.AddProductService.1.InputQueue", m =>
                        {
                            m.BatchId = message.BatchId;
                            m.BatchSourcePath = message.BatchSourcePath;
                            m.RegistrationsInBatch = registrationCount;
                            m.RegistrationIndex = registrationIndex++;
                            m.OriginatorDestination = Bus.SourceOfMessageBeingHandled;

                            m.Name = (string)reader.GetString(0);
                            m.ProductNumber = (string)reader.GetString(1);
                            m.SafetyStockLevel = (int)reader.GetInt32(2);
                            m.ReorderPoint = (int)reader.GetInt32(3);
                            m.StandardCost = (decimal)reader.GetDecimal(4);
                            m.ListPrice = (decimal)reader.GetDecimal(5);
                            m.DaysToManufacture = (int)reader.GetInt32(6);
                            m.SellStartDate = DateTime.FromOADate(reader.GetDouble(7));
                        });
                    }
                }
                stream.Close();
            }
        }

        private bool IsValid(IExcelDataReader reader)
        {
            bool valid = true;
            //TODO - proper validation:
            valid = valid && reader.FieldCount == 8;
            return valid;
        }

        private IExcelDataReader GetDataReader(FileInfo info, FileStream stream)
        {
            IExcelDataReader reader = null;
            switch (info.Extension.ToLower())
            {
                case ".xls":
                    reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    break;
                case ".xlsx":
                    reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    break;
            }
            reader.IsFirstRowAsColumnNames = true;
            //clear the first row - column names:
            reader.Read(); 
            return reader;
        }

        private void SendErrorNotification(StartBatchUpload message)
        {
            BatchStatusChanged notification = new BatchStatusChanged
            {
                BatchId = message.BatchId,
                BatchSourcePath = message.BatchSourcePath,
                Status = BatchStatus.Errored
            };
            Bus.Send(Bus.SourceOfMessageBeingHandled, notification);
        }
    }
}
