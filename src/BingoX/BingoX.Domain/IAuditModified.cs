using System;

namespace BingoX.Domain
{
    /// <summary>
    /// 表示一个可审计修改的实体
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
