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
        static DbEntityInterceptServiceCollectionExtensions()
        {
            Options = new DbEntityInterceptOptions();
        }
        internal static DbEntityInterceptOptions Options { get; private set; }
        static IServiceProvider applicationServices;
        internal static IServiceProvider ApplicationServices
        {
            get
            {
                if (IsMVCWeb)
                {
                    return applicationServices.GetService<Microsoft.AspNetCore.Http.IHttpContextAccessor>().HttpContext.RequestServices;
                }
                return applicationServices;
            }
        }
        static bool IsMVCWeb;
        public static void AddDbEntityInterceptWithMVC(this IServiceCollection services, Action<DbEntityInterceptOptions> setupAction)
        {
            AddDbEntityIntercept(services, setupAction);
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            IsMVCWeb = true;
        }

        public static void AddDbEntityIntercept(this IServiceCollection services, Action<DbEntityInterceptOptions> setupAction)
        {

            setupAction(Options);
            services.AddSingleton<EfDbEntityInterceptManagement>();
        }

        public static void UseDbEntityIntercept(this IApplicationBuilder app)
        {
            applicationServices = app.ApplicationServices;
        }
    }
#endif
}
