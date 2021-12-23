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
using System.Linq.Expressions;
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

        [OneTimeSetUp()]
        public void Setup()
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

                n.DefaultConnectionName = "db1";
                n.Intercepts.Add<AopCreatedInfo>(InterceptDIEnum.Scoped);
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
                        dbi.Intercepts.Add<AopCreatedInfo>();
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
                        //dbi.Intercepts.Add(new AopUser());
                        dbi.DbContextOption = new ConnectionConfig() { ConnectionString = conn, DbType = DbType.SqlServer, InitKeyType = InitKeyType.SystemTable };
                    }
                );
                options = n;
            });

            serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
        }
        [Test]
        public void TestAopAtt()
        {


            var manage = serviceProvider.GetService<EfDbEntityInterceptManagement>();
            Assert.IsNotNull(manage);
            var global = options.DataAccessorBuilderInfos[0].Intercepts.OfType<DbEntityInterceptAttribute>();
            manage.AddRangeIntercepts(global);
            var attributes = manage.GetAttributes(typeof(Account)).ToArray();

            var aops = manage.GetAops(typeof(Account)).ToArray();
            Assert.IsNotNull(aops);
            Assert.IsNotEmpty(aops);
            Assert.AreEqual(aops.Length, attributes.Length);
        }
        [Test]
        public void TestQueryRepository()
        {
            var roleRepository = serviceProvider.GetService<IRepositoryFactory>().Create<Role>();
            var list = roleRepository.QueryAll();
            Assert.GreaterOrEqual(list.Count, 1);
        }

        [Test]
        public void TestQuery_EfRepository()
        {
            var roleRepository = serviceProvider.GetService<IRepositoryFactory>().Create<Role>();
            var list = roleRepository.QueryAll();
            Assert.GreaterOrEqual(list.Count, 1);
        }

        [Test]
        public void TestQueryWithSql_EfRepository()
        {
            var accountRepository = serviceProvider.GetService<AccountRepository>();
            var list = accountRepository.GetAccountOnSql();
            Assert.GreaterOrEqual(list.Length, 1);
        }
        [Test]
        public void TestQueryWithSql_SqlSuargRepository()
        {
            var roleRepository = serviceProvider.GetService<IRepositoryFactory>().CreateRepository<AccountRepository>();
            var list = roleRepository.QueryAll();
            Assert.GreaterOrEqual(list.Count, 1);
        }

        [Test]
        public void TestNoTracking_SqlSuargRepository()
        {
            var roleRepository = serviceProvider.GetService<IRepositoryFactory>().CreateRepository<AccountRepository>();
          
            //var roleRepository = serviceProvider.GetService<IRepositoryFactory>().CreateRepository<AccountRepository>("db2");
            roleRepository.UpdateUserState();
            roleRepository.Commit();
        }
        [Test]
        public void TestTruncate_EFRepository()
        {
            var accountRepository = serviceProvider.GetService<AccountRepository>();
            accountRepository.Truncate();
        }
        [Test]
        public void TestAddUpdate_EFRepository()
        {
            var accountRepository = serviceProvider.GetService<AccountRepository>();
            Assert.IsNotNull(accountRepository);
            var roleRepository = serviceProvider.GetService<IRepository<Role>>();
            // 
            Assert.IsNotNull(roleRepository);
            var accoutroles = accountRepository.GetAccounts();

            DateTime dateTimeInit = DateTime.Now;
            roleRepository.Add(new Role()
            {
                RoleCode = "admin",
                RoleName = "管理员"
            });
            roleRepository.UnitOfWork.Commit();

            var roles = roleRepository.Where(n => n.RoleCode == "admin" && n.CreatedDate > dateTimeInit);
            Assert.AreEqual(roles.Count(), 1);
            Assert.IsNotNull(roles[0]);
            Assert.AreEqual(roles[0].RoleCode, "admin");
            Assert.AreEqual(roles[0].RoleName, "管理员");



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

            accounts = accountRepository.Where(n => n.Name == "张三" && n.CreatedDate > dateTimeInit);
            Assert.AreEqual(accounts.Count(), 1);
            Assert.IsNotNull(accounts[0]);
            Assert.IsNotNull(accounts[0].Role);
            Assert.AreEqual(accounts[0].Name, "张三");
            Assert.AreEqual(accounts[0].Age, 22);

            DateTime dateTimeModified = accounts[0].ModifyDate;

            Assert.GreaterOrEqual(dateTimeModified, dateTimeAdded);
        }
    }

    public class BaseEntityTest : ISnowflakeEntity<BaseEntityTest>
    {
        public DateTime CreatedDate { get; set; }
        public string Created { get; set; }
        public DateTime ModifyDate { get; set; }
        public string Modify { get; set; }
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public long ID { get; set; }
    }
    public class AccountDataAccessor : EFSnowflakeDataAccessor<Account>
    {
        public AccountDataAccessor(EfDbContext context) : base(context)
        {
        }
        public AccountRepository.AccountRole[] GetAccounts()
        {
            var outerSet = context.Set<Account>();
            var inner1Set = context.Set<Role>();
            var query = from outer in outerSet
                        join inner1 in inner1Set on outer.RoleId equals inner1.ID
                        select new AccountRepository.AccountRole { Age = outer.Age, Name = outer.Name, RoleCode = inner1.RoleCode, RoleName = inner1.RoleName };
            return query.ToArray();
        }
    }
    public class RoleRepository : Repository<Role>
    {
        public RoleRepository(RepositoryContextOptions options) : this(options, null)
        {
        }

        public RoleRepository(RepositoryContextOptions options, string dbname) : base(options, dbname)
        {
            Wrapper.SetInclude = opt => opt.Include(n => n.Accounts);
        }
    }
    public class AccountRepository : Repository<Account>
    {
        public AccountRepository(RepositoryContextOptions context) : this(context, "db1")
        {

        }
        public AccountRepository(RepositoryContextOptions options, string dbname) : base(options, dbname)
        {
            _wrapper = CreateWrapper<Account, AccountDataAccessor>("db1");

            _roleWrapper = CreateWrapper<Role>(dbname);
            Wrapper.SetInclude = opt => opt.Include(n => n.Role);
        }
        readonly AccountDataAccessor _wrapper;
        private readonly IDataAccessor<Role> _roleWrapper;
        public void UpdateUserState()
        {
     
            var noTracking = _roleWrapper.AsNoTracking();
            noTracking.Update(n => new Role {   RoleName = "a1" }, n => n.RoleCode == "Admin_1");
        }

        protected override IDataAccessor<Account> Wrapper { get { return _wrapper; } }

        public override IList<Account> Where(Expression<Func<Account, bool>> whereLambda)
        {
            return _wrapper.WhereTracking(whereLambda);
        }
        public AccountRole[] GetAccounts()
        {
            return _wrapper.GetAccounts();
        }
        public AccountRole[] GetAccountOnSql()
        {
            var sqlFacad = CreateSqlFacade("db1");
            return sqlFacad.QueryList<AccountRole>("select  outer1.Name,outer1.Age,inner1.RoleName,inner1.RoleName from [Account]  outer1  INNER  join  [Role]  inner1 on outer1.roleid=inner1.id ").ToArray();
        }

        public void Truncate()
        {
            var sqlFacad = CreateSqlFacade("db1");
            sqlFacad.Truncate<Account>();
            sqlFacad.Commit();
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
        [SugarColumn(IsIgnore = true)]
        public virtual List<Account> Accounts { get; set; }

    }
    //   [DbEntityInterceptAttribute(typeof(AopUser))]
    [CanTruncateAttribute("Account")]
    public class Account : BaseEntityTest, ISnowflakeEntity<Account>, IDomainEntry
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public long RoleId { get; set; }
        [SugarColumn(IsIgnore = true)]
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
    class AopCreatedInfo : IDbEntityAddIntercept, IDbEntityModifiyIntercept, IDbEntityDeleteIntercept
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
