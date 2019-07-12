#if Standard
using Microsoft.EntityFrameworkCore;
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
        Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction ContextTransaction;
        /// <summary>
        /// 初始化事务
        /// </summary>
        /// <param name="level"></param>
        public void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            ContextTransaction = context.Database.BeginTransaction();
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
                context.SaveChanges();
            }

        }

        /// <summary>
        /// 完成事务
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
                context.Database.RollbackTransaction();
            }
        }
#else
        public DbContextTransaction ContextTransaction { get; private set; }
        /// <summary>
        /// 初始化事务
        /// </summary>
        /// <param name="level"></param>
        public void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            ContextTransaction = context.Database.BeginTransaction();
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
                context.SaveChanges();
            }

        }

        /// <summary>
        /// 完成事务
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
#endif

    }
}
