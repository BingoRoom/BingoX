
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

        }
        /// <summary>
        /// 完成事务
        /// </summary>
        public void Commit()
        {

            DoTracker();
            context.ChangeTracker.SaveChanges();

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
    }
}
