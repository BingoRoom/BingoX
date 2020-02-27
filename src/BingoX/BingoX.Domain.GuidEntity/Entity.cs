using BingoX.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Domain
{
    public abstract class BaseEntity : IGuidEntity<BaseEntity>
    {
        public Guid ID { get; set; }
    }

    public abstract class AuditBaseEntity : BaseEntity, IAuditCreated, IAuditModified
    {
        public virtual DateTime CreatedDate { get; set; }
        public virtual string Created { get; set; }
        public virtual DateTime ModifiedDate { get; set; }
        public virtual string Modified { get; set; }
    }
    public abstract class Aggregate : AuditBaseEntity, IAggregate<AuditBaseEntity, Guid>
    {
        private IGenerator<Guid> generator;
        protected IGenerator<Guid> Generator
        {
            get
            {
                if (generator == null) { generator = new GuidGenerator(1, 1); }
                return generator;
            }
        }
        public Aggregate()
        {

        }
        public Aggregate(IGenerator<Guid> generator) : this()
        {
            this.generator = generator;
        }
    }
    public abstract class AggregateRoot : AuditBaseEntity, IAggregateRoot<AuditBaseEntity, Guid>
    {
        private readonly object sync = new object();
        private readonly Lazy<Dictionary<string, MethodInfo>> registeredHandlers;
        private readonly Queue<IDomainEvent> uncommittedEvents = new Queue<IDomainEvent>();
        private IGenerator<Guid> generator;
        protected IGenerator<Guid> Generator
        {
            get
            {
                if (generator == null) { generator = new GuidGenerator(1, 1); }
                return generator;
            }
        }


        public AggregateRoot(IGenerator<Guid> generator) : this()
        {
            this.generator = generator;
        }
        public AggregateRoot()
        {
            registeredHandlers = new Lazy<Dictionary<string, MethodInfo>>(() =>
            {
                var registry = new Dictionary<string, MethodInfo>();
                var methodInfoList = from mi in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                                     let returnType = mi.ReturnType
                                     let parameters = mi.GetParameters()
                                     where mi.IsDefined(typeof(HandlesInlineAttribute), false) &&
                                     returnType == typeof(void) &&
                                     parameters.Length == 1 &&
                                     typeof(IDomainEvent).IsAssignableFrom(parameters[0].ParameterType)
                                     select new { EventName = parameters[0].ParameterType.FullName, MethodInfo = mi };

                foreach (var methodInfo in methodInfoList)
                {
                    registry.Add(methodInfo.EventName, methodInfo.MethodInfo);
                }

                return registry;
            });
        }




        protected void Raise<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent
        {
            lock (sync)
            {
                // 首先处理事件数据。
                this.HandleEvent(domainEvent);

                // 最后将事件缓存在“未提交事件”列表中。
                this.uncommittedEvents.Enqueue(domainEvent);
            }
        }

        private void HandleEvent<TDomainEvent>(TDomainEvent domainEvent)
            where TDomainEvent : IDomainEvent
        {
            var key = domainEvent.GetType().FullName;
            if (registeredHandlers.Value.ContainsKey(key))
            {
                registeredHandlers.Value[key].Invoke(this, new object[] { domainEvent });
            }
        }

        IGenerator<Guid> IAggregateRoot<AuditBaseEntity, Guid>.Generator => generator;
        void IAggregateRoot<AuditBaseEntity, Guid>.Raise<TDomainEvent>(TDomainEvent domainEvent)
        {
            Raise(domainEvent);
        }
    }
}
