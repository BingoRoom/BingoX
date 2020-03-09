using BingoX.DataAccessor;
using BingoX.Repository;
using BingoX.Repository.AspNetCore;
using BingoX.Repository.AspNetCore.SqlSugar;
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
        public static void AddSqlSugar(this RepositoryContextOptionBuilderInfo builderInfo, Action<DataAccessorBuilderInfo> action)
        {
            DataAccessorBuilderInfo dataAccessorBuilderInfo = new DataAccessorBuilderInfo();
            dataAccessorBuilderInfo.DataAccessorProviderAssembly = typeof(SqlSugarRepositoryContextOptionBuilder).Assembly;
            action(dataAccessorBuilderInfo);
            builderInfo.dataAccessorBuilderInfos.Add(dataAccessorBuilderInfo);
            if(!builderInfo.RepositoryContextOptionBuilders.Any(n => typeof(SqlSugarRepositoryContextOptionBuilder).Equals(n.GetType())))
                builderInfo.RepositoryContextOptionBuilders.Add(new SqlSugarRepositoryContextOptionBuilder(builderInfo));
        }
    }
}
