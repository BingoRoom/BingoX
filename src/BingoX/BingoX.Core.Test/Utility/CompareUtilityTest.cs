using BingoX.Helper;
using BingoX.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Core.Test.Utility
{
    [TestFixture]
    [Author("Dason")]
    public class CompressUtilityUtilityTest
    {
        [Test]
        public void TestNewUtility()
        {
            CompressUtility utility = new CompressUtility();
        
        }
    }
    [TestFixture]
    [Author("Dason")]
    public class CompareUtilityTest
    {
        [Test, TestCaseSource(typeof(CompareUtilityDataClass), "TestFlaseCases")]
        public void TestFlaseIsBetween(IComparable value, IComparable x, IComparable y)
        {
            var flag = value.IsBetween(x, y);
            Assert.IsFalse(flag);

        }
        [Test, TestCaseSource(typeof(CompareUtilityDataClass), "TestTrueCases")]
        public void TestTrueIsBetween(IComparable value, IComparable x, IComparable y)
        {
            var flag = value.IsBetween(x, y);
            Assert.IsTrue(flag);
        }

        class CompareUtilityDataClass
        {
            static CompareUtilityDataClass()
            {
                TestFlaseCases = new TestCaseData[] {
                    new TestCaseData(1, 10, 20).SetName("int数字"),
                    new TestCaseData(21, 10, 20).SetName("int数字"),
                    new TestCaseData(1m, 10m, 20m).SetName("decimal数字"),
                    new TestCaseData(21m, 10m, 20m).SetName("decimal数字"),
                    new TestCaseData(1L, 10L, 20L).SetName("long数字"),
                    new TestCaseData(21L, 10L, 20L).SetName("long数字"),
                    new TestCaseData(1D, 10D, 20D).SetName("double数字"),
                    new TestCaseData(21D, 10D, 20D).SetName("double数字"),
                    new TestCaseData(DateTime.MaxValue,new DateTime(2019,1,1), new DateTime(2019,12,31)).SetName("DateTime"),
                    new TestCaseData(DateTime.MinValue, new DateTime(2019,1,1), new DateTime(2019,12,31)).SetName("DateTime"),
                    new TestCaseData(TimeSpan.MaxValue,new TimeSpan(0,0,0), new TimeSpan(23,59,59)).SetName("TimeSpan"),
                    new TestCaseData(TimeSpan.MinValue, new TimeSpan(0,0,0), new TimeSpan(23,59,59)).SetName("TimeSpan"),
                };

                TestTrueCases = new TestCaseData[] {
                    new TestCaseData(10, 10, 20).SetName("int数字"),
                    new TestCaseData(11, 10, 20).SetName("int数字"),
                    new TestCaseData(19, 10, 20).SetName("int数字"),
                    new TestCaseData(20, 10, 20).SetName("int数字"),
                    new TestCaseData(10m, 10m, 20m).SetName("decimal数字"),
                    new TestCaseData(11m, 10m, 20m).SetName("decimal数字"),
                    new TestCaseData(19m, 10m, 20m).SetName("decimal数字"),
                    new TestCaseData(20m, 10m, 20m).SetName("decimal数字"),
                    new TestCaseData(10L, 10L, 20L).SetName("long数字"),
                    new TestCaseData(11L, 10L, 20L).SetName("long数字"),
                    new TestCaseData(19L, 10L, 20L).SetName("long数字"),
                    new TestCaseData(20L, 10L, 20L).SetName("long数字"),
                    new TestCaseData(10D, 10D, 20D).SetName("double数字"),
                    new TestCaseData(11D, 10D, 20D).SetName("double数字"),
                    new TestCaseData(19D, 10D, 20D).SetName("double数字"),
                    new TestCaseData(20D, 10D, 20D).SetName("double数字"),

                    new TestCaseData(new DateTime(2019,1,1),new DateTime(2019,1,1), new DateTime(2019,12,31)).SetName("DateTime"),
                    new TestCaseData(new DateTime(2019,1,31), new DateTime(2019,1,1), new DateTime(2019,12,31)).SetName("DateTime"),
                    new TestCaseData(new TimeSpan(0,0,0),new TimeSpan(0,0,0), new TimeSpan(23,59,59)).SetName("TimeSpan"),
                    new TestCaseData(new TimeSpan(23,59,59), new TimeSpan(0,0,0), new TimeSpan(23,59,59)).SetName("TimeSpan"),
                };
            }
            public static IEnumerable<TestCaseData> TestFlaseCases { get; private set; }
            public static IEnumerable<TestCaseData> TestTrueCases { get; private set; }
        }
        [Test(Description = "IsBetween")]
        public void TestNulIsBetween()
        {
            int? x = null;
            var flag = x.IsBetween(10, 20);
            Assert.IsFalse(flag);
            flag = x.IsBetween(10, 20);
            Assert.IsFalse(flag);


        }

        [Test]
        public void TestIsBetweenThrow()
        {
            Assert.Throws<CompareException>(() =>
            {
                var flag = CompareHelper.IsBetween(1, 10, 1);
                Assert.IsFalse(flag);
                int? x = null;
                flag = CompareHelper.IsBetween(x, 10, 1);
                Assert.IsFalse(flag);
            });
        }


        [Test]
        public void TestNumerCompare()
        {
            Assert.IsTrue(CompareUtility.Compare(1m, 1m));
            Assert.IsTrue(CompareUtility.Compare(1m, 1d));
            Assert.IsTrue(CompareUtility.Compare(1d, 1m));
            Assert.IsTrue(CompareUtility.Compare(1d, 1d));

            Assert.IsFalse(CompareUtility.Compare(1.1m, 1m));
            Assert.IsFalse(CompareUtility.Compare(1.1m, 1d));
            Assert.IsFalse(CompareUtility.Compare(1.1d, 1m));
            Assert.IsFalse(CompareUtility.Compare(1.1d, 1d));

            Assert.IsFalse(CompareUtility.Compare(1m, 1.1m));
            Assert.IsFalse(CompareUtility.Compare(1m, 1.1d));
            Assert.IsFalse(CompareUtility.Compare(1d, 1.1m));
            Assert.IsFalse(CompareUtility.Compare(1d, 1.1d));

        }

        [Test]
        public void TestNumberDigthCompare()
        {
            Assert.IsTrue(CompareUtility.Compare(1.12345m, 1.12345m, 5));
            Assert.IsTrue(CompareUtility.Compare(1.12345m, 1.12345d, 5));
            Assert.IsTrue(CompareUtility.Compare(1.12345d, 1.12345m, 5));
            Assert.IsTrue(CompareUtility.Compare(1.12345d, 1.12345d, 5));

            Assert.IsTrue(CompareUtility.Compare(1.123451m, 1.12345m, 5));
            Assert.IsTrue(CompareUtility.Compare(1.123451m, 1.12345d, 5));
            Assert.IsTrue(CompareUtility.Compare(1.123451d, 1.12345m, 5));
            Assert.IsTrue(CompareUtility.Compare(1.123451d, 1.12345d, 5));


            Assert.IsTrue(CompareUtility.Compare(1.12345m, 1.123451m, 5));
            Assert.IsTrue(CompareUtility.Compare(1.12345m, 1.123451d, 5));
            Assert.IsTrue(CompareUtility.Compare(1.12345d, 1.123451m, 5));
            Assert.IsTrue(CompareUtility.Compare(1.12345d, 1.123451d, 5));


            Assert.IsFalse(CompareUtility.Compare(1.123451m, 1.12345m, 6));
            Assert.IsFalse(CompareUtility.Compare(1.123451m, 1.12345d, 6));
            Assert.IsFalse(CompareUtility.Compare(1.123451d, 1.12345m, 6));
            Assert.IsFalse(CompareUtility.Compare(1.123451d, 1.12345d, 6));


            Assert.IsFalse(CompareUtility.Compare(1.12345m, 1.123451m, 6));
            Assert.IsFalse(CompareUtility.Compare(1.12345m, 1.123451d, 6));
            Assert.IsFalse(CompareUtility.Compare(1.12345d, 1.123451m, 6));
            Assert.IsFalse(CompareUtility.Compare(1.12345d, 1.123451d, 6));
        }

        [Test]
        public void TestNumberNullDigthCompare()
        {
            {
                decimal? x = 1.12345m;
                decimal y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451m;
                y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345m;
                y = 1.123451m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451m;
                y = 1.12345m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345m;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));


                x = null;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }
            {
                decimal? x = 1.12345m;
                decimal? y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451m;
                y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345m;
                y = 1.123451m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451m;
                y = 1.12345m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345m;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
                x = null;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
                x = null;
                y = null;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }
            {
                double? x = 1.12345;
                decimal y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451;
                y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345;
                y = 1.123451m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451;
                y = 1.12345m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
                x = null;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }
            {
                double? x = 1.12345;
                decimal? y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451;
                y = 1.12345m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345;
                y = 1.123451m;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451;
                y = 1.12345m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
                x = null;
                y = 1.123451m;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
                x = 1.12345;
                y = null;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }
            {
                decimal? x = 1.12345m;
                double? y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451m;
                y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345m;
                y = 1.123451;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451m;
                y = 1.12345;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345m;
                y = 1.123451;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = null;
                y = 1.123451;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
                x = 1.12345m;
                y = null;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }

            {
                decimal? x = 1.12345m;
                double y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451m;
                y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345m;
                y = 1.123451;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451m;
                y = 1.12345;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345m;
                y = 1.123451;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = null;
                y = 1.123451;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }
            {
                double? x = 1.12345;
                double y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451;
                y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345;
                y = 1.123451;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451;
                y = 1.12345;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345;
                y = 1.123451;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = null;
                y = 1.123451;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }
            {
                double x = 1.12345;
                double y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.123451;
                y = 1.12345;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));

                x = 1.12345;
                y = 1.123451;
                Assert.IsTrue(CompareUtility.Compare(x, y, 5));


                x = 1.123451;
                y = 1.12345;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));

                x = 1.12345;
                y = 1.123451;
                Assert.IsFalse(CompareUtility.Compare(x, y, 6));
            }





        }
        [Test]
        public void TestStringCompare()
        {
            Assert.IsTrue(CompareUtility.Compare("", ""));
            Assert.IsTrue(CompareUtility.Compare("", " "));
            Assert.IsTrue(CompareUtility.Compare(" ", ""));
            Assert.IsTrue(CompareUtility.Compare("", null));
            Assert.IsTrue(CompareUtility.Compare(null, ""));
            Assert.IsTrue(CompareUtility.Compare(null, null));
        }
    }
}
