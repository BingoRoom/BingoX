using BingoX.ComponentModel;
using BingoX.DataAccessor;
using BingoX.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace BingoX.Repository.AspNetCore
{
    internal class BaseRepositoryContextOptionBuilder : RepositoryContextOptionBuilder
    {
        public BaseRepositoryContextOptionBuilder(RepositoryContextOptionBuilderInfo repositoryContextOptionBuilderInfo) : base(repositoryContextOptionBuilderInfo)
        {
        }

        public override void ReplenishDataAccessorFactories(IServiceProvider serviceProvider, IConfiguration config, RepositoryContextOptions repositoryContextOptions)
        {
        }

        public override void InjectDbIntercepts(IServiceCollection services)
        {
        }

        protected override IEnumerable<DataAccessorBuilderInfo> FilterDataAccessorBuilderInfo(DataAccessorBuilderInfoColletion dataAccessorBuilderInfos)
        {
            return dataAccessorBuilderInfos;
        }


        /// <summary>
        /// 构建自定义仓储的类型
        /// </summary>
        /// <returns></returns>
        public virtual IList<Type> FindImplementedRepositoryType()
        {
            List<Type> list = new List<Type>();
            foreach (var item in DataAccessorBuilderInfos)
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
        public virtual IList<AssemblyScanResult> FindBaseRepositoryType()
        {
            List<AssemblyScanResult> list = new List<AssemblyScanResult>();
            foreach (var item in DataAccessorBuilderInfos)
            {
                if (!item.IsMergeDomianAndDao) continue;
                if (item.DomainEntityAssembly == null) throw new StartupSettingException("未配置领域实体、聚合程序集");
                var assemblyScanClass = new AssemblyScanClass(item.DomainEntityAssembly, typeof(IDomainEntry));
                var domainEntryTypes = assemblyScanClass.Find();
                foreach (var domainEntryType in domainEntryTypes)
                {
                    var type = typeof(IRepository<>).MakeGenericType(domainEntryType);
                    var implementedType = typeof(Repository<>).MakeGenericType(domainEntryType);
                    list.Add(new AssemblyScanResult(type, implementedType));
                }
            }
            return list;
        }
    }
}
