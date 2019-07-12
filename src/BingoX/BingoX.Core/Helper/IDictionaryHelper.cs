using BingoX.Utility;
using System.Collections.Generic;

namespace BingoX
{
    public static class IDictionaryHelper
    {
        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key)) return default(T);
            return ObjectUtility.Cast<T>(dictionary[key]);
        }
        public static string GetValue(this IDictionary<string, string[]> dictionary, string key)
        {
            if (!dictionary.ContainsKey(key)) return string.Empty;
            return string.Join(",", dictionary[key]);
        }
        public static T GetValue<T>(this IDictionary<string, object> dictionary, string key, T defaultValue)
        {
            if (!dictionary.ContainsKey(key)) return defaultValue;
            return ObjectUtility.Cast<T>(dictionary[key], defaultValue);
        }
    }
}
