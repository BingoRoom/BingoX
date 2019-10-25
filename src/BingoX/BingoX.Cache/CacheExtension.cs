
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
namespace BingoX.Cache
{
#if Standard
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration.Json;
    using Microsoft.Extensions.Configuration;
    using BingoX.Extensions;

    public static class CacheExtension
    {

        public static void AddCache(this IServiceCollection services, IConfiguration configuration)
        {


            var configFile = configuration.GetConfigurationByFileName("appsettings.json");
            string cacheType;
            configFile.TryGet("Cache:CacheType", out cacheType);
            switch (cacheType)
            {
                case "redis":
                    {
                        AddRedis(services, configFile);
                        return;
                    }
                default:
                    AddCacheManager(services);
                    break;
            }
        }


        static void AddRedis(IServiceCollection services, JsonConfigurationProvider configFile)
        {


            string connectionString;
            configFile.TryGet("Cache:RedisConnectionString", out connectionString);


            services.AddSingleton<ICacheManager, RedisCacheManager>(o =>
            {
                return new RedisCacheManager(connectionString, o.GetService<ILogger<RedisCacheManager>>());
            });


        }
        static void AddCacheManager(IServiceCollection services)
        {

            services.AddMemoryCache();
            services.AddSingleton<ICacheManager, MemoryCacheeManager>();
            services.AddDistributedMemoryCache();

        }

    }

#endif
}
