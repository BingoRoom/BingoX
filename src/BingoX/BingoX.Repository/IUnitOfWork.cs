using System.Data;

namespace BingoX.Repository
{
    /// <summary>
    /// 事务管理器接口
    /// </summary>
    public interface IUnitOfWork
    {
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
        /// <summary>
        /// 初始化事务
        /// </summary>
        /// <param name="level"></param>
        void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted);
    }

}