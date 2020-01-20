using BingoX.Domain;
using BingoX.EF;
using BingoX.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            BingoEFOptions<DbBoundedContext> options = null;
            services.AddBingoEF<DbBoundedContext>(n =>
            {
                options = n;
                //   
                var opt = new DbContextOptionsBuilder<DbBoundedContext>();
                //   opt.UseMySql("");
                n.DbContextOptions = opt.Options;

                n.AssemblyRepository = this.GetType().Assembly;
                n.Intercepts.Add<AopCreatedInfo>(InterceptDIEnum.Scoped);
                n.Intercepts.Add(new AopUser());
            });
            services.AddScoped<AopUser>();



            var factory = new Microsoft.Extensions.DependencyInjection.DefaultServiceProviderFactory();
            var serviceProvider = factory.CreateServiceProvider(services);

            var manage = serviceProvider.GetService<EF.EfDbEntityInterceptManagement>();

            var global = options.Intercepts.OfType<DbEntityInterceptAttribute>();
            manage.AddRangeGlobalIntercepts(global);
            var attributes = manage.GetAttributes(typeof(UserTest)).ToArray();

            var aops = manage.GetAops(typeof(UserTest)).ToArray();
            Assert.IsNotNull(aops);
            Assert.IsNotEmpty(aops);
            Assert.AreEqual(aops.Length, attributes.Length);
        }



        public class BaseEntityTest
        {
            public long ID { get; set; }

            public DateTime CreatedDate { get; set; }
            public string Created { get; set; }
        }
        public interface IAccountRepository : IRepositorySnowflake<Account>
        {

        }
        public class AccountRepository : EfRepositorySnowflake<Account>, IAccountRepository
        {
            public AccountRepository(EfDbContext context) : base(context)
            {
            }
        }
        public class Account : BaseEntityTest, ISnowflakeEntity<Account>
        {

        }
        [DbEntityInterceptAttribute(typeof(AopUser))]
        class UserTest : BaseEntityTest
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        public class DbBoundedContext : EfDbContext
        {
            public DbBoundedContext(DbContextOptions options) : base(options)
            {
            }
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
