using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NServiceBus;

namespace ExcelUpload.Messages
{
    [Recoverable]
    public class AddProduct : IMessage
    {
        public Guid BatchId { get; set; }
        public int RegistrationIndex { get; set; }
        public int RegistrationsInBatch { get; set; }
        public string BatchSourcePath { get; set; }
        public string OriginatorDestination { get; set; }

        public string Name { get; set; }
        public string ProductNumber { get; set; }
        public int SafetyStockLevel { get; set; }
        public int ReorderPoint { get; set; }
        public decimal StandardCost { get; set; }
        public decimal ListPrice { get; set; }
        public int DaysToManufacture { get; set; }
        public DateTime SellStartDate { get; set; }
    }
}
