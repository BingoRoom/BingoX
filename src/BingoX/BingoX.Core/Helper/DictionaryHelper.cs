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
        /// 从两个字典中找出相同的键，值却不同的项
        /// </summary>
        /// <param name="currentValues">当前字典</param>
        /// <param name="originalValues">历史字典</param>
        /// <returns></returns>
        public static IDictionary<string, object> GetChanges(this IDictionary<string, object> currentValues, IDictionary<string, object> originalValues)
        {
            var changeValues = new Dictionary<string, object>();
            foreach (var item in currentValues)
            {
                if (!originalValues.ContainsKey(item.Key))
                {
                    changeValues.Add(item.Key, item.Value);
                    continue;
                }
                var currentValue = item.Value;

                var originalValue = originalValues[item.Key];
                var codetype = Convert.GetTypeCode(currentValue);
                var originalCodetype = Convert.GetTypeCode(originalValue);
                if (codetype != originalCodetype)
                {
                    changeValues.Add(item.Key, item.Value);
                    continue;
                }
                switch (codetype)
                {
                    case TypeCode.Boolean:
                        if ((bool)currentValue != (bool)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Byte:
                        if ((byte)currentValue != (byte)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Char:
                        if ((char)currentValue != (char)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.DateTime:

                        if ((DateTime)currentValue != (DateTime)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.DBNull:
                        if (!Convert.IsDBNull(originalValue) || currentValue != null)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Decimal:

                        if ((decimal)currentValue != (decimal)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Double:
                        if ((double)currentValue != (double)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Empty:
                        if (currentValue != null)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Int16:
                        if ((short)currentValue != (short)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Int32:
                        if ((int)currentValue != (int)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Int64:

                        if ((long)currentValue != (long)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Object:
                        break;
                    case TypeCode.SByte:
                        if ((sbyte)currentValue != (sbyte)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.Single:
                        if ((float)currentValue != (float)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.String:
                        if (!string.Equals(currentValue, originalValue))

                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.UInt16:

                        if ((ushort)currentValue != (ushort)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.UInt32:

                        if ((uint)currentValue != (uint)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    case TypeCode.UInt64:

                        if ((ulong)currentValue != (ulong)originalValue)
                            changeValues.Add(item.Key, currentValue);
                        break;
                    default:
                        break;
                }

            }

            return changeValues;
        }
        /// <summary>
        /// 获取IDictionary字典对应键的值，并转型为T。
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
        /// 获取IDictionary字典对应键的值，并用","拼接。
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
        /// 获取IDictionary字典对应键的值，并转型为T，支持默认值。
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
        /// 获取NameValueCollection字典对应键的值，并转型为T，支持默认值
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
        /// 获取一个空字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Empty<TKey, TValue>()
        {
            return new Dictionary<TKey, TValue>();
        }

        /// <summary>
        /// 把一个字典的所有项以“{0}={1}{分隔符}”的格式打印出来
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string ToContent<TValue>(this IDictionary<string, TValue> source, char splitChar)
        {
            if (source == null || source.Count == 0) return string.Empty;
            TextWriter writer = new StringWriter();
            foreach (KeyValuePair<string, TValue> keyValuePair in source)
            {
                writer.WriteLine("{0}={1}{2}", keyValuePair.Key, keyValuePair.Value, splitChar);
            }
            string str = writer.ToString();
            writer.Dispose();
            return str;
        }
        /// <summary>
        /// 把两个字典合并
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source">源字典</param>
        /// <param name="trage">被合并字典</param>
        /// <param name="isReplace">字典项重复处理。true:替换源字典中的项目，false:保留原字典项目</param>
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
        /// 创建忽略KEY大小写的新字典
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static IDictionary<string, TValue> CreateDictionary<TValue>()
        {
            return new Dictionary<string, TValue>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 获取字典中指定Key的值，如果不存在则返回TValue的默认值。
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param> 
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueIgnoreCase<TValue>(this IDictionary<string, TValue> dict, string key)
        {
            if (!dict.HasAny()) return default(TValue);
            if (dict.ContainsKey(key)) return dict[key];
            var strkey = dict.Keys.FirstOrDefault(n => string.Equals(key, n, StringComparison.OrdinalIgnoreCase));
            return strkey != null ? dict[strkey] : default(TValue);
        }
        /// <summary>
        /// 获取字典中指定Key的值，并转型为TValue。如果不存在则返回TValue的默认值。
        /// </summary>
        /// <param name="dict"></param>
        /// <param name="key"></param> 
        /// <typeparam name="TValue"></typeparam>
        /// <returns></returns>
        public static TValue GetValueIgnoreCase<TValue>(this IDictionary<string, string> dict, string key)
        {
            if (!dict.HasAny()) return default(TValue);
            if (dict.ContainsKey(key)) return StringUtility.Cast<TValue>(dict[key]);
            var strkey = dict.Keys.FirstOrDefault(n => string.Equals(key, n, StringComparison.OrdinalIgnoreCase));
            return strkey != null ? StringUtility.Cast<TValue>(dict[strkey]).Value : default(TValue);
        }
        /// <summary>
        /// 移除值符合条件的字典项
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static IDictionary<TKey, TValue> Reomve<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key, Func<TValue, bool> func)
        {
            if (func != null && dict.ContainsKey(key) && func(dict[key])) dict.Remove(key);
            return dict;
        }

        /// <summary>
        /// 通过选择器向当前字典添加项，如项存在则忽略
        /// </summary>
        /// <param name="source"></param>
        /// <param name="keySelector"> </param>
        /// <param name="item"> </param>
        /// <returns></returns>
        public static void AddWithoutContains<TKey, TValue>(this IDictionary<TKey, TValue> source, Func<TValue, TKey> keySelector, TValue item)
        {
            if (source == null || keySelector == null) return;
            var obj = keySelector(item);
            if (source.ContainsKey(obj)) return;
            source.Add(obj, item);
        }

        /// <summary>
        /// 尝试向当前字典添加项，如项存在则忽略
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
        /// 尝试向当前字典添加项，如项存在则覆盖
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
                if (!dict.ContainsKey(key))
                {
                    dict.Add(key, value);
                }
                else
                {
                    dict[key] = value;
                }
            }
            return dict;
        }

        /// <summary>
        /// 获取字典项
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
        /// 获取字典项
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
        /// 获取字典项，如项不存在则向字典添加defaultFunc生成的项
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
        /// 获取字典项，如项不存在则向字典添加defaultFunc生成的项
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
        /// 把目标字典与当前字典合并
        /// </summary>
        /// <param name="dict">当前字典</param>
        /// <param name="values">目标字典</param>
        /// <param name="replaceExisted">合并规则；true：替换当前字典的值，false:不替换当前字典的值</param>
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
