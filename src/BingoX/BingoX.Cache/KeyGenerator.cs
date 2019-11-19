using System.Text;
using System.Reflection;
using System;

namespace BingoX.Cache
{
    public class KeyGenerator
    {
        public static string GetCacheKey(MethodInfo methodInfo, object[] args, string prefix)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append($"{prefix}_");
            cacheKey.Append(methodInfo.DeclaringType.Name).Append($"_{methodInfo.Name}");
            foreach (var item in args)
            {
                cacheKey.Append($"_{item}");
            }
            return cacheKey.ToString();
        }
        public static string GetCacheKeyName(object[] args, string keyname)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append($"{keyname}");

            foreach (var item in args)
            {
                cacheKey.Append($"_{item}");
            }
            return cacheKey.ToString();
        }


        public static string GetCacheKeyPrefix(MethodInfo methodInfo, string prefix)
        {
            StringBuilder cacheKey = new StringBuilder();
            cacheKey.Append(prefix);
            cacheKey.Append($"_{methodInfo.DeclaringType.Name}").Append($"_{methodInfo.Name}");
            return cacheKey.ToString();
        }

        public static string GetCacheKeyName(CacheAbleAttribute attribute, MethodInfo methodInfo, object[] args)
        {
            if (attribute.GeneratorType == KeyGeneratorType.Parameters)
            {
                if (!string.IsNullOrEmpty(attribute.CacheKeyName))
                {
                    return GetCacheKeyName(args, attribute.CacheKeyName);
                }
                return GetCacheKey(methodInfo, args, attribute.CacheKeyPrefix);
            }
            if (string.IsNullOrEmpty(attribute.CacheKeyName) && string.IsNullOrEmpty(attribute.CacheKeyPrefix)) throw new Exception("没有配置相关 CacheKeyName CacheKeyPrefix");
            if (!string.IsNullOrEmpty(attribute.CacheKeyName))
            {
                return attribute.CacheKeyName;
            }

            return GetCacheKeyPrefix(methodInfo, attribute.CacheKeyPrefix);
        }
    }
}
