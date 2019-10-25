using System;

namespace BingoX.Cache
{
    public class CacheItem<TItem>
    {


        public CacheItem(string key, TItem data, ExpirationMode expirationMode, TimeSpan timeout)
        {
            Key = key;
            Value = data;
            ExpirationMode = expirationMode;
            ExpirationTimeout = timeout;
        }

        public string Key { get; private set; }
        public TItem Value { get; private set; }
        public ExpirationMode ExpirationMode { get; private set; }
        public TimeSpan ExpirationTimeout { get; private set; }
    }
}
