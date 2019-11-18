using System;
using System.Reflection;
using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;

namespace BingoX.Cache
{
    public class CacheAbleInterceptor : IInterceptor
    {

        public CacheAbleInterceptor(ICacheManager cacheManager, ILogger<CacheAbleInterceptor> logger)
        {
            CacheManager = cacheManager;
            this.logger = logger;
        }
        public ICacheManager CacheManager { get; set; }
        private readonly ILogger<CacheAbleInterceptor> logger;



        public void Intercept(IInvocation invocation)
        {
            //  throw new NotImplementedException();

            CacheAbleAttribute attribute = invocation.Method.GetCustomAttribute<CacheAbleAttribute>();

            if (attribute == null || invocation.Method.ReturnType == typeof(void))
            {
                invocation.Proceed();
                return;
            }

            try
            {


                string cacheKey = KeyGenerator.GetCacheKeyName(attribute, invocation.Method, invocation.Arguments);

                object cacheValue = CacheManager.Get(cacheKey, invocation.Method.ReturnType);
                if (cacheValue != null)
                {
                    invocation.ReturnValue = cacheValue;
                    return;
                }
                invocation.Proceed();

                CacheManager.Add(cacheKey, new CacheItem(cacheKey, invocation.ReturnValue, attribute.ExpirationMode, TimeSpan.FromSeconds(attribute.Expiration)));
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "获取缓存失败");
            }
        }



    }
}
