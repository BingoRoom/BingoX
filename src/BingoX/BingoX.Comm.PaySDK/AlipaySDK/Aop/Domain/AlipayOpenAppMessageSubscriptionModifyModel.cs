using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayOpenAppMessageSubscriptionModifyModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayOpenAppMessageSubscriptionModifyModel : AopObject
    {
        /// <summary>
        /// 授权令牌
        /// </summary>
        [XmlElement("auth_token")]
        public string AuthToken { get; set; }

        /// <summary>
        /// 授权类型，例如app_auth表示三方应用授权
        /// </summary>
        [XmlElement("auth_type")]
        public string AuthType { get; set; }

        /// <summary>
        /// 消费该topic消息的通讯协议类型，目前支持HTTP 或者 WebSocket
        /// </summary>
        [XmlElement("comm_type")]
        public string CommType { get; set; }

        /// <summary>
        /// 消息主题名称
        /// </summary>
        [XmlElement("topic")]
        public string Topic { get; set; }
    }
}