using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Alterview.Infrastructure.Entities
{
    public abstract class ChannelPoolBase<T, TTag>
    {
        private static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(ChannelPoolBase<T, TTag>));

        public const int MaxChannels = 20;

        private Dictionary<TTag, IDataChannel<T>> _links;
        private List<IDataChannel<T>> _pool;
        protected IAsyncCommand<T> _outputCommand;
        private int _maxChannels = MaxChannels;
        private Random _rnd = new Random(Environment.TickCount);

        public ChannelPoolBase(IAsyncCommand<T> outputCommand, int maxChannels = MaxChannels)
        {
            if (outputCommand == null)
            {
                throw new ArgumentNullException();
            }

            _maxChannels = maxChannels;
            _outputCommand = outputCommand;

            _pool = new List<IDataChannel<T>>(_maxChannels);
            _links = new Dictionary<TTag, IDataChannel<T>>();
        }

        private IDataChannel<T> RandomChannel => _pool[_rnd.Next(0, _pool.Count)];
        protected abstract IDataChannel<T> CreateChannel { get; }

        public IDataChannel<T> FindRelevantChannel(TTag tag)
        {
            IDataChannel<T> channel = null;

            log.LogDebug("Searching for relevant channel with eventId:{0}", tag);

            if (!_links.TryGetValue(tag, out channel))
            {
                channel = (_pool.Count < _maxChannels) ? CreateChannel : RandomChannel;
                _pool.Add(channel);
                _links.Add(tag, channel);

                log.LogDebug("Founded channel {0} for tag {1}", channel.Id, tag);
            }

            return channel;
        }

        public void AbortAll()
        {
            foreach (var channel in _pool)
            {
                channel.Abort();
            }

            _pool.Clear();
            _links.Clear();
        }
    }
}
