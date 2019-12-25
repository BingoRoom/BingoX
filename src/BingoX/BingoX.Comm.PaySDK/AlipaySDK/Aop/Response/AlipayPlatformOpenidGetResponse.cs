using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayPlatformOpenidGetResponse.
    /// </summary>
    public class AlipayPlatformOpenidGetResponse : AopResponse
    {
       

        /// <summary>
        /// id字典，key为userId和老的openId，value为新的openId
        /// </summary>
        [XmlElement("dict")]
        public string Dict { get; set; }

    }
}
