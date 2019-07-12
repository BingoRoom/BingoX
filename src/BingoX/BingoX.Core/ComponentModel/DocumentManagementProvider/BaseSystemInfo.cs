using System;

namespace BingoX.ComponentModel.DocumentManagementProvider
{
    /// <summary>
    /// 表示一个基础文件系统基信息
    /// </summary>
    public abstract class BaseSystemInfo
    {
        /// <summary>
        /// 获取或设置一个值标明当前实例为文件夹
        /// </summary>
        public bool IsDirectory { get; protected set; }
        /// <summary>
        /// 获取或设置一个值标明当前实例为文件
        /// </summary>
        public bool IsFile { get; protected set; }
        /// <summary>
        /// 获取或设置一个值指定当前实例的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置一个值指定当前实例的创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 获取或设置一个值指示当前实例所使用的应用程序接口全类型名
        /// </summary>
        public string APIFullName { get; set; }
        /// <summary>
        /// 获取或设置一个值指定当前实例的外键标识
        /// </summary>
        public long ForeignKey { get; set; }

    }
}
