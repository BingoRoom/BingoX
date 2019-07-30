using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace BingoX.Services
{

    public interface IDateTimeService
    {
        DateTime GetNow();
        DateTime GetCurrentYearFirstDate();
        DateTime GetCurrentYearLastDate();
        DateTime GetCurrentMonthFirstDate();
        DateTime GetCurrentMonthLastDate();

        DateTime? TryParse(string str);
        DateTime Parse(string str);

        TimeSpan? TimeTryParse(string str);
        TimeSpan TimeParse(string str);
        string ToDate(DateTime? dateTime);
        string ToTime(DateTime? dateTime);
        string ToTime(TimeSpan? dateTime);
        string ToDateTime(DateTime? dateTime);
    }
    public class DateTimeService : IDateTimeService
    {
        readonly CultureInfo culture = new CultureInfo("zh-hans");
        readonly static string[] DatetimeFormatstrings = { "yyyy-MM-dd HH:mm:ss", "yyyyMMddHHmmss", "yyyy-MM-dd HH:mm", "yyyyMMddHHmm", "yyyy-MM-dd", "yyyyMMdd", };
        readonly static string[] TimeFormatstrings = { "hh:mm:ss", "hhmmss", "hh:mm", "hhmm", };
        public static readonly DateTimeService Default = new DateTimeService();
        public virtual DateTime GetCurrentYearFirstDate()
        {
            var now = GetNow();
            return new DateTime(now.Year, 1, 1);
        }
        public virtual DateTime GetCurrentYearLastDate()
        {
            var now = GetNow();
            return new DateTime(now.Year, 12, 31);
        }
        public virtual DateTime GetCurrentMonthFirstDate()
        {
            var now = GetNow();
            return new DateTime(now.Year, now.Month, 1);
        }
        public virtual DateTime GetCurrentMonthLastDate()
        {
            var now = GetNow();
            var days = DateTime.DaysInMonth(now.Year, now.Month);
            return new DateTime(now.Year, now.Month, days);
        }

        public virtual DateTime GetNow()
        {
            return DateTime.Now;
        }


        public virtual DateTime? TryParse(string str)
        {
            DateTime time;
            if (DateTime.TryParseExact(str, DatetimeFormatstrings, culture, DateTimeStyles.AllowWhiteSpaces, out time))
            {
                return time;
            }
            return null;
        }
        public virtual DateTime Parse(string str)
        {
            return DateTime.ParseExact(str, DatetimeFormatstrings, culture, DateTimeStyles.AllowWhiteSpaces);
        }


        public virtual string ToDate(DateTime? dateTime)
        {
            return string.Format(culture, "{0:yyyy-MM-dd}", dateTime);
        }

        public virtual string ToTime(DateTime? dateTime)
        {
            return string.Format(culture, "{0:HH:mm:ss}", dateTime);
        }

        public virtual string ToDateTime(DateTime? dateTime)
        {
            return string.Format(culture, "{0:yyyy-MM-dd HH:mm:ss}", dateTime);
        }

        public virtual TimeSpan? TimeTryParse(string str)
        {
            TimeSpan time;
            if (TimeSpan.TryParseExact(str, TimeFormatstrings, culture, TimeSpanStyles.None, out time))
            {
                return time;
            }
            return null;
        }

        public virtual TimeSpan TimeParse(string str)
        {
            return TimeSpan.ParseExact(str, TimeFormatstrings, culture, TimeSpanStyles.None);
        }

        public virtual string ToTime(TimeSpan? dateTime)
        {
            return string.Format(culture, "{0:hh\\:mm\\:ss}", dateTime);
        }

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public long ToUnixTimestamp(DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }
        /// <summary>
        /// unix时间戳转成日期
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns>
        public DateTime ToDateTime(long timeStamp)
        {
        
            DateTime dateTimeStart = System.TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }
    }
}
