#if Standard
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using BingoX.Extensions;

namespace BingoX.Comm.PaySDK.AlipaySDK
{
    public static class AlipayExtension
    {
     

        public static void AddAlipay(this IServiceCollection services, Func<IServiceProvider, AlipayConfig> configureOptions)
        {
            services.AddSingleton<AlipayConfig>(configureOptions);
            services.AddSingleton<AlipayApi>();
        }

        public static void AddAlipay(this IServiceCollection services)
        {
            services.AddSingleton<AlipayConfig>(n =>
            {
                var configuration = n.GetService<IConfiguration>();
                var configFile = configuration.GetConfigurationByFileName("appsettings.json");
                string appID;
                configFile.TryGet("Alipay:AppID", out appID);
                string publicKey; 
                string privateKey; 
                configFile.TryGet("Alipay:publicKey", out publicKey);
                configFile.TryGet("Alipay:privateKey", out privateKey);
                AlipayConfig config = new AlipayConfig(appID, publicKey, privateKey);
                return config;
            });
            services.AddSingleton<AlipayApi>();
        }
    }
}
#endif