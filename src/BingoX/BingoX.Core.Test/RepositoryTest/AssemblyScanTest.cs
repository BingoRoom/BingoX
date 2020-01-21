using BingoX.ComponentModel;
using BingoX.Domain;
using BingoX.Repository;
using NUnit.Framework;

namespace BingoX.Core.Test.RepositoryTest
{
    [Author("Dason")]
    [TestFixture]
    public class AssemblyScanTest
    {
        [Test]
        public void TestIsGenericType()
        {
            Assert.IsFalse(typeof(IAccountRepository).IsGenericType);
        }
        [Test]
        public void TestFindEntity()
        {
            var scaner = new AssemblyScanClass(this.GetType().Assembly, typeof(IEntity<,>)) ;
            var types = scaner.Find(null);
            Assert.AreEqual(2, types.Length);
            Assert.AreSame(typeof(IEntity<Role, long>), types[0].BaseType);
            Assert.AreSame(typeof(IEntity<Account, long>), types[1].BaseType);
        }
        [Test]
        public void TestFindEntity2()
        {
            var scaner = new AssemblyScanInterface(this.GetType().Assembly, typeof(IEntity<,>));
            var types = scaner.Find();
            Assert.AreEqual(0, types.Length);
        }
        [Test]
        public void TestFindRepository()
        {
       
            var scaner = new AssemblyScanClass(this.GetType().Assembly, typeof(IRepository<,>)) ;
            var types = scaner.Find();
            Assert.AreEqual(1, types.Length);
            Assert.AreSame(typeof(IRepository<Account, long>), types[0].BaseType);
        }
        [Test]
        public void TestFindRepository3()
        {
            var scaner = new AssemblyScanClass(this.GetType().Assembly, typeof(IRepository));
            var types = scaner.Find();
            Assert.AreEqual(1, types.Length);
        }
    }
}
