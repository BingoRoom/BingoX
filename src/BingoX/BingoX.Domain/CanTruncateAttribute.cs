using System;

namespace BingoX.Domain
{
    /// <summary>
    /// 表示一个可以清除表数据的数据库实体
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class CanTruncateAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="tablename">数据库表名</param>
        public CanTruncateAttribute(string tablename)
        {
            Tablename = tablename;
        }

        public string Tablename { get; }
    }
}