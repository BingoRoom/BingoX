using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace BingoX.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public static class EmptyHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static bool IsEmpty<TKey, TValue>(this IDictionary<TKey, TValue> dictionary)
        {
            return dictionary == null || dictionary.Count == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty(this IList list)
        {
            return list == null || list.Count == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool IsEmpty(this ICollection list)
        {
            return list == null || list.Count == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static bool IsEmpty(this IEnumerable enumerable)
        {
            return enumerable == null || !enumerable.GetEnumerator().MoveNext();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsEmpty<T>(this T[] array)
        {
            return array == null || array.Length == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static bool IsEmpty(this Array array)
        {
            return array == null || array.Length == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataTable dt)
        {
            return dt == null || dt.Rows.Count == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dv"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataView dv)
        {
            return dv == null || dv.Count == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool IsEmpty(this DataSet ds)
        {
            return ds == null || ds.Tables.Count == 0;
        }
    }
}
