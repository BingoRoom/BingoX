using System;

namespace BingoX.Cache
{
    public class CacheAbleAttribute : Attribute
    {
        public CacheAbleAttribute()
        {
            CacheKeyPrefix = string.Empty;
            Expiration = 300;
            ExpirationMode = ExpirationMode.Default;
        }
        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public int Expiration { get; set; }
        /// <summary>
        /// 过期时间（秒）
        /// </summary>
        public ExpirationMode ExpirationMode { get; set; }

        /// <summary>
        /// Key值前缀
        /// </summary>
        public string CacheKeyPrefix { get; set; }

        public string CacheKeyName { get; set; }

    }
}
