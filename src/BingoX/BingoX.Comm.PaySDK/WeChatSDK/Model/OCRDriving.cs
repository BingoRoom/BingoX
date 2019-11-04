using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class OCRDriving : OCR
    {
        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty("plate_num")]
        public string PlateNum { get; set; }
        /// <summary>
        /// 车牌号码
        /// </summary>
        [JsonProperty("plate_num_b")]
        public string PlateNumB { get; set; }
        /// <summary>
        /// 所有人
        /// </summary>
        [JsonProperty("owner")]
        public string Owner { get; set; }
        /// <summary>
        /// 车辆类型
        /// </summary>
        [JsonProperty("vehicle_type")]
        public string VehicleType { get; set; }
        /// <summary>
        /// 住址
        /// </summary>
        [JsonProperty("addr")]
        public string Address { get; set; }
        /// <summary>
        /// 使用性质
        /// </summary>
        [JsonProperty("use_character")]
        public string UseCharacter { get; set; }
        /// <summary>
        /// 品牌型号
        /// </summary>
        [JsonProperty("model")]
        public string Model { get; set; }
        /// <summary>
        /// 车辆识别代号
        /// </summary>
        [JsonProperty("vin")]
        public string Vin { get; set; }
        /// <summary>
        /// 发动机号码
        /// </summary>
        [JsonProperty("engine_num")]
        public string EngineNum { get; set; }
        /// <summary>
        /// 注册日期
        /// </summary>
        [JsonProperty("register_date")]
        public string RegisterDate { get; set; }
        /// <summary>
        /// 发证日期
        /// </summary>
        [JsonProperty("issue_date")]
        public string IssueDate { get; set; }
        /// <summary>
        /// 号牌
        /// </summary>
        [JsonProperty("record")]
        public string Record { get; set; }
        /// <summary>
        /// 核定载人数
        /// </summary>
        [JsonProperty("passengers_num")]
        public string PassengersNum { get; set; }
        /// <summary>
        /// 总质量
        /// </summary>
        [JsonProperty("total_quality")]
        public string TotalQuality { get; set; }
        /// <summary>
        /// 整备质量
        /// </summary>
        [JsonProperty("prepare_quality")]
        public string PrepareQuality { get; set; }
        /// <summary>
        /// 外廓尺寸
        /// </summary>
        [JsonProperty("overall_size")]
        public string OverallSize { get; set; }

    }
}







