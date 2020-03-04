using BingoX.DataAccessor;
using BingoX.DataAccessor.EF;
using BingoX.Domain;
using BingoX.Helper;
using BingoX.Repository;
using BingoX.Repository.AspNetCore.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            ServiceCollection services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var Configuration = builder.Build();
            RepositoryContextOptionBuilder options = null;
            const string conn = "";
            services.AddRepository(Configuration, n =>
            {
                n.dataAccessorBuilderInfos.Add(
                    dbi => {
                        dbi.CustomConnectionName = "test";
                        dbi.AppSettingConnectionName = "DefaultConnection";
                        dbi.DataAccessorAssembly = GetType().Assembly;
                        dbi.EntityAssembly = GetType().Assembly;
                        dbi.DomainEntityAssembly = GetType().Assembly;
                        dbi.DbContextType = typeof(DbBoundedContext);
                        dbi.RepositoryAssembly = GetType().Assembly;
                        dbi.Intercepts.Add<AopCreatedInfo>(InterceptDIEnum.Scoped);
                        dbi.Intercepts.Add(new AopUser());
                        dbi.DbContextOption = new DbContextOptionsBuilder<DbBoundedContext>().UseSqlServer(conn);
                    }
                );
                options = n;
            });

            var factory = new Microsoft.Extensions.DependencyInjection.DefaultServiceProviderFactory();
            var serviceProvider = factory.CreateServiceProvider(services);

            var manage = serviceProvider.GetService<EfDbEntityInterceptManagement>();

            var global = options.dataAccessorBuilderInfos[0].Intercepts.OfType<DbEntityInterceptAttribute>();
            manage.AddRangeGlobalIntercepts(global);
            var attributes = manage.GetAttributes(typeof(Account)).ToArray();

            var aops = manage.GetAops(typeof(Account)).ToArray();
            Assert.IsNotNull(aops);
            Assert.IsNotEmpty(aops);
            Assert.AreEqual(aops.Length, attributes.Length);

            var dbBoundedContext = serviceProvider.GetService<DbBoundedContext>();
            Assert.IsNotNull(dbBoundedContext);
        }
    }


    public class BaseEntityTest
    {
        public long ID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Created { get; set; }
    }

    public class AccountRepository : Repository<Account>
    {
        public AccountRepository(RepositoryContextOptions context) : base(context)
        {
        }
    }
    public class Role : BaseEntityTest, ISnowflakeEntity<Role>, IDomainEntry
    {
        public string RoleCode { get; set; }

        public string RoleName { get; set; }

        public virtual List<Account> Accounts { get; set; }

    }
    [DbEntityInterceptAttribute(typeof(AopUser))]
    public class Account : BaseEntityTest, ISnowflakeEntity<Account>, IDomainEntry
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public long? RoleId { get; set; }
        public virtual Role Role { get; set; }
    }

    public class DbBoundedContext : EfDbContext
    {
        public DbBoundedContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new AccountTypeConfig());
            modelBuilder.ApplyConfiguration(new RoleTypeConfig());
        }
    }

    class RoleTypeConfig : AuditTypeConfig<Role>
    {
        protected override void OnConfigure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Role");
            builder.Property("RoleCode").IsRequired();
            builder.Property("RoleName").IsRequired();
            builder.HasMany(n => n.Accounts).WithOne(n => n.Role);
        }
    }

    class AccountTypeConfig : AuditTypeConfig<Account>
    {
        protected override void OnConfigure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("Account");
            builder.Property("Name").IsRequired();
            builder.HasOne(n => n.Role).WithMany(n => n.Accounts).HasForeignKey(n => n.RoleId);
        }
    }

    abstract class AuditTypeConfig<Entity> : IEntityTypeConfiguration<Entity> where Entity : BaseEntityTest
    {
        public void Configure(EntityTypeBuilder<Entity> builder)
        {
            builder.HasKey("ID");
            builder.Property("CreatedDate").IsRequired();
            builder.Property("Created").IsRequired();
            OnConfigure(builder);
        }

        protected abstract void OnConfigure(EntityTypeBuilder<Entity> builder);
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
