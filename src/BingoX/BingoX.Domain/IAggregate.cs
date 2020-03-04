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
    /// <summary>
    /// 表示一个领域聚合
    /// </summary>
    public interface IAggregate : IDomainEntry
    {
       
    }

    public class HandlesInlineAttribute : Attribute
    {

    }
}
