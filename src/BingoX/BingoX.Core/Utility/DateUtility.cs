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
    }
}
