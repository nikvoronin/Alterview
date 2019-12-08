using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;
using Alterview.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace Alterview.Infrastructure.Entities
{
    public sealed class DataChannel<T> : IDataChannel<T>, IDisposable
    {
        private static ILogger log = LogFactory.GetFactory.CreateLogger(typeof(DataChannel<T>));
        
        // TODO to the config?
        const int MaxChannelElements = 4000;
        const int MaxAttempts = 3;

        private CancellationTokenSource _cts;
        private Channel<T> _channel;
        private IAsyncCommand<T> _repoCommand;
        public Thread Worker { get; private set; }

        public int Id { get; private set; }

        private static int _autoIncId = 0;
        private static int AutoIncId => ++_autoIncId;

        public DataChannel(IAsyncCommand<T> outputDataCommand)
        {
            if (outputDataCommand == null)
            {
                throw new ArgumentNullException("outputDataCommand");
            }

            Id = AutoIncId;
            _channel = Channel.CreateBounded<T>(MaxChannelElements);
            _repoCommand = outputDataCommand;
        }

        private void TryStart()
        {
            if (Worker != null)
            {
                return;
            }

            log.LogDebug("TryStart ChannelId {0}", Id);

            _cts = new CancellationTokenSource();

            Worker = new Thread(() => TaskWorker(_cts.Token));
            Worker.IsBackground = true;
            Worker.Start();
        }

        public bool PushData(T ev)
        {
            TryStart();

            bool done = _channel.Writer.TryWrite(ev);
            return done;
        }

        private async Task<int> UpdateRepository(T ev, CancellationToken token)
        {
            int rowsAffected = 0;
            for (int attemptNo = 0; attemptNo <= MaxAttempts; attemptNo++)
            {
                rowsAffected = await _repoCommand.ExecuteAsync(ev);

                if (rowsAffected > 0)
                {
                    break;
                }

                if (token != null && token.IsCancellationRequested)
                {
                    break;
                }
            }

            return rowsAffected;
        }

        private async void TaskWorker(CancellationToken token)
        {
            try
            {
                while (await _channel.Reader.WaitToReadAsync(token))
                {
                    if (_channel.Reader.TryRead(out T eve))
                    {
                        await UpdateRepository(eve, token);

                        log.LogDebug("UpdateRepository at Channel# {0}", Id);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                log.LogDebug("Channel# {0} task canceled", Id);
            }

            log.LogDebug("Channel# {0} task ended", Id);
        }

        public void Abort()
        {
            _cts?.Cancel();
        }

        public void Dispose()
        {
            Abort();
        }
    }
}
