using BingoX.AspNetCoreInject.EF;
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
            var rcob = new RepositoryContextOptionBuilder(services);
            action(rcob);

            return services;
        }
    }
}
