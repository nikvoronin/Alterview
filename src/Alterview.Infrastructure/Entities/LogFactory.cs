using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Alterview.Infrastructure
{
    public class LogFactory
    {
        public static ILoggerFactory GetFactory = LoggerFactory.Create(b => b
            .AddConsole()
            .SetMinimumLevel(LogLevel.Trace));

    }
}
