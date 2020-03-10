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
        static readonly Type[] ignores = { typeof(IRepository) };
        static readonly RepositoryContextOptionBuilderInfo rcobItme = new RepositoryContextOptionBuilderInfo();
        public static IServiceCollection AddRepository(this IServiceCollection services, IConfiguration config, Action<RepositoryContextOptionBuilderInfo> action)
        {

            action(rcobItme);         

            var rcob = new BaseRepositoryContextOptionBuilder(rcobItme);
            services.AddScoped<RepositoryContextOptions>(serviceProvider =>
            {
                var rco = new RepositoryContextOptions(rcobItme.Mapper, rcobItme.Intercepts.ToList())
                {
                    DefaultConnectionName = rcobItme.DefaultConnectionName,
                };

                //   var rcobService = serviceProvider.GetService<RepositoryContextOptionBuilderInfo>();
                foreach (var item in rcobItme.RepositoryContextOptionBuilders)
                {
                    item.ReplenishDataAccessorFactories(serviceProvider, config, rco);
                }

                return rco;
            });
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            foreach (var item in rcobItme.RepositoryContextOptionBuilders)
            {
                item.InjectDbIntercepts(services);
            }
            foreach (var item in rcobItme.Intercepts.OfType<DbEntityInterceptAttribute>().Where(n => n.DI != InterceptDIEnum.None))
            {

                if (item.Intercept != null) continue;
                switch (item.DI)
                {
                    case InterceptDIEnum.Scoped:
                        services.AddScoped(item.AopType);
                        break;
                    case InterceptDIEnum.Singleton:
                        services.AddSingleton(item.AopType);
                        break;
                    case InterceptDIEnum.Transient:
                        services.AddTransient(item.AopType);
                        break;
                }

            }
            foreach (var item in rcob.FindBaseRepositoryType())
            {
                foreach (var itemInterfaces in item.ImplementedType.GetInterfaces())
                {
                    if (ignores.Any(n => n == itemInterfaces)) continue;
                    services.AddScoped(itemInterfaces, item.ImplementedType);
                }
                services.AddScoped(item.ImplementedType);
            }
            foreach (var item in rcob.FindImplementedRepositoryType())
            {
                foreach (var itemInterfaces in item.GetInterfaces())
                {
                    if (ignores.Any(n => n == itemInterfaces)) continue;
                    services.AddScoped(itemInterfaces, item);
                }
                services.AddScoped(item);
            }
            return services;
        }
    }
}
