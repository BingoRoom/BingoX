namespace BingoX.Domain
{
    public interface IDomainEventHandler { }
    public interface IDomainEventHandler<T> : IDomainEventHandler where T : IDomainEvent
    {
        void HandleEvent(T eventData);
    }
}
