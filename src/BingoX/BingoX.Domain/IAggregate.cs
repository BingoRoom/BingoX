using BingoX.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Domain
{
    public interface IAggregateRoot<T, TId> : IEntity<T, TId> where T : class
    {
        IGenerator<TId> Generator { get; }
        void Raise<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
    }

    public interface IAggregate<T, TId> : IEntity<T, TId> where T : class
    {

    }

    public class HandlesInlineAttribute : Attribute
    {
    }
}
