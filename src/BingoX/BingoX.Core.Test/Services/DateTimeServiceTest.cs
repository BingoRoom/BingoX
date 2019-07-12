using BingoX.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Core.Test
{
    [Author("Dason")]
    [TestFixture]
    public class DateTimeServiceTest
    {
        [Test, TestCaseSource(typeof(DateTimeServiceDataClass), "TestCases")]
        public void TestParse(IDateTimeService timeService)
        {

            var datetime = timeService.Parse("20181011");
            Assert.AreEqual(datetime.Date, new DateTime(2018, 10, 11).Date);
            datetime = timeService.Parse("2018-10-11");
            Assert.AreEqual(datetime.Date, new DateTime(2018, 10, 11).Date);


            datetime = timeService.Parse("2018-10-11 15:10:22");
            Assert.AreEqual(datetime, new DateTime(2018, 10, 11, 15, 10, 22));


            datetime = timeService.Parse("20181011151022");
            Assert.AreEqual(datetime, new DateTime(2018, 10, 11, 15, 10, 22));


            datetime = timeService.Parse("2018-10-11 15:10");
            Assert.AreEqual(datetime, new DateTime(2018, 10, 11, 15, 10, 0));


            datetime = timeService.Parse("201810111510");
            Assert.AreEqual(datetime, new DateTime(2018, 10, 11, 15, 10, 0));

            var time = timeService.TimeParse("151022");
            Assert.AreEqual(time, new TimeSpan(15, 10, 22));
            var time2 = timeService.TimeTryParse("151022");
            Assert.AreEqual(time2, new TimeSpan(15, 10, 22));
        }
        [Test, TestCaseSource(typeof(DateTimeServiceDataClass), "TestNowCases")]
        public void TestGetDate(IDateTimeService timeService, DateTime datetime)
        {

            
            var serviceTime = timeService.GetNow();
            AssertDatetime(serviceTime, datetime);

            Assert.AreEqual(timeService.GetCurrentYearFirstDate().Date, new DateTime(datetime.Year, 1, 1));
            Assert.AreEqual(timeService.GetCurrentYearLastDate().Date, new DateTime(datetime.Year, 12, 31));


            Assert.AreEqual(timeService.GetCurrentMonthFirstDate().Date, new DateTime(datetime.Year, datetime.Month, 1));
            Assert.AreEqual(timeService.GetCurrentMonthLastDate().Date, new DateTime(datetime.Year, datetime.Month, DateTime.DaysInMonth(datetime.Year, datetime.Month)));
        }

        private static void AssertDatetime(DateTime datetime, DateTime serviceTime)
        {
            Assert.Multiple(() =>
            {
                Assert.AreEqual(datetime.Date, serviceTime.Date);
                Assert.AreEqual(datetime.Hour, serviceTime.Hour);
                Assert.AreEqual(datetime.Minute, serviceTime.Minute);
                Assert.AreEqual(datetime.Second, serviceTime.Second);
            });
        }
        [Test, TestCaseSource(typeof(DateTimeServiceDataClass), "TestCases")]
        public void TestToString(IDateTimeService timeService)
        {

            var datetime = new DateTime(2018, 10, 11, 15, 10, 22);
            Assert.AreEqual(timeService.ToDate(datetime), "2018-10-11");
            Assert.AreEqual(timeService.ToTime(datetime), "15:10:22");
            Assert.AreEqual(timeService.ToDateTime(datetime), "2018-10-11 15:10:22");
            Assert.AreEqual(timeService.ToDate(null), "");


            Assert.AreEqual(timeService.ToTime(new TimeSpan(17, 10, 22)), "17:10:22");
            TimeSpan? nulltime = null;
            Assert.AreEqual(timeService.ToTime(nulltime), "");
        }



        class DateTimeServiceDataClass
        {

            public static IEnumerable<DateTimeService> TestCases
            {
                get
                {
                    yield return new DateTimeService();
                    yield return DateTimeService.Default;
                }
            }
            public static IEnumerable<TestCaseData> TestNowCases
            {
                get
                {
                    yield return new TestCaseData(new DateTimeService(), DateTime.Now);
                    yield return new TestCaseData(DateTimeService.Default, DateTime.Now);
                    yield return new TestCaseData(new SubDateTimeService(new DateTime(2018, 11, 18, 22, 54, 33)), new DateTime(2018, 11, 18, 22, 54, 33));
                }
            }
        }
    }
}
