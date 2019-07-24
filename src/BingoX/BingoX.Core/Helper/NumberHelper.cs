using System;

namespace BingoX.Helper
{
    public static class NumberHelper
    {
        /// <summary>
        /// 获取可空类型decimal的值
        /// </summary>
        /// <param name="d">可空类型decimal</param>
        /// <returns></returns>
        public static decimal GetValue(this decimal? d)
        {
            return d == null ? 0 : d.Value;
        }
        /// <summary>
        /// 获取可空类型decimal的值
        /// </summary>
        /// <param name="d">可空类型decimal</param>
        /// <param name="digits">四舍五入法保留小数位数</param>
        /// <returns></returns>
        public static decimal GetValue(this decimal? d, int digits)
        {
            if (digits < 0) throw new ArgumentException("digits不能小于0");
            return d == null ? 0 : Math.Round(d.Value, digits, MidpointRounding.AwayFromZero);
        }

        /// <summary>
        /// 获取decimal的值
        /// </summary>
        /// <param name="d">当前decimal</param>
        /// <param name="digits">四舍五入法保留小数位数</param>
        /// <returns></returns>
        public static decimal GetValue(this decimal d, int digits)
        {
            if (digits < 0) throw new ArgumentException("digits不能小于0");
            return Math.Round(d, digits, MidpointRounding.AwayFromZero);
        }
        /// <summary>
        /// 获取可空类型decimal的值,有小数点舍弃
        /// </summary>
        /// <param name="d">可空类型decimal</param> 
        /// <returns></returns>
        public static decimal GetCeiling(this decimal? d)
        {

            var value = d.GetValue();
            return Math.Ceiling(value);
        }
        /// <summary>
        /// 获取可空类型decimal的值,有小数点舍弃
        /// </summary>
        /// <param name="d">decimal</param> 
        /// <returns></returns>
        public static decimal GetCeiling(this decimal d)
        {
 
            return Math.Ceiling(d);
        }
        /// <summary>
        /// 获取可空类型decimal的值,有小数点取1并入整数
        /// </summary>
        /// <param name="d">可空类型decimal</param> 
        /// <returns></returns>
        public static decimal GetFloor(this decimal? d)
        {

            var value = d.GetValue();
            return Math.Floor(value);
        }

        /// <summary>
        /// 获取可空类型double的值
        /// </summary>
        /// <param name="d">可空类型double</param>
        /// <returns></returns>
        public static double GetValue(this double? d)
        {
            return d == null ? 0 : d.Value;
        }
        /// <summary>
        /// 获取可空类型double的值
        /// </summary>
        /// <param name="d">可空类型ddouble</param>
        /// <param name="digits">四舍五入法保留小数位数</param>
        /// <returns></returns>
        public static double GetValue(this double? d, int digits, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            if (digits < 0) throw new ArgumentException("digits不能小于0");
            return d == null ? 0 : Math.Round(d.Value, digits, midpoint);
        }
        /// <summary>
        /// 获取double的值
        /// </summary>
        /// <param name="d">当前double</param>
        /// <param name="digits">四舍五入法保留小数位数</param>
        /// <returns></returns>
        public static double GetValue(this double d, int digits, MidpointRounding midpoint = MidpointRounding.AwayFromZero)
        {
            if (digits < 0) throw new ArgumentException("digits不能小于0");
            return Math.Round(d, digits, midpoint);
        }
        /// <summary>
        /// 获取可空类型double的值,有小数点舍弃
        /// </summary>
        /// <param name="d">可空类型double</param> 
        /// <returns></returns>
        public static double GetCeiling(this double? d)
        {

            var value = d.GetValue();
            return Math.Ceiling(value);
        }
        /// <summary>
        /// 获取 double的值,有小数点舍弃
        /// </summary>
        /// <param name="d"> double</param> 
        /// <returns></returns>
        public static double GetCeiling(this double d)
        { 
            return Math.Ceiling(d);
        }
        /// <summary>
        /// 获取可空类型double的值,有小数点取1并入整数
        /// </summary>
        /// <param name="d">可空类型double</param> 
        /// <returns></returns>
        public static double GetFloor(this double? d)
        {
            var value = d.GetValue();
            return Math.Floor(value);
        }
        /// <summary>
        /// 获取double的值,有小数点取1并入整数
        /// </summary>
        /// <param name="d"> double</param> 
        /// <returns></returns>
        public static double GetFloor(this double d)
        {

            return Math.Floor(d);
        }
    }
}
