using System;

namespace BingoX.Domain
{
    /// <summary>
    /// 表示可追溯实体修改信息
    /// </summary>
    public interface IAuditModified
    {
        /// <summary>
        /// 修改时间
        /// </summary>
        DateTime ModifiedDate { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        string Modified { get; set; }
    }
}
