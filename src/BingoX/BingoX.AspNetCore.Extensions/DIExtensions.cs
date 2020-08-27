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
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Microsoft.Extensions.DependencyInjection
{

    public class IdentitryJwtBearerOption
    {
        public string RSAPublicKey { get; set; }
        public string Authority { get; set; }
        public string Audience { get; set; }
    }
    public class IdentitryOpenIdOption
    {
        public string Authority { get; set; }
        public string ResponseType { get; set; }
        public string WebSiteClientId { get; set; }
        public string[] Scopes { get; set; }
        public string ClientSecret { get; set; }
    }
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
        public static void AddIdentitryJwtBearer(this IServiceCollection services, IConfiguration configuration)
        {
            var opt = new IdentitryJwtBearerOption();
            configuration.Bind("JWT", opt);

            if (string.IsNullOrEmpty(opt.Audience)) throw new Exception("Audience不能为空");
            if (string.IsNullOrEmpty(opt.RSAPublicKey)) opt.RSAPublicKey = "MIIBCgKCAQEAqEztXyQqMD/+sB6kIcWWR6Vsh0JdNbuQQ+hGEexQZxN6ml5i2rbWRVpzyjxJNKKutHB276/wBZTKAU7ikk7fbk5QELsc7Xf+Xh31YV5uSi2HzuD5IeYxMN6AFx/2cvyIEni8sogC7czRicM/tWo0hyciC55D9gGqqVo2iBrMyil9IFjz4XaVb46f8l22ym16GXY7kvTvrPmleqhLhbdAkgvkrEYmTPulZEa3Vm9h6k4L8gZRZOX3zsfXpEy95iH4tg8NCrHSGzdVUpx6Amg5JhrrukgxCC6icWH0d0fyVkYka+ELMYqYL6HY2aJrsosO7qNUBFBo3S+eBMpXHn7y8QIDAQAB";

            AddIdentitryJwtBearer(services, opt);
        }
        public static void AddIdentitryOpenIdConnect(this IServiceCollection services, IConfiguration configuration)
        {

            var opt = new IdentitryOpenIdOption();
            configuration.Bind("Permission", opt);
            AddIdentitryOpenIdConnect(services, opt);
        }
        public static void AddIdentitryOpenIdConnect(this IServiceCollection services, IdentitryOpenIdOption identitryoption)
        {
            services.AddAuthentication(options =>
            {

                //客户端应用设置使用"Cookies"进行认证
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                //identityserver4设置使用"oidc"进行认证
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                //   options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;

            }).AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
                          .AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
                          {
                              options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                              options.Authority = identitryoption.Authority ?? "http://identityserver.zhjinshui.com/";
                              options.RequireHttpsMetadata = false;
                              options.ClientId = identitryoption.WebSiteClientId;
                              options.ClientSecret = identitryoption.ClientSecret ?? "";
                              options.ResponseType = identitryoption.ResponseType ?? OpenIdConnectResponseType.CodeIdToken;
                              options.SaveTokens = true;
                              options.GetClaimsFromUserInfoEndpoint = true;
                              foreach (var item in identitryoption.Scopes)
                              {
                                  options.Scope.Add(item);
                              }

                              options.Scope.Add("offline_access");

                              options.ClaimActions.MapJsonKey("website", "website");
                              options.TokenValidationParameters.NameClaimType = "name";


                          });
        }

        public static void AddIdentitryJwtBearer(this IServiceCollection services, IdentitryJwtBearerOption identitryJwt)
        {
            string pubcont = identitryJwt.RSAPublicKey;
            var rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPublicKey(new ReadOnlySpan<byte>(Convert.FromBase64String(pubcont)), out int red2s);
            var key = new RsaSecurityKey(rsa);
            services.AddAuthentication("Bearer")
                         .AddJwtBearer(options =>
                         {
                             options.Authority = identitryJwt.Authority ?? "http://IdentityServer.zhjinshui.com";
                             options.RequireHttpsMetadata = false;
                             options.Audience = identitryJwt.Audience;
                             options.TokenValidationParameters = new TokenValidationParameters
                             {
                                 NameClaimType = JwtClaimTypes.Name,
                                 RoleClaimType = JwtClaimTypes.Role,
                                 ValidateIssuerSigningKey = true,
                                 RequireExpirationTime = false,
                                 ValidateLifetime = false,
                                 IssuerSigningKey = key
                             };

                         });
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
