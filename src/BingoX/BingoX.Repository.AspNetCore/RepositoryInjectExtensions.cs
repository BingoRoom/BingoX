using BingoX.DataAccessor;
using BingoX.Repository;
using BingoX.Repository.AspNetCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryInjectExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration config, Action<RepositoryContextOptionBuilderInfo> action)
        {
            var rcobItme = new RepositoryContextOptionBuilderInfo();
            action(rcobItme);
            services.AddSingleton<RepositoryContextOptionBuilderInfo>(rcobItme);

            var rcob = new BaseRepositoryContextOptionBuilder(rcobItme);
            services.AddScoped<RepositoryContextOptions>(serviceProvider => {
                var rco = new RepositoryContextOptions(rcobItme.Mapper);
                var rcobService = serviceProvider.GetService<RepositoryContextOptionBuilderInfo>();
                foreach (var item in rcobService.RepositoryContextOptionBuilders)
                {
                    item.ReplenishDataAccessorFactories(serviceProvider, config, rco);
                }
                return rco;
            });
            foreach (var item in rcobItme.RepositoryContextOptionBuilders)
            {
                item.InjectDbIntercepts(services);
            }
            foreach (var item in rcob.CreateBaseRepositoryType())
            {
                services.AddScoped(item.BaseType, item.ImplementedType);
                services.AddScoped(item.ImplementedType);
            }
            foreach (var item in rcob.CreateRepositoryType())
            {
                services.AddScoped(item);
            }
            return services;
        }
    }
}
