using System.ServiceModel.Channels;
using System.Xml;

namespace Sixeyed.WcfRestErrorHandler.ServiceModel.Messages
{
    /// <summary>
    /// Response message which contains a friendly HTML description of a fault
    /// </summary>
    public class HtmlFaultMessage : Message
    {
        private MessageHeaders _headers;
        private MessageProperties _properties;

        public string Message {get;  set;}

        public HtmlFaultMessage() 
        {
            _headers = new MessageHeaders(MessageVersion.None);
            _properties = new MessageProperties();
        }

        public override MessageHeaders Headers
        {
            get { return this._headers; }
        }

        public override MessageProperties Properties
        {
            get { return this._properties; }
        }

        public override MessageVersion Version
        {
            get { return MessageVersion.None; }
        }

        protected override void OnWriteBodyContents(XmlDictionaryWriter writer)
        {
            writer.WriteStartElement("html");
            writer.WriteStartElement("head");
            writer.WriteElementString("title", "Request Failed");
            writer.WriteRaw(@"<style>BODY { color: #000000; background-color: white; font-family: Verdana; margin-left: 0px; margin-top: 0px; } #content { margin-left: 30px; font-size: .70em; padding-bottom: 2em; } A:link { color: #336699; font-weight: bold; text-decoration: underline; } A:visited { color: #6699cc; font-weight: bold; text-decoration: underline; } A:active { color: #336699; font-weight: bold; text-decoration: underline; } h1 { background-color: #003366; border-bottom: #336699 6px solid; color: #ffffff; font-family: Tahoma; font-size: 26px; font-weight: normal;margin: 0em 0em 10px -20px; padding-bottom: 8px; padding-left: 30px;padding-top: 16px;} pre { font-size:small; background-color: #e5e5cc; padding: 5px; font-family: Courier New; margin-top: 0px; border: 1px #f0f0e0 solid; white-space: pre-wrap; white-space: -pre-wrap; word-wrap: break-word; } table { border-collapse: collapse; border-spacing: 0px; font-family: Verdana; font-size: 1em;} table th { border-right: 2px white solid; border-bottom: 2px white solid; font-weight: bold; background-color: #cecf9c;} table td { border-right: 2px white solid; border-bottom: 2px white solid; background-color: #e5e5cc;}</style>");
            writer.WriteEndElement(); //head
            writer.WriteStartElement("body");
            writer.WriteRaw("<div id='content'>");

            writer.WriteElementString("h1", "Request Failed");
            writer.WriteElementString("h3", Message);

            writer.WriteRaw("</div>");
            writer.WriteEndElement(); //body
            writer.WriteEndElement(); //html
        }
    }
}
