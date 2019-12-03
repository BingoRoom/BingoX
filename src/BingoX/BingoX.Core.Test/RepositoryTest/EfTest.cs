using BingoX.EF;
using BingoX.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

            Microsoft.Extensions.DependencyInjection.ServiceCollection services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            services.AddDbEntityIntercept(n =>
            {
                n.Intercepts.Add<AopCreatedInfo>();
            });
            //services.AddScoped<AopUser>();
            //services.AddScoped<AopCreatedInfo>();
            var factory = new Microsoft.Extensions.DependencyInjection.DefaultServiceProviderFactory();
            var serviceProvider = factory.CreateServiceProvider(services);
          
            var manage = new EF.EfDbEntityInterceptManagement(serviceProvider);

            var global = DbEntityInterceptServiceCollectionExtensions.Options.Intercepts.OfType<DbEntityInterceptAttribute>();
            manage.AddRangeGlobalIntercepts(global);
            var attributes = manage.GetAttributes(typeof(UserTest)).ToArray();

            var aops = manage.GetAops(typeof(UserTest)).ToArray();
            Assert.IsNotNull(aops);
            Assert.IsNotEmpty(aops);
            Assert.AreEqual(aops.Length, attributes.Length);
        }


       
        class BaseEntityTest
        {
            public int Id { get; set; }

            public DateTime CreatedDate { get; set; }
            public string Created { get; set; }
        }

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
