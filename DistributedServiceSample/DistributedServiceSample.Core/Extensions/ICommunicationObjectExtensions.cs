using System;
using System.ServiceModel;
using DistributedServiceSample.Core.Logging;

namespace DistributedServiceSample.Core.Extensions
{
    /// <summary>
    /// Extensions to <see cref="ICommunicationObject"/>
    /// </summary>
    public static class ICommunicationObjectExtensions
    {
        /// <summary>
        /// Safely closes the communication channel, aborting the channel if any exceptions are raised during the close
        /// </summary>
        /// <param name="channel"></param>
        public static void CloseSafely(this ICommunicationObject channel)
        {
            try
            {
                if (channel.State != CommunicationState.Faulted)
                {
                    channel.Close();
                }
                else
                {
                    channel.Abort();
                }
            }
            catch (Exception ex)
            {
                Log.Trace("{0} - exception raised in CloseSafely", channel.GetType().FullName);
                channel.Abort();
            }
        }
    }
}
