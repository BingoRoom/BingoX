namespace BingoX.DataAccessor
{
    /// <summary>
    /// 数据库操作拦截器在DI中的生命周期类型
    /// </summary>
    public enum InterceptDIEnum
    {
        /// <summary>
        /// 未知
        /// </summary>
        None,
        /// <summary>
        /// 瞬时的
        /// </summary>
        Transient,
        /// <summary>
        /// 请求生命周期内的
        /// </summary>
        Scoped,
        /// <summary>
        /// 全局单例的
        /// </summary>
        Singleton
    }
}