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
using BingoX.Domain;

namespace BingoX.Repository.AspNetCore.EF
{
    /// <summary>
    /// 仓储上下文选项构建器
    /// </summary>
    public class RepositoryContextOptionBuilder
    {
         

        public RepositoryContextOptionBuilder( )
        {
            dataAccessorBuilderInfos = new DataAccessorBuilderInfoColletion();
            
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
                IDataAccessorFactory factory = FastReflectionExtensions.FastInvoke(constructor, "", item) as IDataAccessorFactory;
                if(factory == null) throw new StartupSettingException("IDataAccessorFactory失败");
                dict.Add(item.CustomConnectionName, factory);
            }
            return dict;
        }
        /// <summary>
        /// 构建仓储上下文选项
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 构建自定义仓储的类型
        /// </summary>
        /// <returns></returns>
        public IList<Type> CreateRepositoryType()
        {
            List<Type> list = new List<Type>();
            foreach (var item in dataAccessorBuilderInfos)
            {
                if (item.RepositoryAssembly == null) throw new StartupSettingException("未配置仓储程序集");
                var assemblyScanClass = new AssemblyScanClass(item.RepositoryAssembly, typeof(IRepository));
                list.AddRange(assemblyScanClass.Find());
            }
            return list;
        }
        /// <summary>
        /// 构建未实现仓储的领域实体的仓储类型
        /// </summary>
        /// <returns></returns>
        public IList<Type> CreateBaseRepositoryType()
        {
            List<Type> list = new List<Type>();
            foreach (var item in dataAccessorBuilderInfos)
            {
                if (!item.IsMergeDomianAndDao) continue;
                if (item.DomainEntityAssembly == null) throw new StartupSettingException("未配置领域实体、聚合程序集");
                var assemblyScanClass = new AssemblyScanClass(item.RepositoryAssembly, typeof(IDomainEntry));
                var domainEntryTypes = assemblyScanClass.Find();
                foreach (var domainEntryType in domainEntryTypes)
                {
                    var type = typeof(Repository<>).MakeGenericType(domainEntryType);
                    list.Add(type);
                }
            }
            return list;
        }
    }
}
