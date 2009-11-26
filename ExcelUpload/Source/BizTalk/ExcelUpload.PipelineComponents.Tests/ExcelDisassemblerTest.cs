using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Microsoft.BizTalk.Message.Interop;
using Winterdom.BizTalk.PipelineTesting;
using Microsoft.BizTalk.Component;
using ExcelUpload.PipelineComponents;
using ExcelUpload.PipelineComponents.Tests.Properties;

namespace Sixeyed.CacheAdapter.PipelineComponents.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ExcelDisassemblerTest
    {
        private struct Namespace
        {
            public const string CacheAdapter = "http://schemas.sixeyed.com/CacheAdapter/2009";
        }

        private ReceivePipelineWrapper _receivePipeline;
        private ReceivePipelineWrapper ReceivePipeline
        {
            get
            {
                if (this._receivePipeline == null)
                {
                    this._receivePipeline = PipelineFactory.CreateEmptyReceivePipeline();
                    ExcelDisassembler dasm = new ExcelDisassembler();
                    this._receivePipeline.AddComponent(dasm, PipelineStage.Disassemble);
                }
                return this._receivePipeline;
            }
        }

        public ExcelDisassemblerTest() { }

        [TestMethod]
        public void Execute_ConfiguredMessage()
        {
            MemoryStream stream = new MemoryStream(Resources.ProductUpload_Small);
            IBaseMessage inputMessage = MessageHelper.CreateFromStream(stream);
            inputMessage.Context.Write("ReceivedFileName", "http://schemas.microsoft.com/BizTalk/2003/file-properties", "ProductUpload_Small.xls");
            MessageCollection outputMessages = this.ReceivePipeline.Execute(inputMessage);
            Assert.AreEqual(1, outputMessages.Count);
            //XYZ message should configured for caching:
            IBaseMessage outputMessage = outputMessages[0];
        }
    }
}
