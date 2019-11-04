using Newtonsoft.Json;

namespace BingoX.Comm.PaySDK.WeChatSDK
{
    public class OCR
    {
        [JsonProperty("errcode")]
        public string Code { get; set; }
        [JsonProperty("errmsg")]
        public string Message { get; set; }
    }
}







