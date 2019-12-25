using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMessageTotalSendResponse.
    /// </summary>
    public class AlipayMobilePublicMessageTotalSendResponse : AopResponse
    {
      

        /// <summary>
        /// 消息ID
        /// </summary>
        [XmlElement("data")]
        public string Data { get; set; }

      
    }
}
