using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMenuQueryResponse.
    /// </summary>
    public class AlipayMobilePublicMenuQueryResponse : AopResponse
    {
        /// <summary>
        /// 所有菜单列表json串
        /// </summary>
        [XmlElement("all_menu_list")]
        public string AllMenuList { get; set; }

    }
}
