using BingoX.DataAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 仓储构建器信息
    /// </summary>
    public class DataAccessorBuilderInfo
    {
        private Type dbContextType;

        public DataAccessorBuilderInfo()
        {
            Intercepts = new InterceptCollection();
        }
        /// <summary>
        /// 数据库实体程序集
        /// </summary>
        public Assembly EntityAssembly { get; set; }
        /// <summary>
        /// 领域实体、聚合程序集
        /// </summary>
        public Assembly DomainEntityAssembly { get; set; }
        /// <summary>
        /// 数据访问器程序集
        /// </summary>
        public Assembly DataAccessorAssembly { get; set; }
        /// <summary>
        /// 仓储接口程序集
        /// </summary>
        public Assembly RepositoryAssembly { get; set; }
        /// <summary>
        /// 是否为合并领域实体与数据库实体的模式
        /// </summary>
        public bool IsMergeDomianAndDao { get; set; } = true;
        /// <summary>
        /// 数据库拦截器集合
        /// </summary>
        public InterceptCollection Intercepts { get; protected set; }
        /// <summary>
        /// 数据库上下文类型
        /// </summary>
        public Type DbContextType { 
            get => dbContextType; 
            set { 
                dbContextType = value; 
                if (value != null && DataAccessorAssembly == null) DataAccessorAssembly = value.Assembly; 
            } 
        }
        /// <summary>
        /// 数据库上下文选项
        /// </summary>
        public object DbContextOption { get; set; }
        /// <summary>
        /// 自定义的数据库连接字符串名
        /// </summary>
        public string CustomConnectionName { get; set; }
        /// <summary>
        /// AppSetting.json定义的数据库连接字符串名
        /// </summary>
        public string AppSettingConnectionName { get; set; }
        /// <summary>
        /// 数据访问器构建器提供程序
        /// </summary>
        public Assembly DataAccessorProviderAssembly { get; set; }
    }
}
