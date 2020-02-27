using System;

namespace BingoX.Domain
{
    /// <summary>
    /// 表示一个可审计创建的实体
    /// </summary>
    public interface IAuditCreated
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        DateTime CreatedDate { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        string Created { get; set; }
    }
}
