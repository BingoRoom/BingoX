﻿#if Standard
using Microsoft.EntityFrameworkCore;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using BingoX.Repository;
using System.Collections.Generic;

namespace BingoX.EF
{
#if Standard
    public abstract class EfDbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public EfDbContext(DbContextOptions options) : base(options)
        {
            RootContextData = new Dictionary<string, object>();
        }
        internal const string DIConst = "ServiceProvider";
        public IDictionary<string, object> RootContextData { get; private set; }
    }


#else
    public  abstract  class EfDbContext : System.Data.Entity.DbContext, IDbContext
    {
        public EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public EfDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model) { }
        public EfDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }
        public EfDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext) { }
        public EfDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection,model,contextOwnsConnection) { } 
            

    }

#endif
}
