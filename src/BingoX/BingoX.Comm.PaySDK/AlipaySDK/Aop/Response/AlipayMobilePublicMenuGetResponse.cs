using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMenuGetResponse.
    /// </summary>
    public class AlipayMobilePublicMenuGetResponse : AopResponse
    {
    

        /// <summary>
        /// 菜单内容
        /// </summary>
        [XmlElement("menu_content")]
        public string MenuContent { get; set; }

       
    }
}
