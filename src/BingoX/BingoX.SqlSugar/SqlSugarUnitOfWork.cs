using BingoX.Repository;
using System.Data;

namespace BingoX.SqlSugar
{
    public class SqlSugarUnitOfWork : IUnitOfWork
    {
        private SqlSugarDbContext context;

        public SqlSugarUnitOfWork(SqlSugarDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// 初始化事务
        /// </summary>
        /// <param name="level"></param>
        public void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            context.Client.Ado.BeginTran(level);
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            context.Client.Ado.CommitTran();
        }

        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            context.Client.Ado.RollbackTran();
        }
    }
}
