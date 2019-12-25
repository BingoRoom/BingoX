using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMessagebatchPushResponse.
    /// </summary>
    public class AlipayMobilePublicMessagebatchPushResponse : AopResponse
    {
     

        /// <summary>
        /// 消息ID
        /// </summary>
        [XmlElement("data")]
        public string Data { get; set; }

    
    }
}
