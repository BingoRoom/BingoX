using BingoX.DataAccessor;
using BingoX.Repository;
using BingoX.Repository.AspNetCore.EF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryInjectExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration config, Action<RepositoryContextOptionBuilder> action)
        {
            var rcob = new RepositoryContextOptionBuilder();
            action(rcob);
            services.AddSingleton<RepositoryContextOptionBuilder>(rcob);
            services.AddScoped<RepositoryContextOptions>(serviceProvider => rcob.CreateRepositoryContextOptions(rcob.CreateDataAccessorFactories(serviceProvider, config)));
            foreach (var item in rcob.CreateBaseRepositoryType())
            {
                services.AddScoped(item.BaseType, item.ImplementedType);
                services.AddScoped(item.ImplementedType);
            }
            foreach (var item in rcob.CreateRepositoryType())
            {
                services.AddScoped(item);
            }
            rcob.InjectDbIntercepts(services);
            return services;
        }
    }
}
