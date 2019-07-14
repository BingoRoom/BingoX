using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;

namespace BingoX.Helper
{
    public static class DictionaryHelper
    {
        /// <summary>
        /// 获取IDictionary字典对应键的值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key">键</param>
        /// <returns>对应的泛型实例</returns>
        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key)) return default(T);
            return ObjectUtility.Cast<T>(dictionary[key]);
        }
        /// <summary>
        /// 获取IDictionary字典对应键的值
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <returns>数组连接字符串</returns>
        public static string GetValue(this IDictionary<string, string[]> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key)) return string.Empty;
            return string.Join(",", dictionary[key]);
        }
        /// <summary>
        /// 获取IDictionary字典对应键的值，支持默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns>对应的泛型实例或默认值</returns>
        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            if (!dictionary.ContainsKey(key)) return defaultValue;
            return ObjectUtility.Cast<T>(dictionary[key], defaultValue);
        }
        /// <summary>
        /// 获取NameValueCollection字典对应键的值，支持默认值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T GetValue<T>(this NameValueCollection collection, string key, T defaultValue)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            string value = collection.Get(key);
            if (value == null) return defaultValue;
            return ObjectUtility.Cast<T>(value, defaultValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Empty<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToContent<TValue>(this IDictionary<string, TValue> source)
        {
            if (source == null || source.Count == 0) return string.Empty;
            TextWriter writer = new StringWriter();
            foreach (KeyValuePair<string, TValue> keyValuePair in source)
            {
                writer.WriteLine("{0}={1}", keyValuePair.Key, keyValuePair.Value);

            }
            string str = writer.ToString();
            writer.Dispose();
            return str;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="trage"></param>
        /// <param name="isReplace"> </param>
        public static void TryAddOtherDictionaryItems<TKey, TValue>(this IDictionary<TKey, TValue> source, IDictionary<TKey, TValue> trage, bool isReplace = true)
        {
            if (source == null || trage == null) return;
            foreach (var item in trage)
            {
                if (source.ContainsKey(item.Key))
                {
                    if (isReplace)
                    {
                        source[item.Key] = item.Value;
                    }
                }
                else
                {
                    source.Add(item.Key, item.Value);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, TValue> CreateDictionary<TValue>()
        {
            return new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param> 
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueIgnoreCase<TValue>(this IDictionary<string, TValue> dict, string key)
        {
            if (!dict.HasAny())
                return default(TValue);
            if (dict.ContainsKey(key))
                return dict[key];
            var strkey = dict.Keys.FirstOrDefault(n => string.Equals(key, n, StringComparison.OrdinalIgnoreCase));
            return strkey != null ? dict[strkey] : default(TValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param> 
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueIgnoreCase<TValue>(this IDictionary<string, string> dict, string key)
        {
            if (!dict.HasAny())
                return default(TValue);
            if (dict.ContainsKey(key))
                return StringUtility.Cast<TValue>(dict[key]);
            var strkey = dict.Keys.FirstOrDefault(n => string.Equals(key, n, StringComparison.OrdinalIgnoreCase));
            return strkey != null ? StringUtility.Cast<TValue>(dict[strkey]).Value : default(TValue);
        }
        /// <summary>
        /// 移除，对象条件符合则移除
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Reomve<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue, bool> func)
        {
            if (func != null && dict.ContainsKey(key) && func(dict[key]))
                dict.Remove(key);
            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"> </param>
        /// <param name="item"> </param>
        /// <returns></returns>
        public static void AddWithoutContains<TKey, TValue>(this IDictionary<TKey, TValue> source, Func<TValue, TKey> keySelector, TValue item)
        {
            if (source == null || keySelector == null)
                return;
            var obj = keySelector(item);
            if (source.ContainsKey(obj))
                return;

            source.Add(obj, item);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> TryAdd<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict != null && dict.ContainsKey(key) == false)
                dict.Add(key, value);
            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AddOrReplace<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict != null)
            {
                if (dict.ContainsKey(key) == false)
                    dict.Add(key, value);
                else
                    dict[key] = value;
            }
            return dict;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
        {
            return GetValue(dict, key, default(TValue));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultFunc"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueOrdefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue> defaultFunc)
        {
            if (dict != null)
            {
                if (dict.ContainsKey(key))
                {
                    var value = dict[key];
                    return value;
                }
                if (defaultFunc != null)
                {
                    var value = defaultFunc();
                    dict[key] = value;
                    return value;
                }
            }
            return default(TValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultFunc"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueOrdefault<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TKey, TValue> defaultFunc)
        {
            if (dict != null)
            {
                if (dict.ContainsKey(key))
                {
                    var value = dict[key];
                    return value;
                }
                if (defaultFunc != null)
                {
                    var value = defaultFunc(key);
                    dict[key] = value;
                    return value;
                }
            }
            return default(TValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, TValue defaultValue)
        {
            return dict != null && dict.ContainsKey(key) ? dict[key] : defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="values"></param>
        /// <param name="replaceExisted"></param>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dict, IEnumerable<KeyValuePair<TKey, TValue>> values, bool replaceExisted)
        {
            if (dict != null)
            {
                foreach (var item in values.Where(item => dict.ContainsKey(item.Key) == false || replaceExisted))
                    dict[item.Key] = item.Value;
            }
            return dict;
        }
    }
}
