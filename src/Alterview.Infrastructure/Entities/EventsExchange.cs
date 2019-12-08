using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Microsoft.Extensions.Logging;

namespace Alterview.Infrastructure.Entities
{
    /// <summary>
    /// Redirect messages from source to target through several data channels
    /// </summary>
    public class EventsExchange
    {
        private static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(EventsExchange));

        private readonly IExternalDataSource<SportEvent> _dataSource;
        private ChannelPoolBase<SportEvent, int> _pool;

        public EventsExchange(
            IExternalDataSource<SportEvent> dataSource,
            ChannelPoolBase<SportEvent, int> pool)
        {
            _dataSource = dataSource;
            _pool = pool;
        }

        /// <summary>
        /// Start exchange service
        /// </summary>
        public void Start()
        {
            if (_dataSource != null)
            {
                _dataSource.MessageReceive += DataSource_MessageReceive;
            }
        }

        /// <summary>
        /// Stop exchange service
        /// </summary>
        public void Stop()
        {
            if (_dataSource != null)
            {
                _dataSource.MessageReceive -= DataSource_MessageReceive;
            }

            _pool.AbortAll();
        }

        private void DataSource_MessageReceive(object sender, SportEvent ev)
        {
            IDataChannel<SportEvent> channel = null;

            channel = _pool.FindRelevantChannel(ev, ev.EventId);
            if (channel != null)
            {
                bool done = channel.PushData(ev);

                log.LogDebug("Receve new event #{0} at @{1}", ev.EventId, Environment.TickCount);
            }
        }
    }
}
