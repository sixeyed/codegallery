using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace Sixeyed.CarValet.AmqpConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            SetConsoleColour();
            var factory = new ConnectionFactory();
            factory.HostName = Mq.Host;
            using (var connection = factory.CreateConnection())
            {
                using (IModel channel = connection.CreateModel())
                {
                    channel.QueueDeclare(Mq.Queue, true, false, false, null); //durable
                    channel.BasicQos(0, 1, false); //max one message per worker, new msg not delivered till ACK received
                    var consumer = new QueueingBasicConsumer(channel);
                    channel.BasicConsume(Mq.Queue, false, consumer);
                    Console.WriteLine("** Password reset listening on queue: {0} **", Mq.Queue);
                    while (true)
                    {
                        var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();
                        Console.WriteLine("- Received message");
                        try
                        {
                            var stream = new MemoryStream(ea.Body);
                            var serializer = new DataContractJsonSerializer(typeof(PasswordResetRequest));
                            var request = (PasswordResetRequest)serializer.ReadObject(stream);
                            Console.WriteLine("-- Password reset request for: {0}", request.EmailAddress);
                            SendMail(request);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("-- Unknown message type. No action.");
                        }
                        finally
                        {
                            channel.BasicAck(ea.DeliveryTag, false);
                        }
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
            var message = new MailMessage("passwordreset@carvalet.com", request.EmailAddress);
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
