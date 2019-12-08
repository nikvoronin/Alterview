using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Alterview.Infrastructure.Data
{
    /// <summary>
    /// RabbitMq consumer and message converter
    /// </summary>
    /// <typeparam name="T">Output message format</typeparam>
    public class RabbitReceiver<T> : IExternalDataSource<T>
    {
        static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(RabbitReceiver<T>));

        private string _host = "localhost";
        private string _queue = "rabbit";

        private IConnection _connection;
        private IModel _channel;
        private Func<byte[], T> _messageParser;
        private EventingBasicConsumer consumer;

        public readonly bool ManualAcknowledgement = false;

        /// <summary>
        /// Occured when new message received
        /// </summary>
        public event EventHandler<T> MessageReceive;

        /// <summary>
        /// Create a RabbitMq consumer. Then u should use Start to start it
        /// </summary>
        /// <param name="hostName">RabbitMq server host name or address</param>
        /// <param name="queueName">Queue channel name</param>
        /// <param name="messageParser">Message converter from default for RabbitMq byte array to the given type</param>
        /// <param name="manualAcknowledgement">Switch on|off manual acknowledgement mode</param>
        public RabbitReceiver(
            string hostName,
            string queueName,
            Func<byte[], T> messageParser,
            bool manualAcknowledgement = false)
        {
            _host = hostName;
            _queue = queueName;
            _messageParser = messageParser;
            ManualAcknowledgement = manualAcknowledgement;
        }

        /// <summary>
        /// Start RabbitMq consumer
        /// </summary>
        /// <returns>TRUE if consumer was successfully created</returns>
        public bool Start()
        {
            var factory = new ConnectionFactory() { HostName = _host };
            try
            {
                _connection = factory.CreateConnection();
            }
            catch
            {
                log.LogDebug("Cannot create RabbitMq connection");

                return false;
            }

            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: _queue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);

            consumer = new EventingBasicConsumer(_channel);
            consumer.Received += Consumer_Received;

            _channel.BasicConsume(
                queue: _queue,
                autoAck: false,
                consumer: consumer);

            return true;
        }

        private void Consumer_Received(object sender, BasicDeliverEventArgs ea)
        {
            var body = ea.Body;
            var message = Encoding.UTF8.GetString(body);

            try
            { // we do not trust to unknown funcs
                var export = _messageParser(ea.Body);

                if (export != null)
                {
                    if (!ManualAcknowledgement)
                    {
                        _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);

                        log.LogTrace("BasicAck tag#:{0}", ea.DeliveryTag);
                    }

                    log.LogDebug($"->{(ea.Redelivered ? "r" : ">")} Received @{Environment.TickCount}");

                    OnMessageReceive(export);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Stop RabbitMq channel listener
        /// </summary>
        public void Stop()
        {
            _connection?.Dispose();
        }

        private void OnMessageReceive(T message)
        {
            var evh = MessageReceive;
            evh?.Invoke(this, message);
        }

        /// <summary>
        /// Callback for deferred message acknowledge
        /// </summary>
        /// <param name="id">DeliveryTag of the message</param>
        /// <param name="status">Action for acknowledge: Ack - delete from queue, Reject - requeue, etc.</param>
        public void Acknowledge(ulong id, AckStatus status)
        {
            if (ManualAcknowledgement && !_channel.IsClosed)
            {
                try
                {
                    switch (status)
                    {
                        case AckStatus.Ack:
                            _channel.BasicAck(deliveryTag: id, multiple: false);
                            break;
                        default:
                            _channel.BasicReject(deliveryTag: id, requeue: true);
                            break;
                    }
                }
                catch { }
            }
        }
    }
}
