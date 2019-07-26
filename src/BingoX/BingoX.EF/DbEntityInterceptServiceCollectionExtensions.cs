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
        static IServiceProvider applicationServices;
        internal static IServiceProvider ApplicationServices
        {
            get
            {
                if (IsMVCWeb)
                {
                    var accessor = applicationServices.GetService(typeof(Microsoft.AspNetCore.Http.IHttpContextAccessor)) as Microsoft.AspNetCore.Http.IHttpContextAccessor;
                    return accessor.HttpContext.RequestServices;
                }
                return applicationServices;
            }
        }
        static bool IsMVCWeb;
        public static void AddDbEntityInterceptWithMVC(this IServiceCollection services, Action<DbEntityInterceptOptions> setupAction)
        {
            AddDbEntityIntercept(services, setupAction);
            services.AddHttpContextAccessor();//<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            IsMVCWeb = true;
        }

        public static void AddDbEntityIntercept(this IServiceCollection services, Action<DbEntityInterceptOptions> setupAction)
        {
            Options = new DbEntityInterceptOptions();
            setupAction(Options);
            services.AddScoped<EfDbEntityInterceptManagement>();
        }

        public static void UseDbEntityIntercept(this IApplicationBuilder app)
        {
            applicationServices = app.ApplicationServices;
        }
    }
#endif
}
