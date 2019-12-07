using System;
using System.Collections.Generic;
using System.Text;

namespace Alterview.ImportService
{
    public class AppConfiguration
    {
        public string ConnectionString { get; set; }

        public ExternalDataSource MessageQueue { get; set; }
        public class ExternalDataSource
        {
            public string Provider { get; set; }
            public string Host { get; set; }
            public string ChannelName { get; set; }
        }
    }
}
