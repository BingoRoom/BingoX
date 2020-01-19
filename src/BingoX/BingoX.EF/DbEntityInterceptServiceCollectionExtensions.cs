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

namespace BingoX.EF
{

    public static class DbEntityInterceptServiceCollectionExtensions
    {
        static DbEntityInterceptServiceCollectionExtensions()
        {
            Options = new DbEntityInterceptOptions();
        }

        public static DbEntityInterceptOptions Options { get; private set; }
        public static EfDbEntityInterceptManagement InterceptManagement { get; private set; }

#if Standard
        public static void AddDbEntityIntercept(this IServiceCollection services, Action<DbEntityInterceptOptions> setupAction)
        {

            setupAction(Options);
            services.AddSingleton<EfDbEntityInterceptManagement>();
        

            foreach (var item in Options.Intercepts.OfType<DbEntityInterceptAttribute>().Where(n => n.DI != InterceptDIEnum.None))
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

        public static void UseDbEntityIntercept(this IServiceProvider app)
        {
            InterceptManagement = app.GetService<EfDbEntityInterceptManagement>();
            InterceptManagement.AddRangeGlobalIntercepts(Options.Intercepts.OfType<DbEntityInterceptAttribute>());
        }
#endif
    }
}
