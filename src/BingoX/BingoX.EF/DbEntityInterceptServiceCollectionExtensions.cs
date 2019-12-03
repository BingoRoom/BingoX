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
        }

        public static void UseDbEntityIntercept(this IServiceProvider app)
        {
            InterceptManagement = app.GetService<EfDbEntityInterceptManagement>();
            InterceptManagement.AddRangeGlobalIntercepts(Options.Intercepts.OfType<DbEntityInterceptAttribute>());
        } 
#endif
    }
}
