using BingoX.DataAccessor;
using BingoX.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BingoX.DataAccessor.SqlSugar;
using BingoX.Helper;
using BingoX.ComponentModel;
using System;
using BingoX.Domain;

namespace BingoX.Repository.AspNetCore.SqlSugar
{
    /// <summary>
    /// EF仓储上下文选项构建器
    /// </summary>
    public class SqlSugarRepositoryContextOptionBuilder : RepositoryContextOptionBuilder
    {
        public SqlSugarRepositoryContextOptionBuilder(RepositoryContextOptionBuilderInfo repositoryContextOptionBuilderInfo) : base(repositoryContextOptionBuilderInfo)
        {
        }

        public override void ReplenishDataAccessorFactories(IServiceProvider serviceProvider, IConfiguration config, RepositoryContextOptions repositoryContextOptions)
        {
            foreach (var item in dataAccessorBuilderInfos)
            {
                var connectionString = config.GetConnectionString(item.AppSettingConnectionName);
                var factoryType = typeof(SqlSugarDataAccessorFactory<>).MakeGenericType(item.DbContextType);
                var constructor = factoryType.GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 3);
                if (constructor == null) throw new StartupSettingException("找不符合条件的DataAccessorFactory构造器");
                IDataAccessorFactory factory = FastReflectionExtensions.FastInvoke(constructor, serviceProvider, item, connectionString) as IDataAccessorFactory;
                if (factory == null) throw new StartupSettingException("IDataAccessorFactory失败");
                repositoryContextOptions.DataAccessorFactories.Add(item.CustomConnectionName, factory);
            }
        }

        protected override IEnumerable<DataAccessorBuilderInfo> FilterDataAccessorBuilderInfo(DataAccessorBuilderInfoColletion dataAccessorBuilderInfos)
        {
            return dataAccessorBuilderInfos.Where(n => n.DataAccessorProviderAssembly.Equals(Assembly.Load(new AssemblyName("BingoX.Repository.AspNetCore.EF"))));
        }


        /// <summary>
        /// 注入数据库拦截器
        /// </summary>
        /// <param name="services"></param>
        public override void InjectDbIntercepts(IServiceCollection services)
        {
            foreach (var item in dataAccessorBuilderInfos.SelectMany(n=>n. Intercepts.OfType<DbEntityInterceptAttribute>()).Where(n => n.DI != InterceptDIEnum.None))
            {
                if (item.Intercept != null) continue;
                switch (item.DI)
                {
                    case InterceptDIEnum.Scoped:
                        services.AddScoped(item.AopType);
                        break;
                    case InterceptDIEnum.Singleton:
                        services.AddSingleton(item.AopType);
                        break;
                    case InterceptDIEnum.Transient:
                        services.AddTransient(item.AopType);
                        break;
                }
            }
            services.AddScoped<SqlSugarDbEntityInterceptManagement>(serviceProvider =>
            {
                var interceptManagement = new SqlSugarDbEntityInterceptManagement(serviceProvider);
                foreach (var options in dataAccessorBuilderInfos)
                {
                    interceptManagement.AddRangeGlobalIntercepts(options.Intercepts.OfType<DbEntityInterceptAttribute>());
                
                }
                return interceptManagement;
            });
        }
    }
}
