using System;
using System.Xml.Serialization;

namespace Aop.Api.Domain
{
    /// <summary>
    /// AntMerchantExpandItemDeleteModel Data Structure.
    /// </summary>
    [Serializable]
    public class AntMerchantExpandItemDeleteModel : AopObject
    {
        /// <summary>
        /// 商品id
        /// </summary>
        [XmlElement("item_id")]
        public string ItemId { get; set; }
    }
}
