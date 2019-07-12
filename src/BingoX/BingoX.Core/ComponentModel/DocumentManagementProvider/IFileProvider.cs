using System;
using System.Collections.Specialized;

namespace BingoX.ComponentModel.DocumentManagementProvider
{
    /// <summary>
    /// 表示一个文档操作方法提供程序接口
    /// </summary>
    public interface IFileProvider : IDisposable
    {
        /// <summary>
        /// 连接
        /// </summary>
        void Connection();
        /// <summary>
        /// 关闭
        /// </summary>
        void Close();
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="collection"></param>
        void Initialize(NameValueCollection collection);
    }
}
