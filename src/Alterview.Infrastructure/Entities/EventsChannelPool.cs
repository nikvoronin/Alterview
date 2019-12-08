using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Alterview.Core.Models;

namespace Alterview.Infrastructure.Entities
{
    public class EventsChannelPool : ChannelPoolBase<SportEvent, int>
    {
        public EventsChannelPool(IAsyncCommand<SportEvent> outputCommand, int maxChannels = MaxChannels) : base(outputCommand, maxChannels) { }

        protected override IDataChannel<SportEvent> CreateChannel => new DataChannel<SportEvent>(_outputCommand);
    }
}
