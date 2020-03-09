namespace BingoX.Repository
{
    public interface IRepositoryUnitOfWork
    {
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
    }
}