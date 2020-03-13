
using System;
using System.Data;
using System.Linq;

namespace BingoX.DataAccessor.SqlSugar
{
    
    public class SqlSugarUnitOfWork : IUnitOfWork
    {
        private SqlSugarDbContext context;

        public SqlSugarUnitOfWork(SqlSugarDbContext context)
        {
            this.context = context;
        }

        public override int GetHashCode()
        {
            return context.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            if (obj is SqlSugarUnitOfWork)
            {
                var efun = obj as SqlSugarUnitOfWork;
                return object.Equals(efun.context, context);
            }
            return false;

        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            if (this.context.Database.Ado.Transaction != null)
            {
                this.context.Database.Ado.Transaction.Rollback();
                this.context.Database.Ado.Transaction = null;
            }
            context.ChangeTracker.Clear();
        }
        /// <summary>
        /// 完成事务
        /// </summary>
        public void Commit()
        {
            if (this.context.Database.Ado.Transaction != null)
            {
                this.context.Database.Ado.CommitTran();
            }
            else
            {
                SaveChanges();
            }

        }
        public void SaveChanges()
        {
            DoTracker();
            context.SaveChanges();
          
        }
        private void DoTracker()
        {

            var serviceProvider = context.GetServiceProvider();
            if (serviceProvider == null) return;
            var entries = context.ChangeTracker.Entries();

            var interceptManagement = serviceProvider.GetService(typeof(SqlSugarDbEntityInterceptManagement)) as SqlSugarDbEntityInterceptManagement;

            if (interceptManagement != null)
            {
                foreach (var entityEntry in entries)
                {
                    interceptManagement.Interceptor(entityEntry);
                }
            }
        }

        public void BeginTransaction()
        {
            this.context.Database.Ado.BeginTran();

        }
    }
}
