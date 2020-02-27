using BingoX.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Domain
{
    public interface IAggregateRoot : IAggregate
    {

        void Raise<TDomainEvent>(TDomainEvent domainEvent) where TDomainEvent : IDomainEvent;
    }

    public interface IAggregate
    {

    }

    public class HandlesInlineAttribute : Attribute
    {
    }
}
