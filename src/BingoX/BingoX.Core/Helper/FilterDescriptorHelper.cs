using BingoX.ComponentModel.Data;
using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace BingoX.Helper
{
    public static class FilterDescriptorHelper
    {
        public static TValue GetValue<TValue>(this FilterDescriptor[] filters, string name)
        {
            if (filters == null) return default(TValue);
            var str = filters.FirstOrDefault(m => String.Equals(m.Key, name, StringComparison.InvariantCultureIgnoreCase)).Value;
            if (string.IsNullOrEmpty(str))
                return default(TValue);
            return ObjectUtility.Cast<TValue>(str);
        }
        public static TValue GetFilterValue<TValue>(this QueryDescriptor query, string name)
        {
            if (query.Filters == null) return default(TValue);
            var filter = query.Filters.FirstOrDefault(m => String.Equals(m.Key, name, StringComparison.InvariantCultureIgnoreCase));

            return GetValue<TValue>(filter);
        }
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
            try
            {

                if (typeof(TValue).IsEnum)
                {
                    return (TValue)Enum.Parse(typeof(TValue), value, true);
                }
            }
            catch
            {

            }
            var newvalue = ObjectUtility.Cast<TValue>(item);

            return newvalue;
        }
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
