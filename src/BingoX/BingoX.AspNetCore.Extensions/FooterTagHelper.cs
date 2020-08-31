using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace BingoX.AspNetCore.TagHelpers
{
    [HtmlTargetElement("bg-footer")]
    public class FooterTagHelper : TagHelper
    {
        private readonly HttpContext httpContext;

        public FooterTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var boundedContext = httpContext.RequestServices.GetService<BingoX.AspNetCore.IBoundedContext>();
            output.TagName = "footer";
            output.Attributes.Add("class", "border-top footer text-muted");
            TagBuilder builderContainer = new TagBuilder("div");
            builderContainer.AddCssClass("container");
            builderContainer.InnerHtml.Append($"Copyright© {System.DateTime.Now.Year} - {boundedContext.AppName} Powered by .NET Core Version {boundedContext.AppVersion} on {boundedContext.OS}");
            output.Content.AppendHtml(builderContainer);
        }
    }
}
