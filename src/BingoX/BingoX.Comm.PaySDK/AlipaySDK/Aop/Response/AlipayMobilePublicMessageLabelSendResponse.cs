using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicMessageLabelSendResponse.
    /// </summary>
    public class AlipayMobilePublicMessageLabelSendResponse : AopResponse
    {
      

        /// <summary>
        /// 消息ID
        /// </summary>
        [XmlElement("msg_id")]
        public string MsgId { get; set; }
    }
}
