using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;
using Microsoft.Extensions.Logging;

namespace Alterview.Infrastructure.Entities
{
    /// <summary>
    /// ChannelPool for SportEvent with integer Id
    /// </summary>
    public class EventsChannelPool : ChannelPoolBase<SportEvent, int>
    {
        protected new static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(EventsChannelPool));

        public EventsChannelPool(
            IAsyncCommand<SportEvent> outputCommand,
            int maxChannels = MaxChannels
            ) : base(outputCommand, maxChannels) 
        { }

        protected override IDataChannel<SportEvent> CreateChannel => new DataChannel<SportEvent>(_outputCommand);

        protected IDataChannel<SportEvent> RandomChannel => _pool[_rnd.Next(0, _pool.Count)];

        public override IDataChannel<SportEvent> FindRelevantChannel(SportEvent message, int tag)
        {
            return FindRelevantChannel(tag);
        }

        public IDataChannel<SportEvent> FindRelevantChannel(int tag)
        {
            IDataChannel<SportEvent> channel = null;

            log.LogDebug("Searching for relevant channel with eventId:{0}", tag);

            if (!_links.TryGetValue(tag, out channel))
            {
                channel = (_pool.Count < _maxChannels) ? CreateChannel : RandomChannel;
                AddChannel(channel, tag);

                log.LogDebug("Founded channel {0} for tag {1}", channel.Id, tag);
            }

            return channel;
        }
    }
}
