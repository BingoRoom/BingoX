

#if NETCOREAPP3_1
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;


namespace BingoX.DynamicSearch
{


    public static class DynamicSearchExtensions
    {


        static DynamicShearchOption dynamicQueryOption = new DynamicShearchOption();
        private static IMvcBuilder AddDynamicQuery(this IMvcBuilder builder, Action<DynamicShearchOption> action = null)
        {
            action(dynamicQueryOption);
            var configfile = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DynamicSearch.json");
            if (dynamicQueryOption.Tables.Count == 0 && System.IO.File.Exists(configfile)) dynamicQueryOption.LoadConfig(configfile);

            builder.ConfigureApplicationPartManager(m =>
            {
                //把服务注册成API
                //m.ApplicationParts.Add(new AssemblyPart(typeof(IService).Assembly));
                //foreach (var item in option.Modules)
                //{
                //    item.Initialize();//初始化模块
                //                      //将模块添加到ApplicationParts，这样才能发现服务类
                //    var assembly = item.GetType().Assembly;
                //    m.ApplicationParts.Add(new AssemblyPart(assembly));
                //}
                //m.FeatureProviders.Add(new ApiFeatureProvider());
            });

            builder.Services.Configure<MvcOptions>(o =>
            {
                o.Conventions.Add(new ApiConvention());
            });
            return builder;
        }

        public static IMvcBuilder AddDynamicQuery(this IMvcBuilder builder)
        {
            return AddDynamicQuery(builder, x =>
            {
            });


        }
    }
}

#endif