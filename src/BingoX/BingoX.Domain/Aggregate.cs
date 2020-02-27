using BingoX.Generator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.Domain
{

    public abstract class Aggregate : IAggregate
    {


    }
    static class RegisteredHandlers
    {
        readonly static IDictionary<Type, Dictionary<string, MethodInfo>> cache = new Dictionary<Type, Dictionary<string, MethodInfo>>();
        public static Lazy<Dictionary<string, MethodInfo>> GetRegisteredHandlers<T>(T obj) where T : IAggregateRoot
        {

            ///通过缓存取
            return new Lazy<Dictionary<string, MethodInfo>>(() =>
            {
                var type = obj.GetType();
                if (cache.ContainsKey(type)) return cache[type];

                var registry = new Dictionary<string, MethodInfo>();
                var methodInfoList = from mi in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
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
                cache.Add(type, registry); 
                return registry;
            });
        }
    }
    public abstract class AggregateRoot : IAggregateRoot
    {
        private readonly object sync = new object();
        private readonly Lazy<Dictionary<string, MethodInfo>> registeredHandlers;
        private readonly Queue<IDomainEvent> uncommittedEvents = new Queue<IDomainEvent>();

        public AggregateRoot()
        {
            registeredHandlers = RegisteredHandlers.GetRegisteredHandlers(this);
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

 
        void IAggregateRoot.Raise<TDomainEvent>(TDomainEvent domainEvent)
        {
            Raise(domainEvent);
        }
    }
}
