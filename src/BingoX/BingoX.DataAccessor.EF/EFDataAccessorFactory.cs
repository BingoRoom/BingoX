using System;
using BingoX.Helper;
#if Standard
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
using System.Linq;

namespace BingoX.DataAccessor.EF
{
    public class EFDataAccessorFactory<TContext> : IDataAccessorFactory where TContext : EfDbContext
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DbContextOptions<TContext> dbContextOptions;

        public EFDataAccessorFactory(IServiceProvider serviceProvider, DbContextOptions<TContext> dbContextOptions)
        {
            this.serviceProvider = serviceProvider;
            this.dbContextOptions = dbContextOptions;
            dbcontext = CreateScopeDbContext();
            dbcontext.SetServiceProvider(serviceProvider);
        }

        public TContext dbcontext { get; set; }

        private TContext CreateScopeDbContext()
        {
            var constructor = typeof(TContext).GetConstructors().FirstOrDefault();
            var context = FastReflectionExtensions.FastInvoke(constructor, dbContextOptions) as TContext;
            return context;
        }

        public IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }

        void IDataAccessorFactory.AddIndluce<TEntity>(Func<TEntity, IQueryable<TEntity>> include)
        {
            throw new NotImplementedException();
        }

        IDataAccessor<TEntity> IDataAccessorFactory.Create<TEntity>()
        {
            //var dataAccessor = new EFDataAccessor<TEntity>(dbcontext);
            //return dataAccessor;
            //最后这里不知道如何写了，用哪个EFDataAccessor的实现是实际项目决定的。估计得改外观才行了
            throw new NotImplementedException();
        }
    }
}
