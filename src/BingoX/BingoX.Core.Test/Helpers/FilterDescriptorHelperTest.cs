using BingoX.ComponentModel.Data;
using BingoX.Helper;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace BingoX.Core.Test.Helpers
{
    [Author("Dason")]
    [TestFixture]
    public class FilterDescriptorHelperTest
    {

        [Test]

        public void TestFilterDescriptorGetValue()
        {
            FilterDescriptor filterDescriptor = new FilterDescriptor() { Key = "a1", Value = "1234567" };
            var a1 = filterDescriptor.GetValue<long>();
            Assert.AreEqual(a1, 1234567);
        }
    }
    [Author("Dason")]
    [TestFixture]
    public class DictionaryHelperTest
    {

        [Test]

        public void TestGetChanges()
        {
            IDictionary<string, object> currentValues = new Dictionary<string, object>() { { "ID", 123456 }, { "Date", new DateTime(2018, 1, 1) }, { "de", 11.11d }, { "str", "1" } };
            IDictionary<string, object> originalValues = new Dictionary<string, object>() { { "ID", 123456 }, { "Date", new DateTime(2018, 1, 1) }, { "de", 11.11d }, { "str", "1" } };
            var changes = currentValues.GetChanges(originalValues);
            Assert.AreEqual(changes.Count, 0);
        }
    }
    [Author("Dason")]
    [TestFixture]
    public class CollectionHelperTest
    {
        class UserInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }

        [Test]

        public void TestToTable()
        {
            IList<UserInfo> users = new List<UserInfo> { new UserInfo { Id = 1, Name = "a1" }, new UserInfo { Id = 2, Name = "b1" } };
            Assert.AreEqual(users.ToDataTable().Rows.Count, 2);
        }

        [Test]

        public void TestToTable2()
        {
            IList<UserInfo> users = new List<UserInfo> { new UserInfo { Id = 1, Name = "a1" }, new UserInfo { Id = 2, Name = "b1" } };
            var dt = users.ToDataTable(n => n.Name);
            Assert.AreEqual(dt.Columns.Count, 1);
            Assert.AreEqual(dt.Rows.Count, 2);
        }
    }
}
