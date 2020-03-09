using BingoX.DataAccessor;
using BingoX.DataAccessor.EF;
using BingoX.DataAccessor.SqlSugar;
using BingoX.Domain;
using BingoX.Generator;
using BingoX.Helper;
using BingoX.Repository;
using BingoX.Repository.AspNetCore;
using BingoX.Repository.AspNetCore.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using SqlSugar;
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
        IServiceProvider serviceProvider;
        RepositoryContextOptionBuilderInfo options;


        [Test]
        public void TestAopAtt()
        {
            ServiceCollection services = new ServiceCollection();
            var builder = new ConfigurationBuilder()
               .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var Configuration = builder.Build();

            const string conn = "Data Source =(localdb)\\MSSQLLocalDB;Initial Catalog = BingoTest_DB;persist security info=True;;user id = sa;password = sasa;";
            //const string conn = "Data Source =localhost;Initial Catalog = BingoTest_DB;persist security info=True;;user id = sa;password = sasa;";
            services.AddRepository(Configuration, n =>
            {

                n.DefaultConnectionName = "db2";
                n.AddEF(
                    dbi =>
                    {
                        dbi.CustomConnectionName = "db1";
                        dbi.AppSettingConnectionName = "DefaultConnection";
                        dbi.DataAccessorAssembly = GetType().Assembly;
                        dbi.EntityAssembly = GetType().Assembly;
                        dbi.DomainEntityAssembly = GetType().Assembly;
                        dbi.DbContextType = typeof(DbBoundedContext);
                        dbi.RepositoryAssembly = GetType().Assembly;
                        dbi.Intercepts.Add<AopCreatedInfo>(InterceptDIEnum.Scoped);
                        //dbi.Intercepts.Add(new AopUser());
                        dbi.DbContextOption = new DbContextOptionsBuilder<DbBoundedContext>().UseSqlServer(conn).Options;
                    }
                );
                n.AddSqlSugar(
                    dbi =>
                    {
                        dbi.CustomConnectionName = "db2";
                        dbi.AppSettingConnectionName = "DefaultConnection";
                        dbi.DataAccessorAssembly = GetType().Assembly;
                        dbi.EntityAssembly = GetType().Assembly;
                        dbi.DomainEntityAssembly = GetType().Assembly;
                        dbi.DbContextType = typeof(SqlSugarDbBoundedContext);
                        dbi.RepositoryAssembly = GetType().Assembly;
                        dbi.Intercepts.Add<AopCreatedInfo>(InterceptDIEnum.Scoped);
                        //dbi.Intercepts.Add(new AopUser());
                        dbi.DbContextOption = new ConnectionConfig() { ConnectionString = conn, DbType = DbType.SqlServer };
                    }
                );
                options = n;
            });

            serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(services);

            var manage = serviceProvider.GetService<EfDbEntityInterceptManagement>();
            Assert.IsNotNull(manage);
            var global = options.DataAccessorBuilderInfos[0].Intercepts.OfType<DbEntityInterceptAttribute>();
            manage.AddRangeGlobalIntercepts(global);
            var attributes = manage.GetAttributes(typeof(Account)).ToArray();

            var aops = manage.GetAops(typeof(Account)).ToArray();
            Assert.IsNotNull(aops);
            Assert.IsNotEmpty(aops);
            Assert.AreEqual(aops.Length, attributes.Length);

            var accountRepository = serviceProvider.GetService<AccountRepository>();
            Assert.IsNotNull(accountRepository);

            var roleRepository = serviceProvider.GetService<IRepositoryFactory>().Create<Role>("db2");
            Assert.IsNotNull(roleRepository);

            roleRepository.Add(new Role()
            {
                RoleCode = "admin",
                RoleName = "管理员"
            });
            roleRepository.UnitOfWork.Commit();

            var roles = roleRepository.Where(n => n.RoleCode == "admin");
            Assert.AreEqual(roles.Count(), 1);
            Assert.IsNotNull(roles[0]);
            Assert.AreEqual(roles[0].RoleCode, "admin");
            Assert.AreEqual(roles[0].RoleName, "管理员");

            DateTime dateTimeInit = DateTime.Now;

            accountRepository.Add(new Account()
            {
                Name = "黄彬",
                Age = 35,
                RoleId = roles[0].ID
            });
            accountRepository.UnitOfWork.Commit();

            var accounts = accountRepository.Where(n => n.Name == "黄彬" && n.CreatedDate > dateTimeInit);
            Assert.AreEqual(accounts.Count(), 1);
            Assert.IsNotNull(accounts[0]);
            Assert.IsNotNull(accounts[0].Role);
            Assert.AreEqual(accounts[0].Name, "黄彬");
            Assert.AreEqual(accounts[0].Age, 35);
            Assert.AreEqual(accounts[0].Role.RoleCode, "admin");
            Assert.AreEqual(accounts[0].Role.RoleName, "管理员");

            DateTime dateTimeAdded = accounts[0].ModifyDate;

            accounts[0].Name = "张三";
            accounts[0].Age = 22;
            accountRepository.UpdateRange(accounts);
            accountRepository.UnitOfWork.Commit();

            accounts = accountRepository.Where(n => n.Name == "张三");
            Assert.AreEqual(accounts.Count(), 1);
            Assert.IsNotNull(accounts[0]);
            Assert.IsNotNull(accounts[0].Role);
            Assert.AreEqual(accounts[0].Name, "张三");
            Assert.AreEqual(accounts[0].Age, "22");

            DateTime dateTimeModified = accounts[0].ModifyDate;

            Assert.GreaterOrEqual(dateTimeModified, dateTimeAdded);
        }
    }

    public class BaseEntityTest : ISnowflakeEntity<Role>
    {
        public DateTime CreatedDate { get; set; }
        public string Created { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Modify { get; set; }
        public long ID { get; set; }
    }

    public class AccountRepository : Repository<Account>
    {
        public AccountRepository(RepositoryContextOptions context) : base(context)
        {
            _wrapper = CreateWrapper<Account>("db1");
            Wrapper.SetInclude = opt => opt.Include(n => n.Role);
        }
        readonly IDataAccessor<Account> _wrapper;
        protected override IDataAccessor<Account> Wrapper { get { return _wrapper; } }
        public IList<AccountRole> GetAccounts()
        {
            var join = CreateJoinFacade().Query<Account, Role, long, AccountRole>(
                outer => outer.RoleId,
                inner => inner.ID,
                (outer, inner) => new AccountRole
                {
                    RoleCode = inner.RoleCode,
                    RoleName = inner.RoleName,
                    Name = outer.Name,
                    Age = outer.Age
                });
            return join.ToList((outer, inner) => outer.Age > 10);
        }
        public class AccountRole
        {
            public string RoleCode { get; set; }

            public string RoleName { get; set; }

            public string Name { get; set; }
            public int Age { get; set; }
        }
    }


    //  [DbEntityInterceptAttribute(typeof(AopUser))]
    public class Role : BaseEntityTest, ISnowflakeEntity<Role>, IDomainEntry
    {
        public string RoleCode { get; set; }

        public string RoleName { get; set; }

        public virtual List<Account> Accounts { get; set; }

    }
    //   [DbEntityInterceptAttribute(typeof(AopUser))]
    public class Account : BaseEntityTest, ISnowflakeEntity<Account>, IDomainEntry
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public long RoleId { get; set; }
        public virtual Role Role { get; set; }
    }
    public class SqlSugarDbBoundedContext : SqlSugarDbContext
    {
        public SqlSugarDbBoundedContext(ConnectionConfig config) : base(config)
        {
        }
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
            builder.Property("ModifyDate").IsRequired();
            builder.Property("Modify").IsRequired();
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
    class AopCreatedInfo : IDbEntityIntercept, IDbEntityAddIntercept, IDbEntityModifiyIntercept, IDbEntityDeleteIntercept
    {
        public bool AllowDelete { get { return true; } }

        public bool AllowModifiy { get { return true; } }

        public bool AllowAdd { get { return true; } }

        public void OnAdd(DbEntityCreateInfo info)
        {
            SnowflakeGenerator snowflake = new SnowflakeGenerator(1, 1, 1);
            info.SetValue("ID", snowflake.New());
            info.SetValue("CreatedDate", DateTime.Now);
            info.SetValue("Created", "创建者");
            info.SetValue("ModifyDate", DateTime.Now);
            info.SetValue("Modify", "修改者");
            info.Accept = true;
        }

        public void OnDelete(DbEntityDeleteInfo info)
        {

        }

        public void OnModifiy(DbEntityChangeInfo info)
        {
            info.SetValue("ModifyDate", DateTime.Now);
            info.SetValue("Modify", "修改者");
            info.Accept = true;
        }
    }
}
