using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicShortlinkCreateResponse.
    /// </summary>
    public class AlipayMobilePublicShortlinkCreateResponse : AopResponse
    {
       

        /// <summary>
        /// 短链接url
        /// </summary>
        [XmlElement("shortlink")]
        public string Shortlink { get; set; }
    }
}
