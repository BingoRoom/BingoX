using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BingoX.Domain
{
    /// <summary>
    /// 表示一个领域事件
    /// </summary>
    public interface IDomainEvent
    {
        Guid TraceID { get; }
        DateTime EventTime { get; }
        Exception Exception { get; set; }
        string Message { get; set; }

    }
    /// <summary>
    /// 表示一个领域事件发布者
    /// </summary>
    public interface IEventPublisher : IDisposable
    {
        void Publish<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
            where TEvent : IDomainEvent;
    }
    /// <summary>
    /// 表示一个领域事件订阅者
    /// </summary>
    public interface IEventSubscriber : IDisposable
    {
        void Subscribe();
    }
}
