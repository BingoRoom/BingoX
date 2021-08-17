using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.Utility
{

    [Serializable]
    public class CompareException : Exception
    {
        public CompareException() { }
        public CompareException(string message) : base(message) { }
        public CompareException(string message, Exception inner) : base(message, inner) { }
        protected CompareException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
    /// <summary>
    /// 表示一个针对操作数比较的工具
    /// </summary>
    public static class CompareUtility
    {
        #region
        /// <summary>
        /// 比较两个字符串是否相等
        /// </summary>
        /// <param name="x">比较参数1</param>
        /// <param name="y">比较参数2</param> 
        /// <returns>比较结果</returns>
        public static bool Compare(string x, string y)
        {
            if (x == null && y == null) return true;
            if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y)) return true;
            var flag = string.Equals(x, y, StringComparison.OrdinalIgnoreCase);
            if (flag) return true;
            if (x == null || y == null) return false;
            return string.Equals(x.Trim(), y.Trim(), StringComparison.OrdinalIgnoreCase);
        }
        /// <summary>
        /// 比较两个字符串是否相等
        /// </summary>
        /// <param name="str1">比较参数1</param>
        /// <param name="str2">比较参数2</param>
        /// <param name="compare">比较规则</param>
        /// <returns>比较结果</returns>
        public static bool Compare(string str1, string str2, StringCompare compare = StringCompare.IgnoreCase)
        {
            if (string.IsNullOrWhiteSpace(str1) && string.IsNullOrWhiteSpace(str2)) return true;
            var tmpstr1 = StringUtility.RemoveSpace(str1);
            var tmpstr2 = StringUtility.RemoveSpace(str2);
            return string.Equals(tmpstr1, tmpstr2, compare == StringCompare.None ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 提供一个断言Func，从一个指定类型的枚举中获取第一个符合断言的实例
        /// </summary>
        /// <typeparam name="T">指定类型</typeparam>
        /// <param name="item">待比较字符串</param>
        /// <param name="source">指定类型的枚举</param>
        /// <param name="predicate">断言Func</param>
        /// <param name="compare">比较规则</param>
        /// <returns>符合断言的实例</returns>
        public static T FirstOrDefault<T>(string item, IEnumerable<T> source, Func<T, string> predicate, StringCompare compare = StringCompare.IgnoreCase) where T : class
        {
            if (!source.HasAny()) return default(T);
            return source.FirstOrDefault(s => Compare(item, predicate(s), compare));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool Compare(decimal x, decimal y)
        {
            return x.CompareTo(y) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool Compare(double x, decimal y)
        {
            return x.CompareTo((double)y) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool Compare(decimal x, double y)
        {
            return x.CompareTo((decimal)y) == 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool Compare(double x, double y)
        {
            return x.CompareTo(y) == 0;
        }

        #endregion
        /// <summary>
        /// 判断指定类型的泛型对象是否在指定的范围内
        /// </summary>
        /// <param name="x">指定类型的泛型对象</param>
        /// <param name="min">起</param>
        /// <param name="max">止</param>
        /// <typeparam name="T">指定类型</typeparam>
        /// <returns>判断结果</returns>
        /// <exception cref="CompareException">表示一个比对过程中发生的错误</exception>
        public static bool IsBetween<T>(this T? x, T min, T max) where T : struct, IComparable
        {
            if (x == null) return false;
            if (min.CompareTo(max) > 0) throw new CompareException("最小值大於最大值");
            var val = x.Value;
            return (val.CompareTo(min) >= 0 && val.CompareTo(max) <= 0);
        }
        #region decimal

        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal x, decimal y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }


        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, decimal y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal x, decimal? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, decimal? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        #endregion

        #region double

        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double x, double y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y, digth);
            return tmpx.CompareTo(tmpy) == 0;
        }

        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double? x, double y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double x, double? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">以小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double? x, double? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }

        #endregion

        #region double&decimal
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double x, decimal y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = (double)Math.Round(y, digth);
            return tmpx.CompareTo(tmpy) == 0;
        }
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double? x, decimal y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = (double)Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double x, decimal? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = (double)Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(double? x, decimal? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = (double)Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal x, double y, int digth)
        {
            var tmpx = Math.Round(x, digth);
            var tmpy = (decimal)Math.Round(y, digth);
            return tmpx.CompareTo(tmpy) == 0;
        }
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, double y, int digth)
        {
            if (x == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = (decimal)Math.Round(y, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal x, double? y, int digth)
        {
            if (y == null) return false;
            var tmpx = Math.Round(x, digth);
            var tmpy = (decimal)Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        /// <summary>
        /// 比较浮点值，以小数点后位数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="digth">小数点后位数</param>
        /// <returns></returns>
        public static bool Compare(decimal? x, double? y, int digth)
        {
            if (x == null) return false;
            if (y == null) return false;
            var tmpx = Math.Round(x.Value, digth);
            var tmpy = (decimal)Math.Round(y.Value, digth);
            return Compare(tmpx, tmpy);
        }
        #endregion
    }

    /// <summary>
    /// 表示一个字符比较规则的枚举
    /// </summary>
    public enum StringCompare
    {
        /// <summary>
        /// 完全比较
        /// </summary>
        None,
        /// <summary>
        /// 忽略大小写
        /// </summary>
        IgnoreCase,
    }
}
