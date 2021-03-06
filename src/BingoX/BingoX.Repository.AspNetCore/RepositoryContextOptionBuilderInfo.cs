﻿using BingoX.DataAccessor;
using System.Collections.Generic;
using System.Reflection;

namespace BingoX.Repository.AspNetCore
{
    /// <summary>
    /// 仓储上下文选项构建器
    /// </summary>
    public class RepositoryContextOptionBuilderInfo
    {
        public RepositoryContextOptionBuilderInfo()
        {
            DataAccessorBuilderInfos = new DataAccessorBuilderInfoColletion();
            RepositoryContextOptionBuilders = new List<RepositoryContextOptionBuilder>();
            Intercepts = new InterceptCollection();
        }
        public virtual InterceptCollection Intercepts { get; private set; }
        /// <summary>
        /// 数据访问器构建器信息集合
        /// </summary>
        public virtual DataAccessorBuilderInfoColletion DataAccessorBuilderInfos { get; private set; }
        /// <summary>
        /// 仓储程序集
        /// </summary>
        public virtual Assembly RepositoryAssembly { get; set; }
        /// <summary>
        /// 领域聚合、实体、值对像与数据库实体之间的映射器
        /// </summary>
        public virtual IRepositoryMapper Mapper { get; set; }
        /// <summary>
        /// 仓储上下文选项构建器集合
        /// </summary>
        public virtual List<RepositoryContextOptionBuilder> RepositoryContextOptionBuilders { get; private set; }
        public string DefaultConnectionName { get; set; }
    }
}
