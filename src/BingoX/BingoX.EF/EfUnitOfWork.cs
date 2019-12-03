#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel; 
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System.Data;
using System.Linq;
using BingoX.Repository;

namespace BingoX.EF
{


    public class EfUnitOfWork : IUnitOfWork
    {
        private EfDbContext context;

        public EfUnitOfWork(EfDbContext context)
        {
            this.context = context;
        }

#if Standard
        //  public EfDbEntityInterceptManagement InterceptManagement { get; set; }
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

#if Standard
            var entries = context.ChangeTracker.Entries();
            var interceptManagement = DbEntityInterceptServiceCollectionExtensions.InterceptManagement;
            if (interceptManagement != null)
            {
                foreach (var entityEntry in entries)
                {
                    interceptManagement.Interceptor(entityEntry);
                }
            }
#endif
        }

    }
}
