using BingoX.Generator;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BingoX.Core.Test.Generator
{
    [Author("Dason")]
    [TestFixture]
    public class SnowflakeGeneratorTest
    {
        private const long WorkerMask = 0x000000000001F000L;
        private const long DatacenterMask = 0x00000000003E0000L;
        private const ulong TimestampMask = 0xFFFFFFFFFFC00000UL;
        [Test]
        public void It_should_return_the_correct_job_id()
        {
            var s = new SnowflakeGenerator(1, 1);
            Assert.That(s.WorkerId, Is.EqualTo(1));
        }

        [Test]
        public void It_should_return_the_datacenter_id()
        {
            var s = new SnowflakeGenerator(1, 1);
            Assert.That(s.DatacenterId, Is.EqualTo(1));
        }

        [Test]
        public void It_should_properly_mask_worker_id()
        {
            const int workerId = 0x1F;
            const int datacenterId = 0;
            var worker = new SnowflakeGenerator(workerId, datacenterId);
            for (var i = 0; i < 1000; i++)
            {
                var id = worker.New();
                var expected = (id & WorkerMask) >> 12;
                Assert.That(workerId, Is.EqualTo(expected));
            }
        }

        [Test]
        public void It_should_properly_mask_the_datacenter_id()
        {
            const int workerId = 0x1F;
            const int datacenterId = 0;
            var worker = new SnowflakeGenerator(workerId, datacenterId);
            for (var i = 0; i < 1000; i++)
            {
                var id = worker.New();
                var expected = (id & DatacenterMask) >> 17;
                Assert.That(datacenterId, Is.EqualTo(expected));
            }
        }

        [Test]
        public void It_should_generate_ids_over_50_billion()
        {
            var worker = new SnowflakeGenerator(0, 0);
            StringBuilder filetext = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                var id = worker.New();
                filetext.AppendLine(id.ToString());
            }
            File.WriteAllText(@"D:\id.txt", filetext.ToString());
     //       Assert.That(id, Is.GreaterThan(50000000000L));
        }

    }
}
