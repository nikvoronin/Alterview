using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Alterview.Infrastructure;
using Alterview.Infrastructure.Data;
using Alterview.Infrastructure.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alterview.ImportService
{
    class Program
    {
        public const string DefaultConfigFilename = "appconfig.json";
        static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(Program));

        static IReadOnlyDictionary<string, string> DefaultConfigurationStrings { get; } =
            new Dictionary<string, string>
            {
                {"ConnectionString", @"Server=(localdb)\\mssqllocaldb;Database=LocalTestDatabase;Trusted_Connection=True;" },
                {"MessageQueue:Provider", "RabbitMq" },
                {"MessageQueue:Host", "localhost" },
                {"MessageQueue:Channel", "hare_mu001" },
                {"ChannelPool:MaxChannels", "20" },
            };

        static AppConfiguration GetConfiguration()
        {
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(DefaultConfigurationStrings)
                .AddJsonFile(DefaultConfigFilename, false)
                .Build();

            AppConfiguration appConfiguration = Configuration.Get<AppConfiguration>();
            return appConfiguration;
        }

        static void Main(string[] args)
        {
            AppConfiguration config = GetConfiguration();

            // var testSender = new RabbitSender(config.MessageQueue.Host, config.MessageQueue.ChannelName, sendIntervalMs: 10);

            Func<byte[], SportEvent> messageParser = b => JsonSerializer.Deserialize<SportEvent>(b.AsSpan());

            var dataReceiver = new RabbitReceiver<SportEvent>(
                config.MessageQueue.Host,
                config.MessageQueue.ChannelName,
                messageParser
                );

            var eventsRepo = new EventsRepository(config.ConnectionString);

            var channelPool = new EventsChannelPool(
                eventsRepo.CreateUpdateCommand(),
                config.ChannelPool.MaxChannels
                );
            
            var eventExchanger = new EventsExchange(dataReceiver, channelPool);

            eventExchanger.Start();
            dataReceiver.Start();
            // testSender.Start();

            WaitForEnter("Press Enter to stop service...");

            // testSender.Stop();
            dataReceiver.Stop();
            eventExchanger.Stop();

            WaitForEnter("Press Enter to close application...");


            void WaitForEnter(string message)
            {
                Console.WriteLine(message);
                Console.ReadLine();
            }
        }
    }
}
