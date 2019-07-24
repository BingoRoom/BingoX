using BingoX.Repository;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoX.Core.Test.RepositoryTest
{
    [Author("Dason")]
    [TestFixture]
    public class EfTest
    {

        [Test]

        public void TestAopAtt()
        {
            var manage = new EF.EfDbEntityInterceptManagement();
            var aops = manage.GetAops(typeof(UserTest));
            Assert.IsNotNull(aops);
            Assert.IsNotEmpty(aops);
            Assert.AreEqual(aops.Length, 2);
        }


        [DbEntityInterceptAttribute(typeof(AopCreatedInfo))]
        class BaseEntityTest
        {
            public int Id { get; set; }

            public DateTime CreatedDate { get; set; }
            public string Created { get; set; }
        }

        [DbEntityInterceptAttribute(typeof(AopUser))]
        [DbEntityInterceptAttribute(typeof(AopUser))]
        class UserTest : BaseEntityTest
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }
        class AopUser : IDbEntityIntercept
        {
            public bool AllowDelete { get { return false; } }

            public bool AllowModifiy { get { return true; } }

            public bool AllowAdd { get { return true; } }

            public void OnAdd(DbEntityCreateInfo info)
            {
                throw new NotImplementedException();
            }

            public void OnDelete(DbEntityDeleteInfo info)
            {
                throw new NotImplementedException();
            }

            public void OnModifiy(DbEntityChangeInfo info)
            {
                throw new NotImplementedException();
            }
        }
        class AopCreatedInfo : IDbEntityIntercept
        {
            public bool AllowDelete { get { return true; } }

            public bool AllowModifiy { get { return true; } }

            public bool AllowAdd { get { return true; } }

            public void OnAdd(DbEntityCreateInfo info)
            {
                throw new NotImplementedException();
            }

            public void OnDelete(DbEntityDeleteInfo info)
            {
                throw new NotImplementedException();
            }

            public void OnModifiy(DbEntityChangeInfo info)
            {
                throw new NotImplementedException();
            }
        }
    }
}
