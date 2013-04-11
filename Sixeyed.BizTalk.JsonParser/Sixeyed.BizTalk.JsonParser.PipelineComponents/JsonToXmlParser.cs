using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using interop = Microsoft.BizTalk.Component.Interop;

namespace Sixeyed.BizTalk.JsonParser.PipelineComponents
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [System.Runtime.InteropServices.Guid(ComponentGuid)]
    public class JsonToXmlParser : PipelineComponentBase, interop.IComponent
    {
        private static BTS.MessageType _MessageTypeProperty = new BTS.MessageType();
        private const string ComponentGuid = "B6A3C444-AD61-4AE5-BA4A-812A8CC69ABE";

        [Browsable(true)]
        [Description("Root Element Name")]
        public string RootElementName { get; set; }

        [Browsable(true)]
        [Description("Target Namespace")]
        public string TargetNamespace { get; set; }
        
        public override string Description
        {
            get { return "Parses JSON to XML"; }
        }

        public override void Load(IPropertyBag propertyBag, int errorLog)
        {
            try
            {
                object val = ReadPropertyBag(propertyBag, "TargetNamespace");
                if (val != null)
                {
                    TargetNamespace = (string)val;
                }
                val = ReadPropertyBag(propertyBag, "RootElementName");
                if (val != null)
                {
                    RootElementName = (string)val;
                }
            }
            catch (Exception ex)
            {
                string message = string.Format(CultureInfo.InvariantCulture, "IPersistPropertyBag.Load error: {0}", ex.ToString());
                Trace.WriteLine(message);
                throw;
            }
        }

        public override void Save(IPropertyBag propertyBag, bool clearDirty, bool saveAllProperties)
        {
            try
            {
                WritePropertyBag(propertyBag, "RootElementName", RootElementName);
                WritePropertyBag(propertyBag, "TargetNamespace", TargetNamespace);
            }
           catch (Exception ex)
            {
                string message = string.Format(CultureInfo.InvariantCulture, "IPersistPropertyBag.Save error: {0}", ex.ToString());
                Trace.WriteLine(message);
                throw;
            }
        }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            //read the incoming JSON stream:
            var inStream = pInMsg.BodyPart.GetOriginalDataStream();
            var json = string.Empty;
            using (var reader = new StreamReader(inStream))
            {
                json = reader.ReadToEnd();
            }

            //convert to XML:
            var document = JsonConvert.DeserializeXNode(json, "root");
            var output = new XElement(XName.Get(RootElementName, TargetNamespace));
            output.Add(document.Root.Descendants());

            //fix up the namespaces:
            XNamespace ns = TargetNamespace;
            foreach (var element in output.Descendants())
            {
                element.Name = ns.GetName(element.Name.LocalName);
                var attributes = element.Attributes().ToList();
                element.Attributes().Remove();
                foreach (XAttribute attribute in attributes)
                {
                    if (!attribute.IsNamespaceDeclaration)
                    {
                        element.Add(new XAttribute(attribute.Name.LocalName, attribute.Value));
                    }
                }
            }

            //write to output body stream:
            var outStream = new MemoryStream();
            var writer = new XmlTextWriter(outStream, Encoding.Default);
            output.WriteTo(writer);
            writer.Flush();
            outStream.Flush();
            outStream.Position = 0;
            pInMsg.BodyPart.Data = outStream;

            //promote the message type:
            pInMsg.Context.Promote(_MessageTypeProperty.Name.Name, _MessageTypeProperty.Name.Namespace, string.Format("{0}#{1}", TargetNamespace, RootElementName));

            return pInMsg;
        }
    }
}
