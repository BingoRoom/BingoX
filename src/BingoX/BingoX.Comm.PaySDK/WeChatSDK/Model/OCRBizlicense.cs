using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class OCRBizlicense : OCR
    {
        /// <summary>
        /// 注册号
        /// </summary>
        [JsonProperty("reg_num")]
        public string RegNum { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        [JsonProperty("serial")]
        public string Serial { get; set; }
        /// <summary>
        /// 法定代表人姓名
        /// </summary>
        [JsonProperty("legal_representative")]
        public string LegalRepresentative { get; set; }
        /// <summary>
        /// 企业名称
        /// </summary>
        [JsonProperty("enterprise_name")]
        public string EnterpriseName { get; set; }
        /// <summary>
        /// 组成形式
        /// </summary>
        [JsonProperty("type_of_organization")]
        public string TypeOfOrganization { get; set; }
        /// <summary>
        /// 经营场所/企业住所
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
        /// <summary>
        /// 公司类型
        /// </summary>
        [JsonProperty("type_of_enterprise")]
        public string TypeOfEnterprise { get; set; }
        /// <summary>
        /// 经营范围
        /// </summary>
        [JsonProperty("business_scope")]
        public string BusinessScope { get; set; }
        /// <summary>
        /// 注册资本
        /// </summary>
        [JsonProperty("registered_capital")]
        public string RegisteredCapital { get; set; }
        /// <summary>
        /// 实收资本
        /// </summary>
        [JsonProperty("paid_in_capital")]
        public string PaidInCapital { get; set; }
        /// <summary>
        /// 营业期限
        /// </summary>
        [JsonProperty("valid_period")]
        public string ValidPeriod { get; set; }
        /// <summary>
        /// 注册日期/成立日期
        /// </summary>
        [JsonProperty("registered_date")]
        public string RegisteredDate { get; set; }
      
    }
}







