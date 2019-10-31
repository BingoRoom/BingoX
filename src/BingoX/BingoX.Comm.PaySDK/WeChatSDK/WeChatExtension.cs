#if Standard
using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using BingoX.Extensions;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public static class WeChatExtension
    {
        public static void UseWeChat(this IApplicationBuilder applicationBuilder)
        {
            var config = applicationBuilder.ApplicationServices.GetService<WeChatConfig>();
            applicationBuilder.Use(async (context, next) =>
            {
                if (context.Request.Path.HasValue && context.Request.Path.Value == config.VerifyName) await context.Response.WriteAsync(config.VerifyContent);
                await next();
            });
        }

        public static void AddWeChat(this IServiceCollection services, Func<IServiceProvider, WeChatConfig> configureOptions)
        {
            services.AddSingleton<WeChatConfig>(configureOptions);
            services.AddSingleton<WeChatApi>();
        }

        public static void AddWeChat(this IServiceCollection services)
        {
            services.AddSingleton<WeChatConfig>(n =>
            {
                var configuration = n.GetService<IConfiguration>();
                var configFile = configuration.GetConfigurationByFileName("appsettings.json");
                string appID;
                configFile.TryGet("Wechat:AppID", out appID);
                string appSecret;
                configFile.TryGet("Wechat:AppSecret", out appSecret);
                string grantType;
                configFile.TryGet("Wechat:GrantType", out grantType);
                string verifyContent;
                configFile.TryGet("Wechat:VerifyContent", out verifyContent);
                string verifyName;
                configFile.TryGet("Wechat:VerifyName", out verifyName);
                WeChatConfig config = new WeChatConfig(appID, appSecret, grantType, verifyContent, verifyName);
                return config;
            });
            services.AddSingleton<WeChatApi>();
        }
    }
}
#endif