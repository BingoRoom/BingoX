using BingoX.DynamicSearch;
using BingoX.Security;
using BingoX.Utility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;

namespace BingoX.Test.Db
{
    [Author("Dason")]
    [TestFixture]
    public class DynamicSchemaReaderTest
    {
        [OneTimeSetUp]
        public void Setup()
        {

        }
        [Test]
        public void TestSchemaReader()
        {
            DynamicSchemaReader reader = new DynamicSchemaReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Db", "config.json"));
            reader.LoadConfig();

            DynamicQuerySerivce serivce = new DynamicQuerySerivce(reader.DataAccessors, reader.Tables);
            serivce.Query("greeinfo");
            serivce.Query("greeinfo", new System.Collections.Specialized.NameValueCollection() { { "fullname","珠海"} });
            serivce.GetId("greeinfo", 400);
        }

    }
}
