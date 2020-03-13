using BingoX.ComponentModel;
using BingoX.DataAccessor;
using BingoX.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// 构建未实现仓储的领域实体的仓储类型
        /// </summary>
        /// <returns></returns>
        public virtual AssemblyScanResult[] FindRepositoryType()
        {
            List<AssemblyScanResult> list = new List<AssemblyScanResult>();
            foreach (var item in DataAccessorBuilderInfos)
            {
                if (item.RepositoryAssembly != null)
                {
                    var assemblyScanClass = new AssemblyScanClass(item.RepositoryAssembly, typeof(IRepository));
                    var repositoryTypes = assemblyScanClass.Find();
                    foreach (var itemRepositoryType in repositoryTypes)
                    {
                        foreach (var iteminterfcae in itemRepositoryType.GetInterfaces())
                        {
                            if (iteminterfcae == typeof(IRepository)) continue;
                            if (list.Any(n => n.BaseType == iteminterfcae)) continue;
                            list.Add(new AssemblyScanResult(iteminterfcae, itemRepositoryType));
                        }
                        if (list.Any(n => n.BaseType == itemRepositoryType)) continue;
                        list.Add(new AssemblyScanResult(itemRepositoryType, itemRepositoryType));
                    }

                }
                if (item.IsMergeDomianAndDao)
                {
                    if (item.DomainEntityAssembly == null) throw new StartupSettingException("未配置领域实体、聚合程序集");
                    var assemblyScanClass = new AssemblyScanClass(item.DomainEntityAssembly, typeof(IDomainEntry));
                    var domainEntryTypes = assemblyScanClass.Find();
                    foreach (var domainEntryType in domainEntryTypes)
                    {

                        var itemRepositoryType = typeof(Repository<>).MakeGenericType(domainEntryType);
                        foreach (var iteminterfcae in itemRepositoryType.GetInterfaces())
                        {
                            if (iteminterfcae == typeof(IRepository)) continue;
                            if (list.Any(n => n.BaseType == iteminterfcae)) continue;
                            list.Add(new AssemblyScanResult(iteminterfcae, itemRepositoryType));
                        }
                        if (list.Any(n => n.BaseType == itemRepositoryType)) continue;
                        list.Add(new AssemblyScanResult(itemRepositoryType, itemRepositoryType));
                    }
                }
            }
            return list.ToArray();
        }
    }
}
