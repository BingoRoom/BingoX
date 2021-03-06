﻿using BingoX.ComponentModel;
using BingoX.DataAccessor;
using BingoX.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;

namespace BingoX.Repository.AspNetCore
{
    /// <summary>
    /// 抽象仓储选项构造器
    /// </summary>
    public abstract class RepositoryContextOptionBuilder
    {
        protected readonly RepositoryContextOptionBuilderInfo RepositoryContextOptionBuilderInfo;
        protected readonly IEnumerable<DataAccessorBuilderInfo>  DataAccessorBuilderInfos;
        public RepositoryContextOptionBuilder(RepositoryContextOptionBuilderInfo repositoryContextOptionBuilderInfo)
        {
            this.RepositoryContextOptionBuilderInfo = repositoryContextOptionBuilderInfo;
            DataAccessorBuilderInfos = FilterDataAccessorBuilderInfo(repositoryContextOptionBuilderInfo.DataAccessorBuilderInfos);
        }
        /// <summary>
        /// 向数据访问器工厂集合补充工厂
        /// </summary>
        /// <param name="serviceProvider">服务提供器</param>
        /// <param name="config">主机配置</param>
        /// <param name="repositoryContextOptions">仓储上下文选项</param>
        public abstract void ReplenishDataAccessorFactories(IServiceProvider serviceProvider, IConfiguration config, RepositoryContextOptions repositoryContextOptions);
        /// <summary>
        /// 注入数据库拦截器
        /// </summary>
        /// <param name="services">服务集合</param>
        public abstract void InjectDbIntercepts(IServiceCollection services);
        /// <summary>
        /// 过滤数据访问器构建信息
        /// </summary>
        /// <param name="dataAccessorBuilderInfos">数据访问器构建信息总集合</param>
        /// <returns>返回符合当前类型操作的数据访问器构建信息</returns>
        protected abstract IEnumerable<DataAccessorBuilderInfo> FilterDataAccessorBuilderInfo(DataAccessorBuilderInfoColletion dataAccessorBuilderInfos);

        protected virtual void InitIntercept(IServiceProvider serviceProvider, DbEntityInterceptManagement interceptManagement)
        {
            var copt = serviceProvider.GetService<RepositoryContextOptions>();
            foreach (var options in DataAccessorBuilderInfos)
            {
                interceptManagement.AddRangeIntercepts(options.Intercepts.OfType<DbEntityInterceptAttribute>());

            }
            interceptManagement.AddRangeIntercepts(copt.Intercepts.OfType<DbEntityInterceptAttribute>());
        }
    }
}
