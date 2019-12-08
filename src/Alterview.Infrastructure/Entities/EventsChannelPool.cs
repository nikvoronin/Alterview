using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;

namespace Alterview.Infrastructure.Entities
{
    public class EventsChannelPool : ChannelPoolBase<SportEvent, int>
    {
        public EventsChannelPool(Func<SportEvent, int> sendDataCommand, int maxChannels = MaxChannels) : base(sendDataCommand, maxChannels) { }

        // TODO new DataChannel()
        protected override IDataChannel<SportEvent> CreateChannel => null;// new DataChannel<SportEvent>();
    }
}
