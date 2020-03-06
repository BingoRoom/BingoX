using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个数据库上下文
    /// </summary>
    public interface IDbContext
    {
        /// <summary>
        /// 数据库上下文辅助数据字典
        /// </summary>
        IDictionary<string, object> RootContextData { get; }
        /// <summary>
        /// 设置服务提供器
        /// </summary>
        /// <param name="serviceProvider"></param>
        void SetServiceProvider(System.IServiceProvider serviceProvider);
        /// <summary>
        /// 获取服务提供器
        /// </summary>
        /// <returns></returns>
        IServiceProvider GetServiceProvider();
    }
}
