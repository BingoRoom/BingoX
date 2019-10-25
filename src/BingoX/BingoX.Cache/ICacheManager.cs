using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Cache
{
    public interface ICacheManager
    {

        //TItem Get<TItem>(object key);
        //TItem Set<TItem>(object key, TItem value);
        //TItem Set<TItem>(object key, TItem value, DateTimeOffset absoluteExpiration);
        //TItem Set<TItem>(object key, TItem value, TimeSpan absoluteExpirationRelativeToNow);
        void Remove(string key);
        TItem GetOrAdd<TItem>(string key, Func<string, CacheItem<TItem>> p);
        void Add<TItem>(string key, CacheItem<TItem> p);
        TItem Get<TItem>(string key);
    }
}
