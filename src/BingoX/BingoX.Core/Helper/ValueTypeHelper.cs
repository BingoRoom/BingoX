using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Helper
{
    public static class ValueTypeHelper
    {
        /// <summary>
        ///  在...之间
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsBetween<T>(this T value, T? min, T? max) where T : struct, IComparable
        {
            return Nullable.Compare(value, min) >= 0 && Nullable.Compare(value, max) <= 0;
        }

        /// <summary>
        ///  在...之间
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsBetween<T>(this T? value, T? min, T? max) where T : struct, IComparable
        {
            return Nullable.Compare(value, min) >= 0 && Nullable.Compare(value, max) <= 0;
        }


        /// <summary>
        ///  小于等于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparaValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool LessThan<T>(this T? value, T? comparaValue) where T : struct, IComparable
        {
            return Nullable.Compare(value, comparaValue) <= 0;
        }

        /// <summary>
        /// 大于等于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparaValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool GreatThan<T>(this T? value, T? comparaValue) where T : struct, IComparable
        {
            return Nullable.Compare(value, comparaValue) >= 0;
        }

        /// <summary>
        ///  在...之间
        /// </summary>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool IsBetween<T>(this T value, T min, T max) where T : IComparable
        {
            return value.CompareTo(min) >= 0 && value.CompareTo(max) <= 0;
        }

        /// <summary>
        ///  小于等于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparaValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool LessThan<T>(this T value, T comparaValue) where T : IComparable
        {
            return value.CompareTo(comparaValue) <= 0;
        }

        /// <summary>
        ///  小于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparaValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Less<T>(this T value, T comparaValue) where T : IComparable
        {
            return value.CompareTo(comparaValue) < 0;
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparaValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool Great<T>(this T value, T comparaValue) where T : IComparable
        {
            return value.CompareTo(comparaValue) > 0;
        }

        /// <summary>
        /// 大于
        /// </summary>
        /// <param name="value"></param>
        /// <param name="comparaValue"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool GreatThan<T>(this T value, T comparaValue) where T : IComparable
        {
            return value.CompareTo(comparaValue) >= 0;
        }
    }
}
