using System.Collections.Generic;
using System.Linq;
using Amazon.SQS.Model;
using CloudServiceBus.Entities.ServiceResponses;

namespace CloudServiceBus.Core.Extensions
{
    public static class ReceiveMessageResponseExtensions
    {
        public static IEnumerable<T> MessagesAsResponse<T>(this ReceiveMessageResponse response)
            where T : ServiceResponse
        {
            var result = new List<T>();
            if (response.IsSetReceiveMessageResult() && response.ReceiveMessageResult.Message.Count > 0)
            {
                var receiveMessageResult = response.ReceiveMessageResult;
                foreach (Message message in receiveMessageResult.Message)
                {
                    result.Add(message.BodyAsResponse<T>());
                }
            }
            return result;
        }

        public static T FirstMessageAsResponse<T>(this ReceiveMessageResponse response)
            where T : ServiceResponse
        {
            var result = response.MessagesAsResponse<T>();
            return result.FirstOrDefault();
        }
    }
}
