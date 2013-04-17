using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace Sixeyed.CarValet.AzureQueueConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            SetConsoleColour();

            var connectionString = "Endpoint=sb://csug-apr13.servicebus.windows.net/;SharedSecretIssuer=owner;SharedSecretValue=Je4Dequ3M2IwBzLXoDE1IezfUpteQSbt5lX0rxIiChY=";
            var client = QueueClient.CreateFromConnectionString(connectionString, "password_reset", ReceiveMode.PeekLock);
            Console.WriteLine("Listening, 'x' to exit");

            while (true)
            {
                var message = client.Receive();
                if (message != null)
                {
                    Console.WriteLine("- Received message");
                    try
                    {
                        var serializer = new DataContractJsonSerializer(typeof(PasswordResetRequest));
                        var request = message.GetBody<PasswordResetRequest>(serializer);
                        Console.WriteLine("-- Password reset request for: {0}", request.EmailAddress);
                        SendMail(request);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-- Unknown message type. No action.");
                    }
                    finally
                    {
                        message.Complete();
                    }
                }
            }
        }

        private static Random _Random = new Random();
        private static void SetConsoleColour()
        {
            var colour = _Random.Next(1, 15);
            Console.ForegroundColor = (ConsoleColor)colour;
        }

        private static void SendMail(PasswordResetRequest request)
        {
            var message = new MailMessage("azurepasswordreset@carvalet.com", request.EmailAddress);
            message.Subject = "Your password has been reset";
            message.Body = "You will need to change it when you first log in";
            var smtp = new SmtpClient();
            smtp.SendAsync(message, null);
        }

        private struct Mq
        {
            public const string Host = "localhost";
            public const string Queue = "password_reset";
        }

        [DataContract]
        private class PasswordResetRequest
        {
            [DataMember]
            public string EmailAddress { get; set; }
        }
    }
}
