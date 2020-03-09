
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
                DoTracker();
                SaveChanges();
            }

        }
        public void SaveChanges()
        {
            object entity = null;
            SqlSugarEntityState state = SqlSugarEntityState.Unchanged;
            try
            {
                this.context.Database.Ado.BeginTran();
                foreach (var item in this.context.ChangeTracker.Entries())
                {
                    entity = item.Entity;
                    state = item.State;
                    item.ExecuteCommand();
                }
                this.context.Database.Ado.CommitTran();
            }
            catch (Exception ex)
            {

                throw new DataAccessorException("执行出错" + state + entity, ex);
            }

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
