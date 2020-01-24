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
        protected static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(ChannelPoolBase<T, TTag>));

        public const int MaxChannels = 20;

        protected Dictionary<TTag, IDataChannel<T>> _links;
        protected List<IDataChannel<T>> _pool;
        protected int _maxChannels = MaxChannels;
        protected Random _rnd = new Random(Environment.TickCount);

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

        protected abstract IDataChannel<T> CreateChannel { get; }

        protected void AddChannel(IDataChannel<T> channel, TTag tag)
        {
            _pool.Add(channel);
            _links.Add(tag, channel);
        }

        /// <summary>
        /// Find relevant channel for the give message or message tag
        /// </summary>
        /// <param name="tag">Tag assigned with message</param>
        /// <returns>DataChannel relevant to the given tag</returns>
        public abstract IDataChannel<T> FindRelevantChannel(T message, TTag tag);

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
