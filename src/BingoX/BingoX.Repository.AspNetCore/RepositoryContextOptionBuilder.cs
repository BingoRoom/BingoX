using BingoX.DataAccessor;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BingoX.Repository.AspNetCore
{
    /// <summary>
    /// 仓储上下文选项构建器
    /// </summary>
    public class RepositoryContextOptionBuilder
    {
        private readonly IServiceCollection services;
        private readonly IConfiguration configuration;

        public RepositoryContextOptionBuilder(IServiceCollection services, IConfiguration configuration)
        {
            dataAccessorBuilderInfos = new DataAccessorBuilderInfoColletion();
            this.services = services;
            this.configuration = configuration;
        }
        /// <summary>
        /// 数据访问器构建器信息集合
        /// </summary>
        public DataAccessorBuilderInfoColletion dataAccessorBuilderInfos{ get; protected set; }
        /// <summary>
        /// 数据库连接名称集合
        /// </summary>
        public Dictionary<string, string> ConnectionNames { get; set; }
        /// <summary>
        /// 仓储程序集
        /// </summary>
        public Assembly RepositoryAssembly { get; set; }
        /// <summary>
        /// 领域聚合、实体、值对像与数据库实体之间的映射器
        /// </summary>
        public IRepositoryMapper Mapper { get; set; }
        /// <summary>
        /// 执行构建
        /// </summary>
        public void Build()
        {

        }

        private IDataAccessorFactory CreateDataAccessorFactory()
        {
            throw new NotImplementedException();
        }

        private RepositoryContextOptions CreateRepositoryContextOptions(IDictionary<string, IDataAccessorFactory> dict)
        {
            if (Mapper == null) throw new RepositoryOperationException("IRepositoryMapper为空，构建仓储失败");
            var rco = new RepositoryContextOptions(Mapper);
            foreach (var item in dict)
            {
                rco.DataAccessorFactories.Add(item.Key, item.Value);
            }
            services.AddScoped<RepositoryContextOptions>();
            return rco;
        }

        private void InjectRepository()
        {

        }
    }
}
