using System;
using System.Collections.Specialized;

namespace BingoX.FileSystem
{
    /// <summary>
    /// 表示一个文档操作方法提供程序接口
    /// </summary>
    public interface IDocumentManagementProvider : IDisposable
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
