#if Standard
using Microsoft.EntityFrameworkCore;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using BingoX.Repository;

namespace BingoX.EF
{
#if Standard
    public abstract class EfDbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public EfDbContext(DbContextOptions options) : base(options)
        {

        }
    }
     

#else
    public abstract class EfDbContext : System.Data.Entity.DbContext, IDbContext
    {
        public EfDbContext(string nameOrConnectionString) : base() { }
        public EfDbContext(string nameOrConnectionString, DbCompiledModel model) : base() { }
        public EfDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base() { }
        public EfDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base() { }
        public EfDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base() { }

    }

#endif
}
