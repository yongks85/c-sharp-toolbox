using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Utilities
{
    public interface IConsumerProducer<T>
    {
        void AddProducer(Func<T> produce);
        void StartConsuming(Action<T> consume, uint consumers = 1);
        void SetChannelLimit(int limit);
        void ProduceComplete();
        void ForceStop();
    }

    public class ConsumerProducer<T> : IConsumerProducer<T>, IDisposable
    {
        private readonly CancellationTokenSource _cts;
        private Channel<T> _channel;
        private readonly List<Task> _consumers;
        private readonly List<Task> _producers;

        public ConsumerProducer()
        {
            _cts = new CancellationTokenSource();
            _channel = Channel.CreateBounded<T>(5);
            _consumers = new List<Task>();
            _producers = new List<Task>();
        }

        public void AddProducer(Func<T> produce)
        {
            _producers.Add(Task.Factory.StartNew(() => {
                _channel.Writer.TryWrite(produce()); 
            }));

        }

        public void StartConsuming(Action<T> consume, uint consumers = 1)
        {
            for (var i = 0; i < consumers; i++)
            {
                _consumers.Add(Task.Factory.StartNew(async () => {
                    while (await _channel.Reader.WaitToReadAsync(_cts.Token))
                        if (_channel.Reader.TryRead(out var data)) consume(data);
                }));
            }
        }

        public void SetChannelLimit(int limit) =>
            _channel = limit <= 0 ? Channel.CreateUnbounded<T>() : Channel.CreateBounded<T>(limit);

        public void ProduceComplete()
        {
            _channel.Writer.Complete();
            Task.WaitAll(_producers.ToArray());
            Task.WaitAll(_consumers.ToArray());
        }

        public void ForceStop() => _cts.Cancel();

        public void Dispose()
        {
            _producers.ForEach(p=> p.Dispose());
            _consumers.ForEach(c=> c.Dispose());
           _cts.Dispose();
        }

        //public void NotifyComplete(Action complete) => SpinWait.SpinUntil( () => _consumers.TrueForAll(t=> t.IsCompleted))
    }
}
