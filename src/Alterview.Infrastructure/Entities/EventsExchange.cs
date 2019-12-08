using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Microsoft.Extensions.Logging;

namespace Alterview.Infrastructure.Entities
{
    public class EventsExchange
    {
        static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(EventsExchange));

        private readonly IExternalDataSource<SportEvent> _dataSource;
        private List<IDataChannel<SportEvent>> _pool;

        public EventsExchange(
            IExternalDataSource<SportEvent> dataSource,
            List<IDataChannel<SportEvent>> pool)
        {
            _dataSource = dataSource;
            _pool = pool;
        }

        public void Start()
        {
            if (_dataSource != null)
                _dataSource.MessageReceive += DataSource_MessageReceive;
        }

        public void Stop()
        {
            if (_dataSource != null)
                _dataSource.MessageReceive -= DataSource_MessageReceive;

            // TODO abort all channels
            // _pool.AbortAll();
        }

        private void DataSource_MessageReceive(object sender, SportEvent ev)
        {
            IDataChannel<SportEvent> channel = null;

            // TODO find relevant channel from list or pool
            //channel = _pool.FindRelevantChannel(ev, ev.EventId);
            if (channel != null)
            {
                bool done = channel.PushData(ev);

                log.LogDebug("Receve new event #{0} at @{1}", ev.EventId, Environment.TickCount);
            }
        }
    }
}
