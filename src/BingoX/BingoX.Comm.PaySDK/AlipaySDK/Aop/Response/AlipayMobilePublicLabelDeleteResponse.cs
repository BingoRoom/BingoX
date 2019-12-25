using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicLabelDeleteResponse.
    /// </summary>
    public class AlipayMobilePublicLabelDeleteResponse : AopResponse
    {
     

        /// <summary>
        /// 标签编号
        /// </summary>
        [XmlElement("id")]
        public long Id { get; set; }

    
    }
}
