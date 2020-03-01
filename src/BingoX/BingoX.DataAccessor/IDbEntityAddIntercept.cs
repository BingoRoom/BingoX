namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个针对数据库新增操作的拦截器
    /// </summary>
    public interface IDbEntityAddIntercept : IDbEntityIntercept
    {
        /// <summary>
        /// 执行新增拦截
        /// </summary>
        /// <param name="info">当前DAO的新增状态信息</param>
        void OnAdd(DbEntityCreateInfo info);

    }
}