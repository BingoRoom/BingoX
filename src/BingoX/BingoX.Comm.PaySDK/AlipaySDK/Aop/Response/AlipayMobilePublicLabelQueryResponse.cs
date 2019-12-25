using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMobilePublicLabelQueryResponse.
    /// </summary>
    public class AlipayMobilePublicLabelQueryResponse : AopResponse
    {
       

        /// <summary>
        /// 所有标签
        /// </summary>
        [XmlArray("labels")]
        [XmlArrayItem("string")]
        public List<string> Labels { get; set; }

       
    }
}
