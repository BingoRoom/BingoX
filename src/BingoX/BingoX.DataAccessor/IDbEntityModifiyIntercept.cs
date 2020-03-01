namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个针对数据库修改操作的拦截器
    /// </summary>
    public interface IDbEntityModifiyIntercept : IDbEntityIntercept
    {
        /// <summary>
        /// 执行修改拦截
        /// </summary>
        /// <param name="info">当前DAO的修改状态信息</param>
        void OnModifiy(DbEntityChangeInfo info);

    }
}