using BingoX.AspNetCore;
using BingoX.AspNetCore.Extensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Html;
using System.IO;

namespace System.Web
{
    public static class BingoXHtmlHelper
    {
        readonly static JsonSerializerSettings settings;
        readonly static JsonSerializer jsonSerializer;


        static BingoXHtmlHelper()
        {

            settings = new JsonSerializerSettings
            {
                DateFormatString = "yyyy/MM/dd HH:mm:ss",
                Formatting = Formatting.None,
                FloatParseHandling = FloatParseHandling.Decimal
            };
            settings.Converters.Add(new LongToStingJsonConverter());


            settings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
            jsonSerializer = JsonSerializer.Create(settings);

        }
        public static IBoundedContext GetBoundedContext(this IHtmlHelper helper)
        {
            IServiceProvider applicationServices = helper.ViewContext.HttpContext.RequestServices;
            return applicationServices.GetService<IBoundedContext>();

        }
        public static T GetUser<T>(this IHtmlHelper helper) where T : ICurrentUser
        {
            IServiceProvider applicationServices = helper.ViewContext.HttpContext.RequestServices;
            return applicationServices.GetService<T>();
        }
        public static IHtmlContent RawEntity(this IHtmlHelper helper, object entity)
        {
            StringWriter writer = new StringWriter();
            jsonSerializer.Serialize(writer, entity);
            var jsoncontent = writer.ToString();
            return helper.Raw(jsoncontent);
        }
    }
}
