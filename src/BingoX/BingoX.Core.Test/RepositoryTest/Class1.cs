using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoX.Core.Test.RepositoryTest
{
    class AggregateTest
    {

 
        public class OrderAggregateRoot : IAggregateRoot
        {
            void IAggregateRoot.Raise<TDomainEvent>(TDomainEvent domainEvent)
            {
                throw new NotImplementedException();
            }

            public void GetOrder(string orderId)
            {
                //OrderInfo= value
            }
            public virtual OrderAggregate OrderInfo { get; private set; }
            public virtual string OrderId { get; set; }
            public virtual BuyerInoAggregate Buyer { get; private set; }
            public virtual ExpressageAggregate Expressage { get; private set; }
            public virtual IList<ProductAggregate> Products { get; private set; }
            public virtual void Add1()
            {

            }
            public virtual void Add2()
            {

            }
            public virtual ExpressageAggregateRoot GetExpressageAggregate()
            {
                throw new NotImplementedException();
            }
            public virtual ExpressageAggregateRoot CretaeExpressageAggregate()
            {
                throw new NotImplementedException();
            }
        }

        public class ExpressageAggregateRoot : IAggregateRoot
        {

            void IAggregateRoot.Raise<TDomainEvent>(TDomainEvent domainEvent)
            {
                throw new NotImplementedException();
            }
            public static ExpressageAggregateRoot GetByExpressage(string orderId)
            {
                throw new NotImplementedException();
                //OrderInfo= value
            }

            public static ExpressageAggregateRoot GetByOrder(string orderId)
            {
                throw new NotImplementedException();
                //OrderInfo= value
            }
        }
        public class ProductAggregate : IAggregate, IDomainEntry
        {

        }
        public class ExpressageAggregate : IAggregate
        {

        }
        public class OrderAggregate : IAggregate
        {
            public string OrderNo { get; set; }
        }
        public class BuyerInoAggregate : IAggregate
        {

        }
    }
}
