using System;

namespace BingoX.Domain
{
    /// <summary>
    /// 表示可追溯实体创建信息
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
