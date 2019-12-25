using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicLabelUserQueryResponse.
    /// </summary>
    public class AlipayMobilePublicLabelUserQueryResponse : AopResponse
    { 

        /// <summary>
        /// 标签编号，英文逗号分隔。
        /// </summary>
        [XmlElement("label_ids")]
        public string LabelIds { get; set; }
         
    }
}
