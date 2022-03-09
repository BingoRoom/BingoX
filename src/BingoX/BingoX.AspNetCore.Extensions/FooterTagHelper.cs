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
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var boundedContext = httpContext.RequestServices.GetService<BingoX.AspNetCore.IBoundedContext>();
            var recordFiling = httpContext.RequestServices.GetService<BingoX.AspNetCore.IRecordFiling>();
            output.TagName = "footer";
            output.Attributes.Add("class", "border-top footer text-muted");
            TagBuilder builderContainer = new TagBuilder("div");
            builderContainer.AddCssClass("container");
            builderContainer.MergeAttribute("style", "margin-right: unset;margin-left: auto;");
            if (recordFiling != null)
            {
                builderContainer.InnerHtml.Append($"Copyright© {recordFiling.CopyrightYear}");
                var url = recordFiling.Url ?? "https://beian.miit.gov.cn/";
                if (!string.IsNullOrEmpty(recordFiling.No)) builderContainer.InnerHtml.AppendHtml($" - <a href='{url}'>工业和信息化部备案管理系统网站 {recordFiling.No}</a>");
                if (!string.IsNullOrEmpty(recordFiling.Company)) builderContainer.InnerHtml.Append($" 主办单位：{recordFiling.Company}");
            }
            builderContainer.InnerHtml.Append($"{boundedContext.AppName} Powered by .NET Core Version {boundedContext.AppVersion} on {boundedContext.OS}");
            output.Content.AppendHtml(builderContainer);
        }

    }
}
