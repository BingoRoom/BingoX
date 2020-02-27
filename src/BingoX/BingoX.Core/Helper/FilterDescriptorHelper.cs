using BingoX.ComponentModel.Data;
using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BingoX.Helper
{
    /// <summary>
    /// 表示一个针对筛选描述符的辅助
    /// </summary>
    public static class FilterDescriptorHelper
    {
        /// <summary>
        /// 从FilterDescriptor[]中获取指定名称的从FilterDescriptor的值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="filters"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this FilterDescriptor[] filters, string name)
        {
            if (filters == null) return default(TValue);
            var str = filters.FirstOrDefault(m => String.Equals(m.Key, name, StringComparison.InvariantCultureIgnoreCase)).Value;
            if (string.IsNullOrEmpty(str))
                return default(TValue);
            return ObjectUtility.Cast<TValue>(str);
        }
        /// <summary>
        /// 从QueryDescriptor中获取指定名称的FilterDescriptor的值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        //public static TValue GetFilterValue<TValue>(this QueryDescriptor query, string name)
        //{
        //    if (query.Filters == null) return default(TValue);
        //    var filter = query.Filters.FirstOrDefault(m => String.Equals(m.Key, name, StringComparison.InvariantCultureIgnoreCase));
        //    return GetValue<TValue>(filter);
        //}
        /// <summary>
        /// 从FilterDescriptor获取值
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static TValue GetValue<TValue>(this FilterDescriptor filter)
        {
            return GetValue<TValue>(filter.Value);
        }
        private static TValue GetValue<TValue>(string value)
        {
            var item = value;
            if (string.IsNullOrEmpty(item)) return default(TValue);
            if (typeof(TValue) == typeof(DateTime) || typeof(TValue) == typeof(DateTime?))
            {
                DateTime dateTime;
                if (DateTime.TryParseExact(item, new[] { "yyyy-MM-dd", "yyyy-MM-dd HH:mm:ss" }, null, DateTimeStyles.None, out dateTime))
                {
                    object tobj = dateTime;
                    return (TValue)tobj;
                }
            }
          
            var newvalue = ObjectUtility.Cast<TValue>(item);
            return newvalue;
        }
        /// <summary>
        /// 从FilterDescriptor中获取可Split的值，并返回数组
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static TValue[] GetValues<TValue>(this FilterDescriptor filter)
        {
            if (string.IsNullOrEmpty(filter.Value)) return EmptyUtility<TValue>.EmptyArray;
            List<TValue> list = new List<TValue>();
            foreach (var item in filter.Value.Split(','))
            {
                var newvalue = GetValue<TValue>(item);
                if (Equals(newvalue, null) && typeof(TValue).IsClass) continue;
                list.Add(newvalue);
            }
            return list.ToArray();
        }
        /// <summary>
        /// 从QueryDescriptor中获取指定名称的FilterDescriptor，并且获取可Split的值，并返回数组
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="query"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static TValue[] GetFilterValues<TValue>(this QueryDescriptor query, string name)
        {
            if (query.Filters == null) return default(TValue[]);

            var str = query.Filters.FirstOrDefault(m => String.Equals(m.Key, name, StringComparison.InvariantCultureIgnoreCase)).Value;
            if (string.IsNullOrEmpty(str)) return default(TValue[]);
            List<TValue> list = new List<TValue>();
            foreach (var item in str.Split(','))
            {
                var newvalue = GetValue<TValue>(item);
                if (Equals(newvalue, null) && typeof(TValue).IsClass) continue;
                list.Add(newvalue);
            }
            return list.ToArray();
        }
    }
}
