using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BingoX.Domain
{
    public interface IDomainEvent
    {
        Guid TraceID { get; }
        DateTime EventTime { get; }
        Exception Exception { get; set; }
        string Message { get; set; }

    }
    public interface IEventPublisher : IDisposable
    {
        void Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent;
    }

    public interface IEventSubscriber : IDisposable
    {
        void Subscribe();
    }
}
