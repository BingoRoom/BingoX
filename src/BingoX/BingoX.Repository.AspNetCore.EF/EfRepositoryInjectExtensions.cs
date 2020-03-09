using BingoX.DataAccessor;
using BingoX.Repository;
using BingoX.Repository.AspNetCore;
using BingoX.Repository.AspNetCore.EF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class EfRepositoryInjectExtensions
    {
        public static void AddEF(this RepositoryContextOptionBuilderInfo builderInfo, Action<DataAccessorBuilderInfo> action)
        {
            DataAccessorBuilderInfo dataAccessorBuilderInfo = new DataAccessorBuilderInfo();
            dataAccessorBuilderInfo.DataAccessorProviderAssembly = typeof(EfRepositoryContextOptionBuilder).Assembly;
            action(dataAccessorBuilderInfo);
            builderInfo.DataAccessorBuilderInfos.Add(dataAccessorBuilderInfo);
            if(!builderInfo.RepositoryContextOptionBuilders.Any(n => typeof(EfRepositoryContextOptionBuilder).Equals(n.GetType())))
                builderInfo.RepositoryContextOptionBuilders.Add(new EfRepositoryContextOptionBuilder(builderInfo));
        }
    }
}
