using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class MaterialCount
    {
        [JsonProperty("voice_count")]
        public int VoiceCount { get; set; }
        [JsonProperty("video_count")]
        public int VideoCount { get; set; }
        [JsonProperty("image_count")]
        public int ImageCount { get; set; }
        [JsonProperty("news_count")]
        public int NewsCount { get; set; }
    }
}







