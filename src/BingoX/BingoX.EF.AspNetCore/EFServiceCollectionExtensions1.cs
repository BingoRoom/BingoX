#if Standard
using Microsoft.Extensions.DependencyInjection;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System;
using BingoX.Repository;
using System.Linq;
using BingoX.EF;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BingoX.Helper;
using BingoX.Domain;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;

namespace BingoX.EF
{

    public static class EFServiceCollectionExtensions
    {

        static ConstructorInfo dbContextConstructor;
        static ParameterInfo optParameter;
        public static void AddBingoEF<TContext>(this IServiceCollection services, Action<BingoEFOptions<TContext>> setupAction) where TContext : EfDbContext
        {
            var options = new BingoEFOptions<TContext>();

            setupAction(options);
            if (options.DbContextOptions == null) throw new BingoX.Repository.RepositoryException("DbContextOptions为空");
            services.AddSingleton<BingoEFOptions>(n => options);
            services.AddSingleton<DbContextOptions>(options.DbContextOptions);
            services.AddSingleton<DbContextOptions<TContext>>(options.DbContextOptions);
            dbContextConstructor = typeof(TContext).GetConstructors().FirstOrDefault();
            optParameter = dbContextConstructor.GetParameters().FirstOrDefault();

            services.AddScoped<TContext>(n => CreateScopedDbContext<TContext>(n));
            services.AddScoped<EfDbContext, TContext>(n =>
            {
                var dbContext = n.GetService<TContext>();
                return dbContext;
            });
            services.AddSingleton<EfDbEntityInterceptManagement>(n =>
            {
                var interceptManagement = new EfDbEntityInterceptManagement(n);
                interceptManagement.AddRangeGlobalIntercepts(options.Intercepts.OfType<DbEntityInterceptAttribute>());
                return interceptManagement;
            });

            RegeditTypes<IDomainFactory>(services, options.AssemblyFactory, typeof(IDomainFactory));
            RegeditTypes<IDomainEventHandler>(services, options.AssemblyDomainEventHandler, typeof(IDomainEventHandler<>));
            RegeditTypes<IRepository>(services, options.AssemblyRepository, typeof(IRepository<,>), typeof(IRepositorySnowflake<>));
            RegeditTypes<IRepositoryBuilder>(services, options.AssemblyRepository);

            foreach (var item in options.Intercepts.OfType<DbEntityInterceptAttribute>().Where(n => n.DI != InterceptDIEnum.None))
            {
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

        }
        static TContext CreateScopedDbContext<TContext>(IServiceProvider serviceProvider) where TContext : EfDbContext
        {
            var optarg = serviceProvider.GetService(optParameter.ParameterType);
            TContext dbContext = FastReflectionExtensions.FastInvoke(dbContextConstructor, optarg) as TContext;
            dbContext.SetServiceProvider(serviceProvider);
            return dbContext;
        }
        private static void RegeditTypes<T>(IServiceCollection services, Assembly ass, params Type[] ignoreInterfaceTypes)
        {
            if (ass == null) return;


            var pairs = GetInheritedInterfaceAndImplementedClass(ass, typeof(T), ignoreInterfaceTypes);
            foreach (var item in pairs)
            {
                services.AddScoped(item.Key);
                item.Value.Select(n => services.AddScoped(n, item.Key)).ToArray();
            }
        }
        /// <summary>
        /// 从当前程序集反射指定类型的所有派生类型
        /// </summary>
        /// <typeparam name="TInterfaceType"></typeparam>
        /// <param name="assembly"></param>
        /// <returns></returns>
        static IDictionary<Type, Type[]> GetInheritedInterfaceAndImplementedClass(Assembly assembly, Type interfaceType, Type[] ignoreInterfaceTypes)
        {

            IDictionary<Type, Type[]> pairs = new Dictionary<Type, Type[]>();
            var implementedClass = assembly.GetTypes().Where(o => o.IsClass && !o.IsAbstract && interfaceType.IsAssignableFrom(o));
            foreach (var item in implementedClass)
            {
                var inheritedInterfaces = item.GetInterfaces().Where(o => (ignoreInterfaceTypes != null && ignoreInterfaceTypes.All(n => n.Name != o.Name)) && interfaceType != o && interfaceType.IsAssignableFrom(o)).ToArray();

                pairs.Add(item, inheritedInterfaces);

            }
            return pairs;
        }


    }
}
