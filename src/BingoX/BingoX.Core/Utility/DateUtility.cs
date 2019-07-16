using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Utility
{
    public class DateUtility
    {
        /// <summary>
        /// Timestamp开始时间
        /// </summary>
        static readonly DateTime UnixTpStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));

        /// <summary>
        /// 是否为数据库可用时间
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static bool IsDbDateTime(DateTime value)
        {
            return ValueTypeHelper.IsBetween(value, System.Data.SqlTypes.SqlDateTime.MinValue.Value, System.Data.SqlTypes.SqlDateTime.MaxValue.Value);
        }

        /// <summary>
        /// 是否为润年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        public static bool IsLeapYear(int year)
        {
            if (year % 4 != 0)
                return false;
            if (year % 100 == 0)
                return year % 400 == 0;
            else
                return true;
        }
        /// <summary>
        /// 获取DateTime的Timestamp
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>精确到秒的Timestamp</returns>
        public static long ToUtp(DateTime dt)
        {
            TimeSpan toNow = dt - UnixTpStart;
            return (long)Math.Round(toNow.TotalSeconds);
        }
        /// <summary>
        /// 通过年、月、日获取对应的Timestamp
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns>精确到秒的Timestamp</returns>
        public static long ToUtp(int year, int month, int day)
        {
            return ToUtp(new DateTime(year, month, day));
        }
        /// <summary>
        /// 通过年、月、日、时、分、秒获取对应的Timestamp
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <returns>精确到秒的Timestamp</returns>
        public static long ToUtp(int year, int month, int day, int hour, int minute, int second)
        {
            return ToUtp(new DateTime(year, month, day, hour, minute, second));
        }
        /// <summary>
        /// 把Timestamp转为对应的DateTime
        /// </summary>
        /// <param name="tp">精确到秒的Timestamp</param>
        /// <returns>DateTime实例</returns>
        public static DateTime FromUtp(long tp)
        {
            return UnixTpStart + (new TimeSpan(tp * 10000000));
        }
        /// <summary>
        /// 当前时间的Timestamp
        /// </summary>
        public static long Now { get { return ToUtp(DateTime.Now); } }
    }
}
