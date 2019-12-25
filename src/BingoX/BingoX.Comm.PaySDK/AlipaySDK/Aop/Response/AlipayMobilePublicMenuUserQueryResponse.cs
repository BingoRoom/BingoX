using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMenuUserQueryResponse.
    /// </summary>
    public class AlipayMobilePublicMenuUserQueryResponse : AopResponse
    {
     

        /// <summary>
        /// 菜单唯一标识
        /// </summary>
        [XmlElement("menu_key")]
        public string MenuKey { get; set; }

    }
}
