#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System.Data;
using System.Linq;

namespace BingoX.DataAccessor.EF
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private EfDbContext context;

        public EfUnitOfWork(EfDbContext context)
        {
            this.context = context;
        }

#if Standard
        public Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction ContextTransaction { get; private set; }
#else
        public DbContextTransaction ContextTransaction { get; private set; }

#endif
        /// <summary>
        /// 初始化事务
        /// </summary>
        /// <param name="level"></param>
        public void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            ContextTransaction = context.Database.BeginTransaction();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (ContextTransaction != null)
            {
                ContextTransaction.Rollback();
                ContextTransaction = null;
            }
            else
            {
                var items = this.context.ChangeTracker.Entries().ToList();
                items.ForEach(o => o.State = EntityState.Unchanged);

            }
        }
        /// <summary>
        /// 完成事务
        /// </summary>
        public void Commit()
        {
            if (ContextTransaction != null)
            {
                ContextTransaction.Commit();
                ContextTransaction = null;
            }
            else
            {
                DoTracker();
                context.SaveChanges();
            }

        }
        private void DoTracker()
        {
            var entries = context.ChangeTracker.Entries();
            if (!context.RootContextData.ContainsKey(EfDbContext.DIConst)) return;
            var serviceProvider = context.RootContextData[EfDbContext.DIConst] as System.IServiceProvider;
            if (serviceProvider == null) return;
#if Standard
            var interceptManagement = serviceProvider.GetService<EfDbEntityInterceptManagement>();
#else
            var interceptManagement = serviceProvider.GetService(typeof(EfDbEntityInterceptManagement)) as EfDbEntityInterceptManagement;
#endif
            if (interceptManagement != null)
            {
                foreach (var entityEntry in entries)
                {
                    interceptManagement.Interceptor(entityEntry);
                }
            }
        }
    }
}
