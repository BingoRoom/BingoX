using BingoX.Repository.AspNetCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class RepositoryInjectExtensions
    {
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration configuration, Action<RepositoryContextOptionBuilder> action)
        {
            var rcob = new RepositoryContextOptionBuilder(services, configuration);
            action(rcob);
            rcob.Build();
            return services;
        }
    }
}
