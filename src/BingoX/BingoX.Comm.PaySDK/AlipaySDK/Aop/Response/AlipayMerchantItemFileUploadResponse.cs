using System;
using System.Xml.Serialization;

namespace Aop.Api.Response
{
    /// <summary>
    /// AlipayMerchantItemFileUploadResponse.
    /// </summary>
    public class AlipayMerchantItemFileUploadResponse : AopResponse
    {
        /// <summary>
        /// 文件在商品中心的素材标识
        /// </summary>
        [XmlElement("material_id")]
        public string MaterialId { get; set; }
    }
}
