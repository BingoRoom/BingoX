using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class OCRIDCard : OCR
    {

        public string Type { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("addr")]
        public string Address { get; set; }
        [JsonProperty("gender")]
        public string Gender { get; set; }
        [JsonProperty("nationality")]
        public string Nationality { get; set; }
        [JsonProperty("valid_date")]
        public string ValidDate { get; set; }
    }
}







