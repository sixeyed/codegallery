using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.BizTalk.Message.Interop;
using System.IO;
using ESBSimpleSamples.ServiceClient.Interfaces;

namespace TriathlonResults.PipelineComponents.Helpers
{
    public static class PipelineComponentHelper
    {
        public static T GetContextProperty<T>(IBaseMessage message, string contextPropertyName)
        {
            object contextValue = message.Context.Read(contextPropertyName, "http://TriathlonResults.Central.Schemas/1.0/");
            T typedContextValue = default(T);
            if (typedContextValue is Enum)
            {
                string contextValueString = Convert.ToString(contextValue);
                if (!string.IsNullOrEmpty(contextValueString))
                {
                    typedContextValue = (T)Enum.Parse(typeof(T), contextValueString);
                }
            }
            else
            {
                try
                {
                    typedContextValue = (T)Convert.ChangeType(contextValue, typeof(T));
                }
                catch
                {
                }
            }
            return typedContextValue;
        }

        public static void WriteItineraryToMessage(IBaseMessage message, IXmlServiceRequest serviceRequest)
        {   
            //write the itinerary to context:
            message.Context.Write("Itinerary", "http://schemas.microsoft.biztalk.practices.esb.com/itinerary", serviceRequest.GetServiceRequestHeader());
        }

        public static void WriteServiceRequestToMessage(IBaseMessage message, IXmlServiceRequest serviceRequest)
        {
            WriteItineraryToMessage(message, serviceRequest);
            //and add the bodypart data:
            message.BodyPart.Data = serviceRequest.GetServiceRequestBody();
            message.BodyPart.Data.Position = 0;
        }
    }
}
