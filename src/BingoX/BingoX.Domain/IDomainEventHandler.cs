namespace BingoX.Domain
{
    /// <summary>
    /// 表示一个领域事件处理
    /// </summary>
    public interface IDomainEventHandler { }
    /// <summary>
    /// 表示一个领域事件处理
    /// </summary>
    /// <typeparam name="T">领域事件类型</typeparam>
    public interface IDomainEventHandler<T> : IDomainEventHandler where T : IDomainEvent
    {
        void HandleEvent(T eventData);
    }
}
