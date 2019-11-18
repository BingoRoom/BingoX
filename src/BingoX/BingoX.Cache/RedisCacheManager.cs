
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
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
        public bool CanUpdate { get; set; }
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
                                //  ResponseTimeout = 15000,
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
            Add(key, new CacheItem(key, item.Value, item.ExpirationMode, item.ExpirationTimeout));
        }
        public void Add(string key, CacheItem item)
        {
            if (Convert.GetTypeCode(item.Value) == TypeCode.DBNull) return;
            var buffer = CanUpdate ? JsonConvert.SerializeObject(item) : JsonConvert.SerializeObject(item.Value);

            TimeSpan? timespan = item.ExpirationTimeout;


            distributedCache.StringSetAsync(key, buffer, timespan, When.Always, CommandFlags.None).Wait();
        }
        public TItem Get<TItem>(string key)
        {
            var obj = Get(key, typeof(TItem));
            if (obj == null) return default(TItem);
            return (TItem)obj;


        }

        public TItem GetOrAdd<TItem>(string key, Func<string, CacheItem<TItem>> p)
        {

            var item = Get(key,typeof(TItem));
            if (item != null) return (TItem)item;


            var cacheItem = p(key);
            Add(key, cacheItem);
            return cacheItem.Value;
        }

        public void Remove(string key)
        {
            distributedCache.KeyDeleteAsync(key).Wait();
        }

        public object Get(string key,Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            RedisKey redisKey = key;
            var buffer = distributedCache.StringGetAsync(redisKey).Result;
            if (!buffer.HasValue) return default;

            try
            {
                if (CanUpdate)
                {
                    JsonTextReader reader = new JsonTextReader(new StringReader(buffer));
                     
                    string value = null;
                    ExpirationMode expirationMode = ExpirationMode.None;
                    TimeSpan ExpirationTimeout = new TimeSpan(0, 10, 0);
                    while (reader.Read())
                    {
                        switch (reader.Path)
                        {
                           
                            case "Value":
                                value = reader.ReadAsString();
                                break;
                            case "ExpirationMode":
                                expirationMode = (ExpirationMode)reader.ReadAsInt32();
                                break;
                            case "ExpirationTimeout":
                                ExpirationTimeout = TimeSpan.Parse(reader.ReadAsString());
                                break;
                            default:
                                break;
                        }
                    }
                    if (string.IsNullOrEmpty(value)) return null;
                   
                    var dto = JsonConvert.DeserializeObject(value, type);
                    if (expirationMode == ExpirationMode.Sliding)
                    {
                        Add(key, new CacheItem(key, dto, expirationMode, ExpirationTimeout));
                    }
                    return dto;
                }
                else
                {
                    return JsonConvert.DeserializeObject(buffer, type);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "序列化失败");
                return default;
            }


        }
    }
}
