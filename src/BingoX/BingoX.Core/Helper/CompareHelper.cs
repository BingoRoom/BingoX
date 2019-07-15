using BingoX.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Helper
{
    /// <summary>
    /// 提供一个关于比较器的扩展方法
    /// </summary>
    public static class CompareHelper
    {
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
    }
}
