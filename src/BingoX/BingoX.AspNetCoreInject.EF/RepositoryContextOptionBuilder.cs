using BingoX.DataAccessor;
using BingoX.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using BingoX.DataAccessor.EF;
using BingoX.Helper;
using BingoX.ComponentModel;
using System;

namespace BingoX.AspNetCoreInject.EF
{
    /// <summary>
    /// 仓储上下文选项构建器
    /// </summary>
    public class RepositoryContextOptionBuilder
    {
        private readonly IServiceCollection services;

        private static readonly IDictionary<Type, Type> repositoryMappingCache = new Dictionary<Type, Type>();

        public RepositoryContextOptionBuilder(IServiceCollection services)
        {
            dataAccessorBuilderInfos = new DataAccessorBuilderInfoColletion();
            this.services = services;
        }
        /// <summary>
        /// 数据访问器构建器信息集合
        /// </summary>
        public DataAccessorBuilderInfoColletion dataAccessorBuilderInfos{ get; protected set; }
        /// <summary>
        /// 仓储程序集
        /// </summary>
        public Assembly RepositoryAssembly { get; set; }
        /// <summary>
        /// 领域聚合、实体、值对像与数据库实体之间的映射器
        /// </summary>
        public IRepositoryMapper Mapper { get; set; }

        /// <summary>
        /// 创建数据访问器工厂
        /// </summary>
        /// <returns></returns>
        public IDictionary<string, IDataAccessorFactory> CreateDataAccessorFactories()
        {
            IDictionary<string, IDataAccessorFactory> dict = new Dictionary<string, IDataAccessorFactory>();
            foreach (var item in dataAccessorBuilderInfos)
            {
                var factoryType = typeof(EFDataAccessorFactory<>).MakeGenericType(item.DbContextType);
                var constructor = factoryType.GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 2);
                if (constructor == null) throw new StartupSettingException("找不符合条件的DataAccessorFactory构造器");
                IDataAccessorFactory factory = FastReflectionExtensions.FastInvoke(constructor, services, item) as IDataAccessorFactory;
                if(factory == null) throw new StartupSettingException("IDataAccessorFactory失败");
                dict.Add(item.CustomConnName, factory);
            }
            return dict;
        }

        public RepositoryContextOptions CreateRepositoryContextOptions(IDictionary<string, IDataAccessorFactory> dict)
        {
            if (Mapper == null) throw new RepositoryException("IRepositoryMapper为空，构建仓储失败");
            var rco = new RepositoryContextOptions(Mapper);
            foreach (var item in dict)
            {
                rco.DataAccessorFactories.Add(item.Key, item.Value);
            }
            return rco;
        }

        public IDictionary<Type, Type> CreateRepositoryMapping(IServiceCollection services)
        {
            IDictionary<Type, Type> dict = new Dictionary<Type, Type>();
            foreach (var item in dataAccessorBuilderInfos)
            {
                if (item.RepositoryAssembly == null) throw new StartupSettingException("未配置仓储程序集");
                if (item.DomainEntityAssembly == null) throw new StartupSettingException("未配置领域实体、聚合程序集");
                var assemblyScanInterface = new AssemblyScanInterface(item.RepositoryAssembly, typeof(IRepository));
                var customRepositoryTypes = assemblyScanInterface.Find();
                if (!customRepositoryTypes.Any()) continue;
                foreach (var crt in customRepositoryTypes)
                {
                    if (repositoryMappingCache.ContainsKey(crt))
                    {
                        dict.Add(crt, repositoryMappingCache[crt]);
                        continue;
                    }
                    var assemblyScanClass = new AssemblyScanClass(item.RepositoryAssembly, crt);
                    var customRepositoryImpl = assemblyScanClass.Find();
                    if (!customRepositoryImpl.Any()) continue;
                    repositoryMappingCache.Add(crt, customRepositoryImpl[0]);
                    dict.Add(crt, customRepositoryImpl[0]);
                }
                //todo 这里反射出所有领域实体、聚合、值对像的类型并为其指定使用公共仓储实现

            }
            return dict;
        }
    }
}
