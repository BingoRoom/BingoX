using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMessagePushResponse.
    /// </summary>
    public class AlipayMobilePublicMessagePushResponse : AopResponse
    {

        /// <summary>
        /// 消息ID
        /// </summary>
        [XmlElement("data")]
        public string Data { get; set; }

    }
}
