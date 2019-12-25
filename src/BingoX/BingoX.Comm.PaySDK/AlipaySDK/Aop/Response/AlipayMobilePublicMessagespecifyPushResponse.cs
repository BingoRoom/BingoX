using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMessagespecifyPushResponse.
    /// </summary>
    public class AlipayMobilePublicMessagespecifyPushResponse : AopResponse
    {
     

        /// <summary>
        /// 消息ID
        /// </summary>
        [XmlElement("data")]
        public string Data { get; set; }

    }
}
