using System;
using Amazon.SQS.Model;
using CloudServiceBus.Core.Serialization;
using CloudServiceBus.Entities.ServiceRequests;
using CloudServiceBus.Entities.ServiceResponses;

namespace CloudServiceBus.Core.Extensions
{
    public static class SendMessageRequestExtensions
    {
        public static void RequestToBody<T>(this SendMessageRequest message, T instance)
            where T : ServiceRequest
        {
            SetMessageBody(message, typeof(T), instance);
        }

        public static void ResponseToBody<T>(this SendMessageRequest message, T instance)
            where T : ServiceResponse
        {
            SetMessageBody(message, typeof(T), instance);
        }

        private static void SetMessageBody(SendMessageRequest message, Type type, object instance)
        {
            message.MessageBody = Serializer.Current.Serialize(type, instance).ToString();
        }

    }
}
