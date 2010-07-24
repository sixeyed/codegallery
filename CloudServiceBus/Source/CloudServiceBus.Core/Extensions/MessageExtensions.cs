using System;
using Amazon.SQS.Model;
using CloudServiceBus.Core.Serialization;
using CloudServiceBus.Entities.ServiceRequests;
using CloudServiceBus.Entities.ServiceResponses;

namespace CloudServiceBus.Core.Extensions
{
    public static class MessageExtensions
    {
        public static T BodyAsRequest<T>(this Message message)
            where T : ServiceRequest
        {
            Type type = typeof(T);
            var instance = GetObject(type, message.Body) as T;
            if (instance != null && instance.ServiceRequestName != type.Name)
            {
                instance = null;
            }
            return instance;
        }

        public static T BodyAsResponse<T>(this Message message)
            where T : ServiceResponse
        {
            Type type = typeof(T);
            var instance = GetObject(type, message.Body) as T;
            if (instance != null && instance.ServiceResponseName != type.Name)
            {
                instance = null;
            }
            return instance;
        }

        private static object GetObject(Type type, string serialized)
        {
            object instance = null;
            try
            {
                instance = Serializer.Current.Deserialize(type, serialized);                
            }
            catch (Exception ex)
            {
                //TODO - logging
            }
            return instance;
        }
    }
}
