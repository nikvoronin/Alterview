using System;
using System.Collections.Generic;
using System.Text;
using Alterview.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Alterview.Infrastructure.Entities
{
    /// <summary>
    /// Pool of the data channels with tags
    /// </summary>
    /// <typeparam name="T">The type of the data message</typeparam>
    /// <typeparam name="TTag">The type of the message tag. Message tag may not equal to the message.Id</typeparam>
    public abstract class ChannelPoolBase<T, TTag>
    {
        private static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(ChannelPoolBase<T, TTag>));

        public const int MaxChannels = 20;

        private Dictionary<TTag, IDataChannel<T>> _links;
        private List<IDataChannel<T>> _pool;
        private int _maxChannels = MaxChannels;
        private Random _rnd = new Random(Environment.TickCount);

        protected IAsyncCommand<T> _outputCommand;

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

        /// <summary>
        /// Find relevant channel for the give message or message tag
        /// </summary>
        /// <param name="tag">Tag assigned with message</param>
        /// <returns>DataChannel relevant to the given tag</returns>
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

        /// <summary>
        /// Abort all background data channels
        /// </summary>
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
