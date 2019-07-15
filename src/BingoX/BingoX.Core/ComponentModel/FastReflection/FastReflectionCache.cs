using System.Collections.Generic;
using System.Threading;

namespace BingoX.ComponentModel.FastReflection
{
    public abstract class FastReflectionCache<TKey, TValue> : IFastReflectionCache<TKey, TValue>
    {
        private Dictionary<TKey, TValue> m_cache = new Dictionary<TKey, TValue>();
        private object m_rwLock = new object();

        public TValue Get(TKey key)
        {
            TValue value = default(TValue);

            lock (m_rwLock)
            {
                bool cacheHit = this.m_cache.TryGetValue(key, out value);
                if (cacheHit) return value;
                if (!this.m_cache.TryGetValue(key, out value))
                {
                    try
                    {
                        value = this.Create(key);
                        this.m_cache[key] = value;
                    }
                    finally
                    {

                    }
                }
            }
            return value;
        }

        protected abstract TValue Create(TKey key);
    }
}
