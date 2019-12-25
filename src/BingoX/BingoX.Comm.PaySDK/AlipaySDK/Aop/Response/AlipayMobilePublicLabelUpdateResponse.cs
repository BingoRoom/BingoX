using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicLabelUpdateResponse.
    /// </summary>
    public class AlipayMobilePublicLabelUpdateResponse : AopResponse
    {
   

        /// <summary>
        /// 标签编号
        /// </summary>
        [XmlElement("id")]
        public long Id { get; set; }

       
    }
}
