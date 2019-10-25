using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AlipayDataDataserviceAdUserCreateModel Data Structure.
    /// </summary>
    [Serializable]
    public class AlipayDataDataserviceAdUserCreateModel : AopObject
    {
        /// <summary>
        /// 投放账户支付宝PID
        /// </summary>
        [XmlElement("alipay_pid")]
        public string AlipayPid { get; set; }

        /// <summary>
        /// 灯火平台提供给外部系统的访问token
        /// </summary>
        [XmlElement("biz_token")]
        public string BizToken { get; set; }
    }
}
