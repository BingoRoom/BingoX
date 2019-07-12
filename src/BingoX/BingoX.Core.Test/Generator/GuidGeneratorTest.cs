using BingoX.Generator;
using NUnit.Framework;
using System;

namespace BingoX.Core.Test.Generator
{
    [Author("Dason")]
    [TestFixture]
    public class GuidGeneratorTest
    {
        [Test]
        public void It_should_return_the_correct_job_id()
        {
            var s = new GuidGenerator(1, 1);
            Assert.That(s.WorkerId, Is.EqualTo(1));
        }

        [Test]
        public void It_should_return_the_datacenter_id()
        {
            var s = new GuidGenerator(1, 1);
            Assert.That(s.DatacenterId, Is.EqualTo(1));
        }

        [Test]
        public void newid()
        {
            var s = new GuidGenerator(1, 1);
            var id = s.New();
            Assert.IsNotNull(id);
        }
    }
}
