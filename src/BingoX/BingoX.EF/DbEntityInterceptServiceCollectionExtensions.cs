#if Standard
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System;

namespace BingoX.EF
{
#if Standard
    public static class DbEntityInterceptServiceCollectionExtensions
    {
        internal static DbEntityInterceptOptions Options { get; private set; }
        internal static IServiceScopeFactory ServiceScopeFactory { get; private set; }
        internal static IServiceProvider ApplicationServices { get; private set; }


        public static void AddDbEntityIntercept(this IServiceCollection services, Action<DbEntityInterceptOptions> setupAction)
        {
            Options = new DbEntityInterceptOptions();
            setupAction(Options);
            services.AddSingleton<EfDbEntityInterceptManagement>();
        }

        public static void UseDbEntityIntercept(this IApplicationBuilder app)
        {
            ServiceScopeFactory = app.ApplicationServices.GetService<IServiceScopeFactory>();
            ApplicationServices = app.ApplicationServices;
        }
    }
#endif
}
