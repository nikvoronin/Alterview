using System;
using System.Collections.Generic;
using Alterview.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alterview.ImportService
{
    class Program
    {
        public const string DefaultConfigFilename = "appconfig.json";
        static ILogger logger = LogFactory.GetFactory.CreateLogger(typeof(Program));

        static IReadOnlyDictionary<string, string> DefaultConfigurationStrings { get; } =
            new Dictionary<string, string>
            {
                {"ConnectionString", @"Server=(localdb)\\mssqllocaldb;Database=LocalTestDatabase;Trusted_Connection=True;" },
                {"MessageQueue:Provider", "RabbitMq" },
                {"MessageQueue:Host", "localhost" },
                {"MessageQueue:Channel", "hare_mu001" },
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
            // var dataReceiver = new RabbitReceiver(config.MessageQueue.Host, config.MessageQueue.ChannelName);
            // var channelPool = new ChannelPool(config.ConnectionString);
            // var eventExchanger = new EventExchanger(dataReceiver, channelPool);

            // eventExchanger.Start();
            // dataReceiver.Start();
            // testSender.Start();

            WaitForEnter("Enter to stop...");

            // testSender.Stop();
            // dataReceiver.Stop();
            // eventExchanger.Stop();

            WaitForEnter("Enter to close application...");


            void WaitForEnter(string message)
            {
                Console.WriteLine(message);
                Console.ReadLine();
            }
        }
    }
}
