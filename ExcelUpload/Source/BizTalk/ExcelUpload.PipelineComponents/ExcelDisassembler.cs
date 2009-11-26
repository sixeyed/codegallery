using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using System.IO;
using Excel;

namespace ExcelUpload.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [System.Runtime.InteropServices.Guid("446C0430-AB86-4e8f-99F4-692B7C4C8D90")]
    public class ExcelDisassembler : PipelineComponentBase, IDisassemblerComponent
    {
        IExcelDataReader _reader;
        List<ContextProperty> _contextProperties;

        /// <summary>
        /// Gets the GUID for this component
        /// </summary>
        protected override Guid Guid
        {
            get { return new Guid("446C0430-AB86-4e8f-99F4-692B7C4C8D90"); }
        }

        public void Disassemble(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            //load up the Excel reader:
            Stream originalStream = pInMsg.BodyPart.GetOriginalDataStream();
            string fileName = (string) pInMsg.Context.Read("ReceivedFileName", "http://schemas.microsoft.com/BizTalk/2003/file-properties");
            _reader = GetDataReader(fileName, originalStream);

            //store the incoming context properties to write out again:
            _contextProperties = new List<ContextProperty>();
            for (int i = 0; i < pInMsg.Context.CountProperties; i++)
            {
                ContextProperty property = new ContextProperty();
                property.Value = pInMsg.Context.ReadAt(i, out property.Name, out property.Namespace);
                property.IsPromoted = pInMsg.Context.IsPromoted(property.Name, property.Namespace);
                _contextProperties.Add(property);
            }
        }

        public IBaseMessage GetNext(IPipelineContext pContext)
        {
            if (_reader == null)
            {
                return null;
            }

            if (!_reader.Read())
            {
                _reader.Dispose();
                _reader = null;
                return null;
            }

            StringBuilder xmlBuilder = new StringBuilder();
            xmlBuilder.Append(@"<?xml version=""1.0"" ?>");
            xmlBuilder.Append(@"<Messages xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns=""http://tempuri.net/ExcelUpload.Messages"">");
            xmlBuilder.Append(@"<AddProduct>");
            xmlBuilder.Append(@"<BatchId></BatchId>");
            xmlBuilder.Append(@"<RegistrationIndex></RegistrationIndex>");
            xmlBuilder.Append(@"<RegistrationsInBatch></RegistrationsInBatch>");
            xmlBuilder.Append(@"<BatchSourcePath></BatchSourcePath>");
            xmlBuilder.Append(@"<OriginatorDestination></OriginatorDestination>");
            xmlBuilder.AppendFormat(@"<Name>{0}</Name>", _reader.GetString(0));
            xmlBuilder.AppendFormat(@"<ProductNumber>{0}</ProductNumber>", _reader.GetString(1));
            xmlBuilder.AppendFormat(@"<SafetyStockLevel>{0}</SafetyStockLevel>", _reader.GetInt32(2));
            xmlBuilder.AppendFormat(@"<ReorderPoint>{0}</ReorderPoint>", _reader.GetInt32(3));
            xmlBuilder.AppendFormat(@"<StandardCost>{0}</StandardCost>", _reader.GetDecimal(4));
            xmlBuilder.AppendFormat(@"<ListPrice>{0}</ListPrice>", _reader.GetDecimal(5));
            xmlBuilder.AppendFormat(@"<DaysToManufacture>{0}</DaysToManufacture>", _reader.GetInt32(6));
            //write date in XSD format:
            xmlBuilder.AppendFormat(@"<SellStartDate>{0}</SellStartDate>", DateTime.FromOADate(_reader.GetDouble(7)).ToString("yyyy-MM-ddTHH:mm:ss.fffffff"));
            xmlBuilder.Append(@"</AddProduct>");
            xmlBuilder.Append(@"</Messages>");            

            byte[] data = Encoding.UTF8.GetBytes(xmlBuilder.ToString());
            MemoryStream memStream = new MemoryStream(data);

            IBaseMessagePart messagePart = pContext.GetMessageFactory().CreateMessagePart();
            messagePart.Data = memStream;
            messagePart.ContentType = "text/xml";
            messagePart.Charset = "utf-8";

            IBaseMessage outMsg = pContext.GetMessageFactory().CreateMessage();
            outMsg.AddPart("Body", messagePart, true);
            
            foreach (ContextProperty property in _contextProperties)
            {
                if (property.IsPromoted)
                    outMsg.Context.Promote(property.Name, property.Namespace, property.Value);
                else
                    outMsg.Context.Write(property.Name, property.Namespace, property.Value);
            }

            outMsg.Context.Promote("MessageType", "http://schemas.microsoft.com/BizTalk/2003/system-properties", "http://tempuri.net/ExcelUpload.Messages#Messages");

            return outMsg;
        }


        private IExcelDataReader GetDataReader(string fileName, Stream stream)
        {
            IExcelDataReader reader = null;
            fileName = fileName.ToLower();
            if (fileName.EndsWith(".xls"))
            {
                reader = ExcelReaderFactory.CreateBinaryReader(stream);
            }
            else if (fileName.EndsWith(".xlsx"))
            {
                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            }
            reader.IsFirstRowAsColumnNames = true;
            //clear the first row - column names:
            reader.Read();
            return reader;
        }

        private struct ContextProperty
        {
            public string Name;
            public string Namespace;
            public bool IsPromoted;
            public object Value;
        }
    }
}
