using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class MaterialItem
    {
        [JsonProperty("media_id")]
        public string MediaId { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("update_time")]
        public string UpdateTime { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}







