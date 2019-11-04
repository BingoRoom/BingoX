using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class OCRDrivingLicense : OCR
    {
        /// <summary>
        /// 证号
        /// </summary>
        [JsonProperty("id_num")]
        public string IdNum { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [JsonProperty("sex")]
        public string Sex { get; set; }
        /// <summary>
        /// 国籍
        /// </summary>
        [JsonProperty("nationality")]
        public string Nationality { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        [JsonProperty("address")]
        public string Address { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [JsonProperty("birth_date")]
        public string BirthDate { get; set; }
        /// <summary>
        /// 初次领证日期
        /// </summary>
        [JsonProperty("issue_date")]
        public string IssueDate { get; set; }
        /// <summary>
        /// 准驾车型
        /// </summary>
        [JsonProperty("car_class")]
        public string CarClass { get; set; }
        /// <summary>
        /// 有效期限起始日
        /// </summary>
        [JsonProperty("valid_from")]
        public string ValidFrom { get; set; }
        /// <summary>
        /// 有效期限终止日
        /// </summary>
        [JsonProperty("valid_to")]
        public string ValidTo { get; set; }
        /// <summary>
        /// xx市公安局公安交通管理局
        /// </summary>
        [JsonProperty("official_seal")]
        public string OfficialSeal { get; set; }
     
    }
}







