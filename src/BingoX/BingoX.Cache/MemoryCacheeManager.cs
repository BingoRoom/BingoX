
using System;
using Microsoft.Extensions.Caching.Memory;
namespace BingoX.Cache
{
    public class MemoryCacheeManager : ICacheManager
    {
        private readonly IMemoryCache memoryCache;

        public MemoryCacheeManager(IMemoryCache memoryCache)
        {
            this.memoryCache = memoryCache;
        }

        public void Add<TItem>(string key, CacheItem<TItem> item)
        {
            if (Convert.GetTypeCode(item.Value) == TypeCode.DBNull) return;
            var opt = new MemoryCacheEntryOptions();
            if (item.ExpirationMode == ExpirationMode.Absolute) opt.AbsoluteExpirationRelativeToNow = item.ExpirationTimeout;
            if (item.ExpirationMode == ExpirationMode.Sliding) opt.SlidingExpiration = item.ExpirationTimeout;

            memoryCache.Set(key, item.Value, opt);
        }

        public TItem Get<TItem>(string key)
        {
            var obj = memoryCache.Get(key);
            if (obj is TItem) return (TItem)obj;
            return default;
        }
        public void Add(string key, CacheItem item)
        {
            if (Convert.GetTypeCode(item.Value) == TypeCode.DBNull) return;
            var opt = new MemoryCacheEntryOptions();
            if (item.ExpirationMode == ExpirationMode.Absolute) opt.AbsoluteExpirationRelativeToNow = item.ExpirationTimeout;
            if (item.ExpirationMode == ExpirationMode.Sliding) opt.SlidingExpiration = item.ExpirationTimeout;

            memoryCache.Set(key, item.Value, opt);
        }
        public object Get(string key, Type type)
        {
            return memoryCache.Get(key);
        }

        public TItem GetOrAdd<TItem>(string key, Func<string, CacheItem<TItem>> p)
        {
            var obj = memoryCache.Get(key);
            if (obj is TItem) return (TItem)obj;
            var item = p(key);

            Add(key, item);
            return item.Value;

        }

        public void Remove(string key)
        {
            memoryCache.Remove(key);
        }
    }
}
