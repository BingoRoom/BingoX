using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.AspNetCore.Http;
using BingoX.Helper;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace BingoX.AspNetCore.TagHelpers
{

    [HtmlTargetElement("bg-sidebar-menu")]
    public class SidebarMenuTagHelper : TagHelper
    {
        private readonly HttpContext httpContext;

        public SidebarMenuTagHelper(IHttpContextAccessor httpContextAccessor)
        {
            httpContext = httpContextAccessor.HttpContext;
        }
        public SidebarMenu[] Menus { get; set; }
        public string Header { get; set; }


        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (Menus == null) return;
            TagBuilder builder = new TagBuilder("section");
            builder.AddCssClass("sidebar");
            builder.MergeAttribute("style", "height: auto;");

            TagBuilder ulbuilder = new TagBuilder("ul");
            ulbuilder.AddCssClass("sidebar-menu");
            ulbuilder.MergeAttribute("data-widget", "tree");

            if (!string.IsNullOrEmpty(Header))
            {
                TagBuilder liHeaderbuilder = new TagBuilder("li");
                liHeaderbuilder.AddCssClass("header");
                liHeaderbuilder.InnerHtml.Append(Header);
                ulbuilder.InnerHtml.AppendHtml(liHeaderbuilder);
            }
            var url = httpContext.Request.Path;
            foreach (var item in Menus)
            {
                TagBuilder liHeaderbuilder = new TagBuilder("li");


                TagBuilder liAbuilder = new TagBuilder("a");
                //           liAbuilder.AddCssClass("treeview");
                if (string.IsNullOrEmpty(item.Url) || item.Url == "/#")
                {
                    liAbuilder.MergeAttribute("href", "javascript:void()");
                    //   liAbuilder.MergeAttribute("click", $"{Click}('{item.Url}')");
                }
                else
                {
                    liAbuilder.MergeAttribute("href", item.Url);
                }
                if (!string.IsNullOrEmpty(item.Icon))
                {
                    TagBuilder iconbuilder = new TagBuilder("i");
                    iconbuilder.AddCssClass(item.Icon);
                    //       iconbuilder.MergeAttribute("href", "javascript:void()");
                    liAbuilder.InnerHtml.AppendHtml(iconbuilder);
                }
                liAbuilder.InnerHtml.AppendHtml($"<span>{item.Name}</span>");
                if (item.Childs.IsEmpty())
                {

                    liHeaderbuilder.InnerHtml.AppendHtml(liAbuilder);
                    ulbuilder.InnerHtml.AppendHtml(liHeaderbuilder);
                    continue;
                }
                liHeaderbuilder.AddCssClass("treeview");
                TagBuilder pullrightbuilder = new TagBuilder("span");
                pullrightbuilder.AddCssClass("pull-right-container");
                pullrightbuilder.InnerHtml.AppendHtml("<i class=\"fa fa-angle-left pull-right\"></i>");
                liAbuilder.InnerHtml.AppendHtml(pullrightbuilder);
                TagBuilder clildbuilder = new TagBuilder("ul");
                clildbuilder.AddCssClass("treeview-menu");
                bool active = false;
                foreach (var treeviewMenu in item.Childs)
                {
                    TagBuilder clildlibuilder = new TagBuilder("li");
                    if (treeviewMenu.Active) clildlibuilder.AddCssClass("active");
                    TagBuilder clildAbuilder = new TagBuilder("a");
                    var itemurl = treeviewMenu.Url;
                    if (string.IsNullOrEmpty(itemurl) || itemurl == "/#") itemurl = "javascript:void()";
                    clildAbuilder.MergeAttribute("href", itemurl);
                    if (string.Equals(url, treeviewMenu.Url, System.StringComparison.CurrentCultureIgnoreCase))
                    {
                        liHeaderbuilder.AddCssClass("menu-open");
                        active = true;

                    }
                    var ico = treeviewMenu.Icon ?? "fa fa-circle-o";
                    if (!string.IsNullOrEmpty(ico))
                    {
                        TagBuilder iconbuilder = new TagBuilder("i");
                        iconbuilder.AddCssClass(ico);
                        //       iconbuilder.MergeAttribute("href", "javascript:void()");
                        clildAbuilder.InnerHtml.AppendHtml(iconbuilder);
                    }
                    clildAbuilder.InnerHtml.AppendHtml($"<span style='white-space: normal;word-wrap: break-word;word-break: break-all;'>{treeviewMenu.Name}</span>");
                    clildlibuilder.InnerHtml.AppendHtml(clildAbuilder);
                    clildbuilder.InnerHtml.AppendHtml(clildlibuilder);
                    /*
                       <ul class="treeview-menu" v-if="menu.children && menu.children.length > 0" v-bind:style="{'display' : menu.active ? 'block' : 'none' }">
                            <li v-for="childmenu in menu.children" v-on:click="onmenuclick(childmenu)" v-bind:class="{active:childmenu.active}">
                                <a href="javascript:void()">
                                    <i v-bind:class="childmenu.icon"></i>
                                    <span style="white-space: normal;word-wrap: break-word;word-break: break-all;">
                                        {{childmenu.name}}
                                    </span>

                                </a>
                            </li>

                        </ul>
                     
                     */
                }

                clildbuilder.MergeAttribute("style", active ? "display:block" : "display:none");
                liHeaderbuilder.InnerHtml.AppendHtml(liAbuilder);
                liHeaderbuilder.InnerHtml.AppendHtml(clildbuilder);
                ulbuilder.InnerHtml.AppendHtml(liHeaderbuilder);
            }
            builder.InnerHtml.AppendHtml(ulbuilder);
            output.Attributes.Add("class", "main-sidebar");
            output.Content.AppendHtml(builder);
            //           output.Content.SetHtmlContent(builder);
            output.TagName = "aside";

        }
    }
}
