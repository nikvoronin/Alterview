using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Alterview.Core.Models;
using Alterview.Infrastructure;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Alterview.ImportService.Tests
{
    public class RabbitSender
    {
        static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(RabbitSender));

        string _host = "localhost";
        string _queue = "rabbit";
        int _interval = 1000;

        CancellationTokenSource cts;
        Task _senderTask;

        public RabbitSender(string hostName, string queueName, int sendIntervalMs = 1000)
        {
            _host = hostName;
            _queue = queueName;
            _interval = sendIntervalMs;
        }

        public void Start()
        {
            cts = new CancellationTokenSource();
            _senderTask = new Task(() => SenderTask(cts.Token));
            _senderTask.Start();
        }

        private void SenderTask(CancellationToken cancelToken)
        {
            var factory = new ConnectionFactory() 
            { 
                HostName = _host 
            };

            using var connection = factory.CreateConnection();
            
            log.LogInformation("Connected to AMQP broker");
            
            using var channel = connection.CreateModel();
            channel.QueueDeclare(
                queue: _queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            log.LogInformation("Channel open");

            long packetId = 0;
            var rnd = new Random(Environment.TickCount);
            while (!cancelToken.IsCancellationRequested)
            {
                int eventId = rnd.Next(1, 40);
                int sportId = rnd.Next(149, 186);

                var ev = new SportEvent()
                {
                    EventId = eventId,
                    SportId = sportId,
                    EventName = $"TEST {eventId}",
                    EventDate = DateTime.Now,
                    Team1Price = (decimal)rnd.NextDouble(),
                    DrawPrice = (decimal)rnd.NextDouble(),
                    Team2Price = (decimal)rnd.NextDouble(),
                };

                string message = JsonSerializer.Serialize(ev);
                var body = Encoding.UTF8.GetBytes(message);

                packetId++;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: _queue,
                    basicProperties: null,
                    body: body);

                log.LogTrace("<<- Sent {0} #{1} @{2}", message, packetId, Environment.TickCount);

                Thread.Sleep(_interval < 0 ? 0 : _interval);
            }
        }

        public void Stop()
        {
            cts?.Cancel();
            _senderTask?.Wait();
        }
    }
}
