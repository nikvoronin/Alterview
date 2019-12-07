using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Alterview.ImportService
{
    class Program
    {
        public const string DefaultConfigFilename = "appconfig.json";

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

            var logFactory = LoggerFactory.Create(b => b
                .AddConsole()
                .SetMinimumLevel(LogLevel.Trace));

            var log = logFactory.CreateLogger(typeof(Program));

            // var testSender = new RabbitSender(config.MessageQueue.Host, config.MessageQueue.ChannelName, sendInterval: 10, logger: log);
            // var dataReceiver = new RabbitReceiver(config.MessageQueue.Host, config.MessageQueue.ChannelName, logger: log);
            // var channelPool = new ChannelPool(config.ConnectionString, logger: log);
            // var eventExchanger = new EventExchanger(dataReceiver, channelPool, logger: log);

            // eventExchanger.Start();
            // dataReceiver.Start();
            // autoSender.Start();

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
