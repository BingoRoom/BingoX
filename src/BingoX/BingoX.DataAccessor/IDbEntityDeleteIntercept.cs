namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个针对数据库新增操作的拦截器
    /// </summary>
    public interface IDbEntityDeleteIntercept : IDbEntityIntercept
    {
        /// <summary>
        /// 执行删除拦截
        /// </summary>
        /// <param name="info">当前DAO的删除状态信息</param>
        void OnDelete(DbEntityDeleteInfo info);
    }
}