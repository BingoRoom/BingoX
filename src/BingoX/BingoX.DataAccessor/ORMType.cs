using System.ComponentModel;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// ORM类型
    /// </summary>
    public enum ORMType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        [Description("未知类型")]
        None = 0,
        /// <summary>
        /// EntityFramework
        /// </summary>
        [Description("EntityFramework")]
        EntityFramework = 1,
        /// <summary>
        /// SqlSugar
        /// </summary>
        [Description("SqlSugar")]
        SqlSugar = 0,
        /// <summary>
        /// Dapper
        /// </summary>
        [Description("Dapper")]
        Dapper = 0,
        /// <summary>
        /// 自定义ORM
        /// </summary>
        [Description("自定义ORM")]
        Custom = 99,
    }
}
