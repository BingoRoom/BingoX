using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class BathchMaterial
    {
        [JsonProperty("total_count")]
        public int TotalCount { get; set; }
        [JsonProperty("item_count")]
        public int ItemCount { get; set; }
        [JsonProperty("item")]
        public MaterialItem[] Item { get; set; }

    }
}







