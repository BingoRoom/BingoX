using AutoMapper;
using BingoX.AspNetCore.Extensions;
using BingoX.ExceptionProviders;
using BingoX.Generator;
using BingoX.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using BingoX.Helper;
using BingoX.AspNetCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DIExtensions
    {
        public static void FindConfigureServices(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {

            var diConfigures = assembly.GetTypes().Where(n => typeof(DIConfigureServices).IsAssignableFrom(n) && n.IsClass && !n.IsAbstract).OrderBy(n => n.GetCustomAttribute<DIConfigureServicesAttribute>().Order).ToArray();
            var pars = new object[] { configuration, services };
            foreach (var item in diConfigures)
            {
                var constructor = item.GetConstructors().First();
                constructor.Invoke(pars);
            }
        }
        public static void FindConfigureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = Assembly.GetCallingAssembly();
            FindConfigureServices(services, configuration, assembly);
        }
        public static void AddStandard(this IServiceCollection services)
        {
            var assembly = Assembly.GetCallingAssembly();
            AddStandard(services, assembly);
        }

    
        public static IMvcBuilder AddStandard(this IServiceCollection services, Assembly assembly)
        {
            var osname = "Windows";

            if (Environment.OSVersion.Platform == PlatformID.MacOSX) osname = "Mac";
            if (Environment.OSVersion.Platform == PlatformID.Unix) osname = "Linux";
            if (Environment.OSVersion.Platform == PlatformID.Xbox) osname = "Xbox";
            services.AddSingleton<IBoundedContext>(c => new ScopeBoundedContext
            {
                Generator = c.GetService<IGenerator<long>>(),
                DateTimeService = c.GetService<IDateTimeService>(),
                ContentRootPath = c.GetService<IWebHostEnvironment>().ContentRootPath,
                WebRootPath = c.GetService<IWebHostEnvironment>().WebRootPath,
                IsProduction = c.GetService<IWebHostEnvironment>().IsProduction(),
                OS = osname,
                AppVersion = assembly.GetVersion().ToString(),
                AppName = "珠海金税格力比对系统"
            });
            FaultExceptionProvider.Add(new GenericFaultExceptionProvider<UnauthorizedException>());
            FaultExceptionProvider.Add(new GenericFaultExceptionProvider<NotFoundEntityException>());
            FaultExceptionProvider.Add(new GenericFaultExceptionProvider<ForbiddenException>());
            FaultExceptionProvider.Add(new GenericFaultExceptionProvider<BadRequestException>());
            services.AddHttpContextAccessor();
            services.Configure<IISOptions>(options =>
            {
                options.AutomaticAuthentication = false;
                options.ForwardClientCertificate = false;
            });
            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.AddSingleton<IValidatorFactory, ServiceProviderValidatorFactory>();
            var mvcbuilder = services.AddControllersWithViews()
                 .AddNewtonsoftJson(options =>
                 {
                     options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                     options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                     options.SerializerSettings.Converters.Add(new EnumToStingJsonConverter());
                     options.SerializerSettings.Converters.Add(new LongToStingJsonConverter());
                     options.SerializerSettings.Converters.Add(new DateJsonConverter());
                 }).AddMvcOptions(options =>
                 {
         
                     options.Filters.Add<StandardExceptionFilterAttribute>();
                 })
                .AddFluentValidation(n =>
                {
                    n.RegisterValidatorsFromAssembly(assembly);
                });
            services.AddCors(options =>
            {
                // this defines a CORS policy called "default"
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()//.WithOrigins("http://www.jsclient.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            services.AddAutoMapper(assembly);
            var WorkerId = GetWorkerId();
            var DatacenterId = GetDatacenterId();
            services.AddSingleton<IGenerator<long>, SnowflakeGenerator>(c => new SnowflakeGenerator(WorkerId, DatacenterId));
            return mvcbuilder;
        }
        const int MaxWorkerId = 30;
        const int MaxDatacenterId = 30;
        /// <summary>
        /// 获取机器Id
        /// </summary>
        /// <returns></returns>
        private static int GetWorkerId()
        {
            var workerId = 0;

            try
            {
                IPAddress ipaddress = IPAddress.Parse("0.0.0.0");
                NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
                foreach (NetworkInterface ni in interfaces)
                {
                    if ((ni.NetworkInterfaceType == NetworkInterfaceType.Ethernet ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Wireless80211) &&
                        ni.OperationalStatus == OperationalStatus.Up
                        )
                    {
                        foreach (UnicastIPAddressInformation ip in
                            ni.GetIPProperties().UnicastAddresses)
                        {
                            if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                ipaddress = ip.Address;


                            }
                        }
                    }
                }
                var val = ipaddress.GetAddressBytes().Sum(x => x);
                // 取余
                workerId = val % (int)MaxWorkerId;
            }
            catch
            {
                // 异常的话，生成一个随机数
                workerId = new Random().Next((int)MaxWorkerId - 1);
            }

            return workerId;
        }

        /// <summary>
        /// 获取数据中心Id
        /// </summary>
        /// <returns></returns>
        private static int GetDatacenterId()
        {
            var hostName = Dns.GetHostName();

            var val = System.Text.Encoding.UTF8.GetBytes(hostName).Sum(x => x);
            // 取余
            return val % (int)MaxDatacenterId;
        }
    }
}
