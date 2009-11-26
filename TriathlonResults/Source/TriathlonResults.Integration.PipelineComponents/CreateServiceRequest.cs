using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Resources;
using System.Reflection;
using System.Diagnostics;
using System.Collections;
using System.ComponentModel;
using Microsoft.BizTalk.Message.Interop;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Component;
using Microsoft.BizTalk.Messaging;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Practices.ESB;
using TriathlonResults.Integration.PipelineComponents.Bases;
using ESBSimpleSamples.ServiceClient.Interfaces;
using TriathlonResults.PipelineComponents.Helpers;
using TriathlonResults.Central.ServiceRequests;

namespace TriathlonResults.Integration.PipelineComponents
{    
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    [System.Runtime.InteropServices.Guid("51F1C547-C1D2-4534-9750-1DA8C17075E3")]
    public class CreateServiceRequest : PipelineComponentBase
    {
        protected override Guid Guid
        {
            get { return new Guid("51F1C547-C1D2-4534-9750-1DA8C17075E3"); }
        }

        public override IBaseMessage Execute(IPipelineContext pc, IBaseMessage inmsg)
        {
            //write the itinerary to context, using the typed service request - 
            //assumes the message body is not to be altered:
            RecordResult serviceRequest = new RecordResult();
            PipelineComponentHelper.WriteItineraryToMessage(inmsg, serviceRequest);           
            return inmsg;
        }
    }
}
