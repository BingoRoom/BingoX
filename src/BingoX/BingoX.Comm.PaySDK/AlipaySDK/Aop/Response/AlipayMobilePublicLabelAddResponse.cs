using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicLabelAddResponse.
    /// </summary>
    public class AlipayMobilePublicLabelAddResponse : AopResponse
    {
       

        /// <summary>
        /// 标签编码
        /// </summary>
        [XmlElement("id")]
        public long Id { get; set; }

     

        /// <summary>
        /// 标签名称
        /// </summary>
        [XmlElement("name")]
        public string Name { get; set; }
    }
}
