using BingoX.DataAccessor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 仓储构建器信息
    /// </summary>
    public class DataAccessorBuilderInfo
    {
        public DataAccessorBuilderInfo()
        {
            Intercepts = new InterceptCollection();
        }
        /// <summary>
        /// 数据库实体程序集
        /// </summary>
        public Assembly EntityAssembly { get; set; }
        /// <summary>
        /// 数据访问器程序集
        /// </summary>
        public Assembly DataAccessorAssembly { get; set; }
        /// <summary>
        /// 数据库拦截器集合
        /// </summary>
        public InterceptCollection Intercepts { get; protected set; }
        /// <summary>
        /// 数据库上下文选项
        /// </summary>
        public object DbContextOption { get; set; }
    }
}
