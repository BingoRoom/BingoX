using BingoX.Repository;
using BingoX.Repository.AspNetCore.EF;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryInjectExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, Action<RepositoryContextOptionBuilder> action)
        {
            var rcob = new RepositoryContextOptionBuilder();
            action(rcob);
            services.AddSingleton<RepositoryContextOptionBuilder>(rcob);
            services.AddScoped<RepositoryContextOptions>(service => rcob.CreateRepositoryContextOptions(rcob.CreateDataAccessorFactories()));
            List<Type> list = new List<Type>();
            list.AddRange(rcob.CreateRepositoryType());
            list.AddRange(rcob.CreateBaseRepositoryType());
            foreach (var item in list)
            {
                services.AddScoped(item);
            }
            return services;
        }
    }
}
