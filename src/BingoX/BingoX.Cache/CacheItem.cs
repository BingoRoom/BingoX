using System;
using System.Text;
using System.Reflection;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace BingoX.Cache
{
    public class CacheItem
    {
        public CacheItem(string key, object data, ExpirationMode expirationMode, TimeSpan timeout)
        {
            Key = key;
            Value = data;

            ExpirationMode = expirationMode;
            ExpirationTimeout = timeout;
        }
        public virtual object Value { get; set; }
        public string Key { get; protected set; }
        public ExpirationMode ExpirationMode { get; protected set; }
        public TimeSpan ExpirationTimeout { get; protected set; }
    }
    public class CacheItem<TItem> : CacheItem
    {



        public CacheItem(string key, TItem data, ExpirationMode expirationMode, TimeSpan timeout) : base(key, data, expirationMode, timeout)
        {

        }

        public new TItem Value
        {
            get { return (TItem)base.Value; }
            private set { base.Value = value; }
        }
    }
}
