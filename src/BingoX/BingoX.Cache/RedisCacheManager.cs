
using System;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
namespace BingoX.Cache
{
#if Standard
    using Microsoft.Extensions.Configuration;
#endif


    public class RedisCacheManager : ICacheManager
    {


        public RedisCacheManager(string connectionString, ILogger<RedisCacheManager> logger)
        {
            this.connectionString = connectionString;
            this.logger = logger;
        }

        private readonly string connectionString;
        private readonly ILogger<RedisCacheManager> logger;
        readonly object Locker = new object();
        IConnectionMultiplexer _connMultiplexer;
        IConnectionMultiplexer ConnectionRedisMultiplexer
        {
            get
            {
                if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
                {
                    lock (Locker)
                    {
                        if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
                        {
                            var config = new ConfigurationOptions
                            {
                                AbortOnConnectFail = false,
                                // AllowAdmin = true,
                                ConnectTimeout = 15000,
                                SyncTimeout = 5000,
                                ResponseTimeout = 15000,
                                //Password = "Pwd",//Redis数据库密码
                                EndPoints = { connectionString }// connectionString 为IP:Port 如”192.168.2.110:6379”
                            };
                            _connMultiplexer = ConnectionMultiplexer.ConnectAsync(config).Result;
                        }
                    }
                }

                return _connMultiplexer;
            }
        }
        IDatabase distributedCache
        {
            get
            {
                return ConnectionRedisMultiplexer.GetDatabase();
            }
        }
        public void Add<TItem>(string key, CacheItem<TItem> item)
        {
            if (Convert.GetTypeCode(item.Value) == TypeCode.DBNull) return;
            var buffer = (Newtonsoft.Json.JsonConvert.SerializeObject(item.Value));
            var opt = new Microsoft.Extensions.Caching.Distributed.DistributedCacheEntryOptions();
            TimeSpan? timespan = item.ExpirationTimeout;
            //    if (item.ExpirationMode == ExpirationMode.Absolute) timespan = item.ExpirationTimeout;
            //     if (item.ExpirationMode == ExpirationMode.Sliding) timespan = item.ExpirationTimeout;

            distributedCache.StringSetAsync(key, buffer, timespan, When.Always, CommandFlags.None).Wait();
        }

        public TItem Get<TItem>(string key)
        {
            RedisKey redisKey = key;
            var buffer = distributedCache.StringGetAsync(redisKey).Result;
            if (!buffer.HasValue) return default;

            try
            {
                var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<TItem>(buffer);
                return dto;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "序列化失败");
                return default;
            }




        }

        public TItem GetOrAdd<TItem>(string key, Func<string, CacheItem<TItem>> p)
        {
            RedisKey redisKey = key;
            var buffer = distributedCache.StringGetAsync(redisKey).Result;
            if (buffer.HasValue)
            {
                try
                {
                    var dto = Newtonsoft.Json.JsonConvert.DeserializeObject<TItem>(buffer);
                    return dto;
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "序列化失败");
                    return default;

                }


            }

            var item = p(key);
            Add(key, item);
            return item.Value;
        }

        public void Remove(string key)
        {
            distributedCache.KeyDeleteAsync(key).Wait();
        }
    }
}
