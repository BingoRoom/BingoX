#if Standard
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System.Collections.Generic;

namespace BingoX.DataAccessor.EF
{
#if Standard
    public abstract class EfDbContext : DbContext, IDbContext
    {
        public EfDbContext(DbContextOptions options) :base(options)
        {
            RootContextData = new Dictionary<string, object>();
            SqlFacade = new EFSqlFacade(this);
        }
        internal const string DIConst = "ServiceProvider";
        public IDictionary<string, object> RootContextData { get; private set; }

        public EFSqlFacade SqlFacade { get; private set; }

        ISqlFacade IDbContext.SqlFacade { get { return SqlFacade; } }

        public void SetServiceProvider(System.IServiceProvider serviceProvider)
        {
            if (RootContextData.ContainsKey(DIConst)) RootContextData[DIConst] = serviceProvider;
            else RootContextData.Add(DIConst, serviceProvider);
        }
        public System.IServiceProvider GetServiceProvider()
        {
            return RootContextData.ContainsKey(DIConst) ? RootContextData[DIConst] as System.IServiceProvider : null;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
#else
    public abstract class EfDbContext : System.Data.Entity.DbContext, IDbContext
    {
        public EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
            SqlFacade = new EFSqlFacade(this);
        }
        public EfDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
            SqlFacade = new EFSqlFacade(this);
        }
        public EfDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
            SqlFacade = new EFSqlFacade(this);
        }
        public EfDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext)
        {
            SqlFacade = new EFSqlFacade(this);
        }
        public EfDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
            SqlFacade = new EFSqlFacade(this);
        }

        public EFSqlFacade SqlFacade { get; private set; }

        ISqlFacade IDbContext.SqlFacade { get { return SqlFacade; } }
    }
#endif
}
